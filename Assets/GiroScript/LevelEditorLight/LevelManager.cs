using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HyperCasual.Core;
namespace Giro
{
    /// <summary>
    /// A class used to hold a reference to the current
    /// level and provide access to other classes.
    /// </summary>
    [ExecuteInEditMode]
    public class LevelManager : MonoBehaviour
    {
        /// <summary>
        /// Returns the LevelManager.
        /// </summary>
        public static LevelManager Instance => s_Instance;

        static LevelManager s_Instance;

        public float Countdown { get => countdown; }
        float countdown;
        float maxCountdown;
        float starttime;
        bool isCountdowning;

        AbstractGameEvent restartCountdownEvent;
        AbstractGameEvent pauseCountdownEvent;
        AbstractGameEvent continueCountdownEvent;
        AbstractGameEvent loseEvent;

        /// <summary>
        /// Returns the LevelDefinition used to create this LevelManager.
        /// </summary>
        public LevelDefinition LevelDefinition
        {
            get => m_LevelDefinition;
            set
            {
                m_LevelDefinition = value;

                //if (m_LevelDefinition != null && PlayerController.Instance != null)
                //{
                //    PlayerController.Instance.SetMaxXPosition(m_LevelDefinition.LevelWidth);
                //}
            }
        }
        LevelDefinition m_LevelDefinition;

        public List<PuzzlePiecePair> puzzlePieceInScene;


        /// <summary>
        /// Call this method to add a Spawnable to the list of active Spawnables.
        /// </summary>
        public void AddStep(PuzzlePiecePair pzPiecePair)
        {
            puzzlePieceInScene.Add(pzPiecePair);
        }

        /// <summary>
        /// Calling this method calls the Reset() method on all Spawnables in this level.
        /// </summary>
        public void ResetLevel()
        {
            maxCountdown = m_LevelDefinition.maxCountdown;
            countdown = maxCountdown;
            for (int i = 0; i < puzzlePieceInScene.Count; i++)
            {
                puzzlePieceInScene[i].leftObj.GetComponent<PuzzlePiece>().Reset();
                puzzlePieceInScene[i].gameObject.SetActive(false);
            }
        }

        void Awake()
        {
            SetupInstance();
            puzzlePieceInScene = new List<PuzzlePiecePair>();

        }

        void OnEnable()
        {
            SetupInstance();
            puzzlePieceInScene = new List<PuzzlePiecePair>();
        }


        void SetupInstance()
        {
            if (s_Instance != null && s_Instance != this)
            {
                if (Application.isPlaying)
                {
                    Destroy(gameObject);
                }
                else
                {
                    DestroyImmediate(gameObject);
                }
                return;
            }

            s_Instance = this;
        }

        void RestartCountdown()
        {
            countdown = maxCountdown;
            starttime = Time.time;
            isCountdowning = true;
        }

        void ContinueCountdown()
        {
            isCountdowning = true;
        }

        void PauseCountdown()
        {
            isCountdowning = false;
        }

        private void Update()
        {
            if (isCountdowning)
            {
                countdown = maxCountdown - Time.time + starttime;
            }
        }
    }
}