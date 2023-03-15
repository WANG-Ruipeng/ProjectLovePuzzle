using System.Collections;
using System.Collections.Generic;
using HyperCasual.Core;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Giro
{
    /// <summary>
    /// A class used to store game state information, 
    /// load levels, and save/load statistics as applicable.
    /// The GameManager class manages all game-related 
    /// state changes.
    /// </summary>
    public class GameManager : MonoBehaviour, IGameEventListener
    {
        /// <summary>
        /// Returns the GameManager.
        /// </summary>
        public static GameManager Instance => s_Instance;
        static GameManager s_Instance;
        static float maxCountdown;
        public float Countdown { get => countdown; }
        float countdown;
        float starttime;

        [SerializeField]
        AbstractGameEvent m_WinEvent;
        [SerializeField]
        AbstractGameEvent m_LoseEvent;
        [SerializeField]
        AbstractGameEvent CountdownEvent;

        LevelDefinition m_CurrentLevel;


        /// <summary>
        /// Returns true if the game is currently active.
        /// Returns false if the game is paused, has not yet begun,
        /// or has ended.
        /// </summary>
        public bool IsPlaying => m_IsPlaying;
        bool m_IsPlaying;
        GameObject m_CurrentLevelGO;
        GameObject m_CurrentTerrainGO;
        GameObject m_LevelMarkersGO;
        static GameObject puzzlePoolGO;

        List<Step> m_ActiveSteps = new List<Step>();

        bool isWaiting;

#if UNITY_EDITOR
        bool m_LevelEditorMode;
#endif

        public void Initialize()
        {

        }
        void Awake()
        {
            if (s_Instance != null && s_Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            s_Instance = this;

#if UNITY_EDITOR
            // If LevelManager already exists, user is in the LevelEditorWindow
            if (LevelManager.Instance != null)
            {
                StartGame();
                m_LevelEditorMode = true;
            }
#endif
        }

        /// <summary>
        /// This method calls all methods necessary to load and
        /// instantiate a level from a level definition.
        /// </summary>
        public void LoadLevel(LevelDefinition levelDefinition)
        {
            m_CurrentLevel = levelDefinition;
            LoadLevel(m_CurrentLevel, ref m_CurrentLevelGO);
            //PlaceLevelMarkers(m_CurrentLevel, ref m_LevelMarkersGO);
            StartGame();
        }

        /// <summary>
        /// This method calls all methods necessary to restart a level,
        /// including resetting the player to their starting position
        /// </summary>
        public void ResetLevel()
        {
            //if (PlayerController.Instance != null)
            //{
            //    PlayerController.Instance.ResetPlayer();
            //}

            //if (CameraManager.Instance != null)
            //{
            //    CameraManager.Instance.ResetCamera();
            //}
            countdown = maxCountdown;
            if (LevelManager.Instance != null)
            {
                countdown = maxCountdown;
                LevelManager.Instance.ResetLevel();
            }
        }

        /// <summary>
        /// This method loads and instantiates the level defined in levelDefinition,
        /// storing a reference to its parent GameObject in levelGameObject
        /// </summary>
        /// <param name="levelDefinition">
        /// A LevelDefinition ScriptableObject that holds all information needed to 
        /// load and instantiate a level.
        /// </param>
        /// <param name="levelGameObject">
        /// A new GameObject to be created, acting as the parent for the level to be loaded
        /// </param>
        public static void LoadLevel(LevelDefinition levelDefinition, ref GameObject levelGameObject)
        {
            if (levelDefinition == null)
            {
                Debug.LogError("Invalid Level!");
                return;
            }

            if (levelGameObject != null)
            {
                if (Application.isPlaying)
                {
                    Destroy(levelGameObject);
                }
                else
                {
                    DestroyImmediate(levelGameObject);
                }
            }

            levelGameObject = new GameObject("LevelManager");
            levelGameObject.AddComponent<LevelManager>().LevelDefinition = levelDefinition;
            LevelManager levelManager = LevelManager.Instance;

            //Transform levelParent = levelGameObject.transform;
            //原代码在这里载入了场景中的所有spawnable，但是拼图游戏或许不需要
            if (puzzlePoolGO != null)
            {
                if (Application.isPlaying)
                {
                    Destroy(puzzlePoolGO);
                }
                else
                {
                    DestroyImmediate(puzzlePoolGO);
                }
            }

            puzzlePoolGO = new GameObject("PuzzlePool");
            puzzlePoolGO.transform.SetParent(levelGameObject.transform);


            var stepsList = levelDefinition.puzzleSteps;
            for (int i = 0; i < stepsList.Length; i++)
            {
                GameObject pzppGO = null;
                if (Application.isPlaying)
                {
                    pzppGO = GameObject.Instantiate(levelDefinition.puzzlePiecePoolPrefab);
                }
                else
                {
                    pzppGO = (GameObject)PrefabUtility.InstantiatePrefab(levelDefinition.puzzlePiecePoolPrefab);
                }
                PuzzlePiecePair pzpp = pzppGO.GetComponent<PuzzlePiecePair>();
                pzppGO.transform.SetParent(puzzlePoolGO.transform);
                pzppGO.name = ("PuzzlePair_" + i);

                if (stepsList[i].lStepPrefab != null)
                {
                    GameObject go = null;
                    if (Application.isPlaying)
                    {
                        go = GameObject.Instantiate(stepsList[i].lStepPrefab, pzppGO.transform);
                    }
                    else
                    {
#if UNITY_EDITOR
                        go = (GameObject)PrefabUtility.InstantiatePrefab(stepsList[i].lStepPrefab, pzppGO.transform);
#endif
                    }
                    pzpp.leftObj = go;
                }
                if (stepsList[i].rStepPrefab != null)
                {
                    GameObject go = null;
                    if (Application.isPlaying)
                    {
                        go = GameObject.Instantiate(stepsList[i].rStepPrefab, pzppGO.transform);
                    }
                    else
                    {
#if UNITY_EDITOR
                        go = (GameObject)PrefabUtility.InstantiatePrefab(stepsList[i].rStepPrefab, pzppGO.transform);
#endif
                    }
                    pzpp.rightObj = go;
                }
                pzppGO.SetActive(false);
                levelManager.AddStep(pzpp);
            }
            maxCountdown = levelDefinition.maxCountdown;
        }

        public void UnloadCurrentLevel()
        {
            if (m_CurrentLevelGO != null)
            {
                GameObject.Destroy(m_CurrentLevelGO);
            }

            if (m_LevelMarkersGO != null)
            {
                GameObject.Destroy(m_LevelMarkersGO);
            }

            if (m_CurrentTerrainGO != null)
            {
                GameObject.Destroy(m_CurrentTerrainGO);
            }

            m_CurrentLevel = null;
        }

        void StartGame()
        {
            ResetLevel();
            m_IsPlaying = true;
        }


        public void OnEventRaised()
        {
            if (isWaiting)
            {
                starttime = Time.time;
            }
            isWaiting = !isWaiting;
        }



        private void Update()
        {
            if (isWaiting) return;
            countdown = maxCountdown - Time.time + starttime;
            UIManager.Instance.GetView<HUD>().TimeLeft = countdown;
            if (countdown <= 0)
            {
                Lose();
                return;
            }
        }
        /// <summary>
        /// Creates and instantiates the StartPrefab and EndPrefab defined inside
        /// the levelDefinition.
        /// </summary>
        /// <param name="levelDefinition">
        /// A LevelDefinition ScriptableObject that defines the start and end prefabs.
        /// </param>
        /// <param name="levelMarkersGameObject">
        /// A new GameObject that is created to be the parent of the start and end prefabs.
        /// </param>
        public static void PlaceOthers(LevelDefinition levelDefinition)
        {


            GameObject background = levelDefinition.background.gameObject;


            if (background != null)
            {
                GameObject go = GameObject.Instantiate(background, new Vector3(background.transform.position.x, background.transform.position.y, 0.0f), Quaternion.identity);
            }

        }


        public void Win()
        {
            m_WinEvent.Raise();

#if UNITY_EDITOR
            if (m_LevelEditorMode)
            {
                ResetLevel();
            }
#endif
        }

        public void Lose()
        {
            m_LoseEvent.Raise();
#if UNITY_EDITOR
            if (m_LevelEditorMode)
            {
                ResetLevel();
            }
#endif
        }
    }
}