using System.Collections;
using System.Collections.Generic;
using HyperCasual.Core;
using UnityEngine;
using System;

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
    public class GameManager : MonoBehaviour
    {
        /// <summary>
        /// Returns the GameManager.
        /// </summary>
        public static GameManager Instance => s_Instance;
        static GameManager s_Instance;
        public float maxCountdown;
        public float Countdown { get => countdown; }
        float countdown;
        float starttime;
        GenericGameEventListener CountdownListener;

        [SerializeField]
        AbstractGameEvent m_WinEvent;
        [SerializeField]
        AbstractGameEvent m_LoseEvent;
        [SerializeField]
        AbstractGameEvent CountdownEvent;

        LevelDefinition m_CurrentLevel;
        const string puzzlePoolGOName = "PuzzlePool";
        const string managerResourcePath = "Managers/";

        /// <summary>
        /// Returns true if the game is currently active.
        /// Returns false if the game is paused, has not yet begun,
        /// or has ended.
        /// </summary>
        public bool IsPlaying => m_IsPlaying;
        bool m_IsPlaying;


        GameObject m_CurrentLevelGO;
        static GameObject puzzlePoolGO;
        static List<PuzzlePiecePair> m_ActiveSteps = new List<PuzzlePiecePair>();

        bool isCountdowning;

#if UNITY_EDITOR
        bool m_LevelEditorMode;
#endif

        void Awake()
        {
            CountdownListener = new GenericGameEventListener();
            CountdownListener.EventHandler += OnCountdownEventRaised;
            CountdownEvent.AddListener(CountdownListener);
            if (s_Instance != null && s_Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            s_Instance = this;

#if UNITY_EDITOR
            //If LevelManager already exists, user is in the LevelEditorWindow
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
            SetManagerData(levelDefinition);
            StartGame();
        }

        public void SetManagerData(LevelDefinition lvdef)
        {
            maxCountdown = lvdef.maxCountdown;

            //保存PuzzlePieceManager的设定
            if (PuzzlePieceManager.Instance)
                ResetPuzzlePieceManager(PuzzlePieceManager.Instance, lvdef);
        }
        public static void ResetPuzzlePieceManager(PuzzlePieceManager instance, LevelDefinition m_CurrentLevel)
        {
            instance.seceondEnterDelay = m_CurrentLevel.seceondEnterDelay;
            instance.leftEnterStartPos = m_CurrentLevel.leftEnterStartPos;
            instance.rightEnterStartPos = m_CurrentLevel.rightEnterStartPos;
            instance.enterAnimationCurve = m_CurrentLevel.enterAnimationCurve;

            instance.leftDownStartPos = m_CurrentLevel.leftDownStartPos;
            instance.rightDownStartPos = m_CurrentLevel.rightDownStartPos;
            instance.downAnimationCurve = m_CurrentLevel.downAnimationCurve;

            instance.leftCombineStartPos = m_CurrentLevel.leftCombineStartPos;
            instance.rightCombineStartPos = m_CurrentLevel.rightCombineStartPos;
            instance.leftEndPos = m_CurrentLevel.leftEndPos;
            instance.rightEndPos = m_CurrentLevel.rightEndPos;
            instance.combineAnimationCurve = m_CurrentLevel.combineAnimationCurve;
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

            isCountdowning = false;
            InputManager.Instance.receiveInput = false;
            countdown = maxCountdown;
            starttime = Time.time;
            if (LevelManager.Instance != null)
            {
                LevelManager.Instance.ResetLevel();
            }
            if (UIManager.Instance != null)
            {
                HUD hudWindow = UIManager.Instance.GetView<HUD>();
                hudWindow.LeftLocked = false;
                hudWindow.RightLocked = false;
                hudWindow.Show();
            }
            puzzlePoolGO = LevelManager.Instance.transform.Find(puzzlePoolGOName).gameObject;
            Moveable[] moveables = puzzlePoolGO.GetComponentsInChildren<Moveable>();
            PuzzlePieceManager.Instance.SetMoveableList(moveables);
            for (int i = 0; i < moveables.Length; i++)
            {
                moveables[i].Reset();
            }

            if (PuzzlePieceManager.Instance != null)//在pairs重置之后manager才能reset
            {
                PuzzlePieceManager.Instance.Reset();
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

            puzzlePoolGO = new GameObject(puzzlePoolGOName);
            puzzlePoolGO.transform.SetParent(levelGameObject.transform);


            var stepsList = levelDefinition.puzzleSteps;
            for (int i = 0; i < stepsList.Length; i++)
            {
                GameObject moveableGO = null;
                moveableGO = (GameObject)GameObject.Instantiate(Resources.Load(levelDefinition.puzzlePiecePairPrefab.name));
                if (moveableGO.GetComponent<PuzzlePiecePair>())
                {
                    PuzzlePiecePair moveable = moveableGO.GetComponent<PuzzlePiecePair>();
                    moveableGO.transform.SetParent(puzzlePoolGO.transform);
                    moveableGO.name = ("PuzzlePair_" + i);

                    if (stepsList[i].lStepPrefab != null)
                    {
                        GameObject go = null;
                        go = (GameObject)GameObject.Instantiate(Resources.Load(stepsList[i].lStepPrefab.name), moveableGO.transform);
                        PuzzlePiece pz = go.GetComponent<PuzzlePiece>();
                        pz.RotateCurve = levelDefinition.rotateCurve;
                        pz.collections = new Collectible[stepsList[i].lCollectiblePrefabs.Length];
                        for (int j = 0; j < stepsList[i].lCollectiblePrefabs.Length; j++)
                        {
                            if (!stepsList[i].lCollectiblePrefabs[j])
                                continue;
                            GameObject collectibleInstance = (GameObject)Instantiate(Resources.Load(stepsList[i].lCollectiblePrefabs[j].name), go.transform);
                            pz.collections[j] = collectibleInstance.GetComponent<Collectible>();
                        }
                        moveable.leftObj = go;
                    }
                    if (stepsList[i].rStepPrefab != null)
                    {
                        GameObject go = null;
                        go = (GameObject)GameObject.Instantiate(Resources.Load(stepsList[i].rStepPrefab.name), moveableGO.transform);
                        PuzzlePiece pz = go.GetComponent<PuzzlePiece>();
                        pz.RotateCurve = levelDefinition.rotateCurve;
                        pz.collections = new Collectible[stepsList[i].rCollectiblePrefabs.Length];
                        for (int j = 0; j < stepsList[i].rCollectiblePrefabs.Length; j++)
                        {
                            if (!stepsList[i].rCollectiblePrefabs[j])
                                continue;

                            GameObject collectibleInstance = (GameObject)Instantiate(Resources.Load(stepsList[i].rCollectiblePrefabs[j].name), go.transform);
                            pz.collections[j] = collectibleInstance.GetComponent<Collectible>();
                        }
                        moveable.rightObj = go;
                    }
                    moveableGO.SetActive(true);
                    levelManager.AddStep(moveable);
                    m_ActiveSteps.Add(moveable);
                }
                else if (moveableGO.GetComponent<Platform>())
                {
                    //Platform moveable = moveableGO.GetComponent<Platform>();
                    moveableGO.transform.SetParent(puzzlePoolGO.transform);
                    moveableGO.name = ("Platform_" + i);
                }
            }
        }

        public void UnloadCurrentLevel()
        {
            if (m_CurrentLevelGO != null)
            {
                GameObject.Destroy(m_CurrentLevelGO);
            }
            m_CurrentLevel = null;
        }

        void StartGame()
        {
            ResetLevel();
            m_IsPlaying = true;
        }




        public void OnCountdownEventRaised()
        {
            if (!isCountdowning)
            {
                starttime = Time.time;
            }
            isCountdowning = !isCountdowning;
            InputManager.Instance.receiveInput = isCountdowning;
        }



        private void Update()
        {
            if (!isCountdowning) return;
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