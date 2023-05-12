using System.Collections;
using System.Collections.Generic;
using HyperCasual.Core;
using UnityEngine;
using System;
using Doozy.Runtime.UIManager.Containers;
using UnityEngine.SceneManagement;
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
		public HUD hud;

		LevelDefinition m_CurrentLevel;
		const string puzzlePoolGOName = "PuzzlePool";

		/// <summary>
		/// Returns true if the game is currently active.
		/// Returns false if the game is paused, has not yet begun,
		/// or has ended.
		/// </summary>
		public bool IsPlaying => m_IsPlaying;
		bool m_IsPlaying;

		public Scene gamePlayScene;
		public GameObject m_CurrentLevelGO;
		static GameObject puzzlePoolGO;
		static List<Movable> m_ActiveSteps = new List<Movable>();

		bool isCountdowning;
		bool winOrLose;


#if UNITY_EDITOR
		bool m_LevelEditorMode;
#endif

		void Awake()
		{
			gamePlayScene = this.gameObject.scene;
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
			if (SceneManager.GetActiveScene() == gamePlayScene)
			{
				StartGame();
				m_LevelEditorMode = true;
			}
			else
			{
				Destroy(GameObject.Find("UIManager"));
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
			if (MovableManager.Instance)
				ResetPuzzlePieceManager(MovableManager.Instance, lvdef);
		}
		public static void ResetPuzzlePieceManager(MovableManager instance, LevelDefinition m_CurrentLevel)
		{
			instance.seceondEnterDelay = m_CurrentLevel.seceondEnterDelay;
			instance.leftEnterStartPos = m_CurrentLevel.lPos1;
			instance.rightEnterStartPos = m_CurrentLevel.rPos1;
			instance.enterAnimationCurve = m_CurrentLevel.puzzlesEnterAnimationCurve;

			instance.leftDownStartPos = m_CurrentLevel.lPos2;
			instance.rightDownStartPos = m_CurrentLevel.rPos2;
			instance.downAnimationCurve = m_CurrentLevel.puzzlesDownAnimationCurve;

			instance.leftCombineStartPos = m_CurrentLevel.lPos3;
			instance.rightCombineStartPos = m_CurrentLevel.rPos3;
			instance.leftEndPos = m_CurrentLevel.lPos4;
			instance.rightEndPos = m_CurrentLevel.rPos4;
			instance.combineAnimationCurve = m_CurrentLevel.puzzlesCombineAnimationCurve;

			instance.leftExitPos = m_CurrentLevel.lPos5;
			instance.rightExitPos = m_CurrentLevel.rPos5;
			instance.exitAnimationCurve = m_CurrentLevel.puzzlesExitAnimationCurve;
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
			if (LevelManager.Instance != null)
			{
				LevelManager.Instance.ResetLevel();
			}

			puzzlePoolGO = LevelManager.Instance.transform.Find(puzzlePoolGOName).gameObject;
			Movable[] movables = puzzlePoolGO.GetComponentsInChildren<Movable>();
			MovableManager.Instance.SetmovableList(movables);
			MovableManager.Instance.InitPuzzles();
			for (int i = 0; i < movables.Length; i++)
			{
				movables[i].Reset();
			}

			if (MovableManager.Instance != null)//在pairs重置之后manager才能reset
			{
				MovableManager.Instance.Reset();
			}
			if (!hud)
				hud = GameObject.Find("HUD").GetComponent<HUD>();
			hud.LeftLocked = false;
			hud.RightLocked = false;
			InputManager.Instance.hud = hud;
			isCountdowning = false;
			InputManager.Instance.receiveInput = false;
			countdown = maxCountdown;
			starttime = Time.time;
			winOrLose = false;
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
			levelGameObject.SetActive(true);
			levelGameObject.AddComponent<LevelManager>();
			LevelManager levelManager = LevelManager.Instance;
			levelManager.LevelDefinition = levelDefinition;

			try
			{
				SceneManager.MoveGameObjectToScene(levelGameObject, Instance.gamePlayScene);
			}
			catch (Exception)
			{ }
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
				if (stepsList[i].isPlatform)
				{
					GameObject movableGO = (GameObject)GameObject.Instantiate(Resources.Load("PuzzlePiece/" + stepsList[i].platformObj.name));

					movableGO.transform.SetParent(puzzlePoolGO.transform);
					movableGO.name = ("Platform_" + i);
					movableGO.SetActive(true);
					Movable movable = movableGO.GetComponent<Platform>();
					((Platform)movable).SetParameter(levelDefinition.pos1, levelDefinition.pos2, levelDefinition.pos3, levelDefinition.pos4,
						levelDefinition.platformEnterAnimationCurve, levelDefinition.platformDownAnimationCurve, levelDefinition.platformExitAnimationCurve);

					levelManager.AddStep(movable);
					m_ActiveSteps.Add(movable);
				}
				else
				{
					GameObject movableGO = null;
					movableGO = (GameObject)GameObject.Instantiate(Resources.Load("PuzzleResources/" + levelDefinition.puzzlePiecePairPrefab.name));
					if (movableGO.GetComponent<PuzzlePiecePair>())
					{
						PuzzlePiecePair movable = movableGO.GetComponent<PuzzlePiecePair>();
						((PuzzlePiecePair)movable).SetParameter(levelDefinition.lPos1, levelDefinition.rPos1, levelDefinition.lPos2, levelDefinition.rPos2,
							levelDefinition.lPos3, levelDefinition.rPos3, levelDefinition.lPos4, levelDefinition.rPos4,
							levelDefinition.lPos5, levelDefinition.rPos5,
							levelDefinition.platformEnterAnimationCurve, levelDefinition.platformDownAnimationCurve,
							levelDefinition.puzzlesCombineAnimationCurve, levelDefinition.platformExitAnimationCurve);

						movableGO.transform.SetParent(puzzlePoolGO.transform);
						movableGO.name = ("PuzzlePair_" + i);

						if (stepsList[i].lStepPrefab != null)
						{
							GameObject go = null;
							go = (GameObject)GameObject.Instantiate(Resources.Load("PuzzlePiece/" + stepsList[i].lStepPrefab.name), movableGO.transform);
							PuzzlePiece pz = go.GetComponent<PuzzlePiece>();
							pz.RotateCurve = levelDefinition.rotateCurve;

							for (int j = 0; j < stepsList[i].lCollectibleInfos.Length; j++)
							{
								if (!stepsList[i].lCollectibleInfos[j].prefab)
									continue;

								GameObject collectibleInstance = (GameObject)Instantiate(Resources.Load("Collectible/" + stepsList[i].lCollectibleInfos[j].prefab.name), go.transform);
								pz.collections.Add(stepsList[i].lCollectibleInfos[j].prefab.GetComponent<Collectible>());
								pz.collections[pz.collections.Count - 1].SetData(stepsList[i].lCollectibleInfos[j]);
							}
							movable.leftObj = go;
						}
						if (stepsList[i].rStepPrefab != null)
						{
							GameObject go = null;
							go = (GameObject)GameObject.Instantiate(Resources.Load("PuzzlePiece/" + stepsList[i].rStepPrefab.name), movableGO.transform);
							PuzzlePiece pz = go.GetComponent<PuzzlePiece>();
							pz.RotateCurve = levelDefinition.rotateCurve;
							for (int j = 0; j < stepsList[i].rCollectibleInfos.Length; j++)
							{
								if (!stepsList[i].rCollectibleInfos[j].prefab)
									continue;

								GameObject collectibleInstance = (GameObject)Instantiate(Resources.Load("Collectible/" + stepsList[i].rCollectibleInfos[j].prefab.name), go.transform);
								pz.collections.Add(stepsList[i].rCollectibleInfos[j].prefab.GetComponent<Collectible>());
								pz.collections[pz.collections.Count - 1].SetData(stepsList[i].rCollectibleInfos[j]);
							}
							movable.rightObj = go;
						}
						movableGO.SetActive(true);
						levelManager.AddStep(movable);
						m_ActiveSteps.Add(movable);
					}
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
				PlayerManager.Instance.SetPlayerIdle();
				PlayerManager.Instance.firtAlmostDown = true;
				starttime = Time.time;
				hud.UpdateValueBar(MovableManager.Instance.Progress, MovableManager.Instance.StepNum);
			}
			else
			{

			}
			isCountdowning = !isCountdowning;
			InputManager.Instance.receiveInput = isCountdowning;
		}



		private void Update()
		{
			if (!isCountdowning) return;
			countdown = maxCountdown - Time.time + starttime;
			hud.TimeLeft = countdown;
			if (PlayerManager.Instance.firtAlmostDown && countdown <= PlayerManager.Instance.almostFallTime)
			{
				PlayerManager.Instance.firtAlmostDown = false;
				PlayerManager.Instance.SetPlayerAlmostFall();
			}
			if (countdown <= 0 && !winOrLose)
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
			winOrLose = true;
			hud.UpdateValueBar(MovableManager.Instance.Progress, MovableManager.Instance.StepNum);

			PlayerManager.Instance.SetPlayerVictory();
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
			winOrLose = true;
			PlayerManager.Instance.SetPlayerFall();
#if UNITY_EDITOR
			if (m_LevelEditorMode)
			{
				ResetLevel();
			}
#endif
		}
	}
}