using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Giro
{
    public class LevelEditor : EditorWindow
    {
        bool m_AutoSaveShowSettings;
        bool hasLoadLevel;  
        bool currentLevelNotLoaded;
        bool m_AutoSaveLevel;
        bool m_AutoSavePlayer;
        bool m_AutoSaveCamera;
        bool m_AutoSaveSettingsLoaded;

        static readonly string s_LevelParentTag = "LevelParent";
        const string levelEditorSceneName = "LevelEditorScene";
        const string levelEditorScenePath = "Assets/GiroScript/LevelEditorLight/Scenes/LevelEditorScene.unity";

        const string k_AutoSaveSettingsInitializedKey = "AutoSaveInitialized";
        const string k_AutoSaveLevelKey = "AutoSaveLevel";
        const string k_AutoSavePlayerKey = "AutoSavePlayer";
        const string k_AutoSaveCameraKey = "AutoSaveCamera";
        const string k_AutoSaveShowSettingsKey = "AutoSaveShowSettings";
        const string k_EditorPrefsPreviouslyLoadedLevelPath = "PreviouslyLoadedLevelPath";

        GameObject loadedLevelGO;
        GameObject levelParentGO;
        LevelDefinition SourceLevelDef;
        LevelDefinition LoadedLevelDef;

        [MenuItem("Window/Level Editor")]
        static void Init()
        {
            LevelEditor window = (LevelEditor)EditorWindow.GetWindow(typeof(LevelEditor), false, "Level Editor");
            window.Show();

            // Load auto-save settings
            window.LoadSetting();
        }

        void OnFocus()
        {
            SceneView.duringSceneGui -= OnSceneGUI;
            SceneView.duringSceneGui += OnSceneGUI;

            EditorApplication.playModeStateChanged -= OnPlayModeChanged;
            EditorApplication.playModeStateChanged += OnPlayModeChanged;

            EditorSceneManager.sceneSaved -= OnSceneSaved;
            EditorSceneManager.sceneSaved += OnSceneSaved;
        }
        void LoadSetting()
        {
            bool autoSaveSettingsInitialized = EditorPrefs.GetBool(k_AutoSaveSettingsInitializedKey);

            if (!autoSaveSettingsInitialized)
            {
                // Default all auto-save values to true and save them to Editor Prefs
                // the first time the user opens the window

                m_AutoSaveLevel = true;
                m_AutoSavePlayer = true;
                m_AutoSaveCamera = true;

                EditorPrefs.SetBool(k_AutoSaveLevelKey, m_AutoSaveLevel);
                EditorPrefs.SetBool(k_AutoSavePlayerKey, m_AutoSavePlayer);
                EditorPrefs.SetBool(k_AutoSaveCameraKey, m_AutoSaveCamera);

                EditorPrefs.SetBool(k_AutoSaveSettingsInitializedKey, true);
                return;
            }

            m_AutoSaveShowSettings = EditorPrefs.GetBool(k_AutoSaveShowSettingsKey);
            m_AutoSaveLevel = EditorPrefs.GetBool(k_AutoSaveLevelKey);
            m_AutoSavePlayer = EditorPrefs.GetBool(k_AutoSavePlayerKey);
            m_AutoSaveCamera = EditorPrefs.GetBool(k_AutoSaveCameraKey);

            m_AutoSaveSettingsLoaded = true;
        }

        void SaveSetting()
        {
            EditorPrefs.SetBool(k_AutoSaveLevelKey, m_AutoSaveLevel);
            EditorPrefs.SetBool(k_AutoSavePlayerKey, m_AutoSavePlayer);
            EditorPrefs.SetBool(k_AutoSaveCameraKey, m_AutoSaveCamera);
            EditorPrefs.SetBool(k_AutoSaveShowSettingsKey, m_AutoSaveShowSettings);
        }

        private void OnGUI()
        {
            if (!m_AutoSaveShowSettings)
                LoadSetting();

            if (Application.isPlaying)
            {
                GUILayout.Label("Exit play mode to continue editing level");
                return;
            }


            Scene scene = SceneManager.GetActiveScene();
            if (!scene.name.Equals(levelEditorSceneName))
            {
                if (GUILayout.Button("Open Level Editor Scene"))
                {
                    EditorSceneManager.OpenScene(levelEditorScenePath);
                    if (SourceLevelDef != null)
                    {
                        LoadLevel(SourceLevelDef);
                    }
                }
                return;
            }

            SourceLevelDef = (LevelDefinition)EditorGUILayout.ObjectField("Level Definition", SourceLevelDef, typeof(LevelDefinition), false, null);

            if (SourceLevelDef == null)
            {
                GUILayout.Label("Select a LevelDefinition ScriptableObject to begin.");
                hasLoadLevel = false;
                return;
            }

            if (LoadedLevelDef != null && !SourceLevelDef.name.Equals(LoadedLevelDef.name))
            {
                // Automatically load the new source level if it has changed.
                LoadLevel(LoadedLevelDef);
                return;
            }

            if (Event.current.type == EventType.Layout)
            {
                currentLevelNotLoaded = LevelNotLoaded();
            }

            if (loadedLevelGO != null && !currentLevelNotLoaded)
            {
                if (GUILayout.Button("Reload Level"))
                {
                    LoadLevel(SourceLevelDef);
                }
            }
            else
            {
                LoadLevel(SourceLevelDef);
            }


            if (LoadedLevelDef == null || currentLevelNotLoaded)
            {
                GUILayout.Label("No level loaded.");
                return;
            }

            if (GUILayout.Button("Save Level"))
            {
                SaveLevel(LoadedLevelDef);
            }

            m_AutoSaveShowSettings = EditorGUILayout.BeginFoldoutHeaderGroup(m_AutoSaveShowSettings, "Auto-Save Settings");

            if (m_AutoSaveShowSettings)
            {
                EditorGUI.BeginChangeCheck();
                m_AutoSaveLevel = EditorGUILayout.Toggle(new GUIContent("Save Level on Play", "Any changes made to the level being edited will be automatically saved when entering play mode."), m_AutoSaveLevel);
                m_AutoSavePlayer = EditorGUILayout.Toggle(new GUIContent("Save Player on Play", "Any changes made to the Player prefab will be automatically saved when entering play mode and reflected when playing the game via the Boot scene."), m_AutoSavePlayer);
                m_AutoSaveCamera = EditorGUILayout.Toggle(new GUIContent("Save Camera on Play", "Any changes made to the GameplayCamera prefab will be automatically saved when entering play mode and reflected when playing the game via the Boot scene."), m_AutoSaveCamera);
                if (EditorGUI.EndChangeCheck())
                {
                    SaveSetting();
                }
            }
            EditorGUILayout.Space();

            //往后保存一些其他东西，比如背景颜色啊什么的，参考源代码350行
            if (GUILayout.Button("Read Prop From GameObject"))
            {
                int len = LoadedLevelDef.puzzleSteps.Length;
                for (int i = 0; i < len; i++)
                {
                    var pzPiece = SourceLevelDef.puzzleSteps[i].lStepPrefab.GetComponentInChildren<PuzzlePiece>();
                    SourceLevelDef.puzzleSteps[i].lEdgeProp = pzPiece.edgeProps;

                    pzPiece = SourceLevelDef.puzzleSteps[i].rStepPrefab.GetComponentInChildren<PuzzlePiece>();
                    SourceLevelDef.puzzleSteps[i].rEdgeProp = pzPiece.edgeProps;
                }
            }

        }
        void UnloadOpenLevels()
        {
            GameObject[] levelParents = GameObject.FindGameObjectsWithTag(s_LevelParentTag);
            for (int i = 0; i < levelParents.Length; i++)
            {
                DestroyImmediate(levelParents[i]);
            }

            levelParentGO = null;
        }
        void LoadLevel(LevelDefinition lvdef)
        {
            UnloadOpenLevels();
            if (!EditorSceneManager.GetActiveScene().path.Equals(levelEditorScenePath))
                return;

            LoadedLevelDef = Instantiate(lvdef);
            LoadedLevelDef.name = lvdef.name;

            levelParentGO = new GameObject("Level Parent");
            levelParentGO.tag = s_LevelParentTag;

            GameManager.LoadLevel(LoadedLevelDef, ref loadedLevelGO);

            loadedLevelGO.transform.SetParent(levelParentGO.transform);

            string levelPath = AssetDatabase.GetAssetPath(lvdef);
            EditorPrefs.SetString(k_EditorPrefsPreviouslyLoadedLevelPath, levelPath);

            //m_AttemptedToLoadPreviousLevel = false;

            Repaint();

        }
        void SaveLevel(LevelDefinition lvdef)
        {
            if (m_AutoSaveLevel)
            {
                //// Update array of spawnables based on what is currently in the scene
                //Step[] steps = (Step[])Object.FindObjectsOfType(typeof(Step));
                //lvdef.puzzleSteps = new LevelDefinition.PuzzleStep[steps.Length];
                //for (int i = 0; i < steps.Length; i++)
                //{
                //    try
                //    {
                //        lvdef.puzzleSteps[i] = new LevelDefinition.PuzzleStep()
                //        {
                //            lPos = i,
                //            lStepPrefab = steps[i].gameObject
                //        };
                //    }
                //    catch (System.Exception e)
                //    {
                //        Debug.LogError(e.ToString());
                //    }
                //}

                //// Overwrite source level definition with current version
                //SourceLevelDef.SaveValues(lvdef);
            }




            // Set level definition dirty so the changes will be written to disk
            EditorUtility.SetDirty(SourceLevelDef);

            // Write changes to disk
            AssetDatabase.SaveAssets();
        }

        bool LevelNotLoaded()
        {
            return LoadedLevelDef == null || levelParentGO == null || loadedLevelGO == null;
        }
        void OnSelectionChange()
        {
            // Needed to update color options when a Spawnable is selected
            Repaint();
        }
        void OnDestroy()
        {
            SceneView.duringSceneGui -= OnSceneGUI;
            EditorApplication.playModeStateChanged -= OnPlayModeChanged;
            EditorSceneManager.sceneSaved -= OnSceneSaved;
        }
        void OnPlayModeChanged(PlayModeStateChange state)
        {
            if ((state == PlayModeStateChange.EnteredEditMode || state == PlayModeStateChange.EnteredPlayMode) && SourceLevelDef != null)
            {
                Scene scene = EditorSceneManager.GetActiveScene();
                if (scene.name.Equals(levelEditorSceneName))
                {
                    // Reload the scene automatically
                    LoadLevel(SourceLevelDef);
                }
            }
            else if (state == PlayModeStateChange.ExitingEditMode && SourceLevelDef != null && !LevelNotLoaded())
            {
                Scene scene = EditorSceneManager.GetActiveScene();
                if (scene.name.Equals(levelEditorSceneName))
                {
                    // Save the scene automatically before testing
                    SaveLevel(LoadedLevelDef);
                }
            }
        }
        void OnSceneSaved(Scene scene)
        {
            if (SourceLevelDef != null && !LevelNotLoaded())
            {
                if (scene.name.Equals(levelEditorSceneName))
                {
                    SaveLevel(LoadedLevelDef);
                }
            }
        }

        void OnSceneGUI(SceneView sceneView)//懒得搞-，-
        {
        }

    }
}