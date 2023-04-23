﻿using System;
using System.Collections.Generic;
using HyperCasual.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Giro
{
	/// <summary>
	/// This singleton determines the state of the game based on observed game events.
	/// </summary>
	[Serializable]
	public class SequenceManager : AbstractSingleton<SequenceManager>
	{
		[SerializeField]
		GameObject[] m_PreloadedAssets;
		[SerializeField]
		AbstractLevelData[] m_Levels;
		[SerializeField]
		GameObject[] m_LevelManagers;
		public AbstractLevelData[] Levels => m_Levels;
		[Header("Events")]
		[SerializeField]
		AbstractGameEvent m_ContinueEvent;
		[SerializeField]
		AbstractGameEvent m_BackEvent;
		[SerializeField]
		AbstractGameEvent m_WinEvent;
		[SerializeField]
		AbstractGameEvent m_LoseEvent;
		[SerializeField]
		AbstractGameEvent m_PauseEvent;

		[Header("Other")]
		[SerializeField]
		float m_SplashDelay = 2f;

		readonly StateMachine m_StateMachine = new();
		IState m_SplashScreenState;
		IState m_MainMenuState;
		IState m_LevelSelectState;
		readonly List<IState> m_LevelStates = new();
		public IState m_CurrentLevel;

		SceneController m_SceneController;

		/// <summary>
		/// Initializes the SequenceManager
		/// </summary>
		public void Initialize()
		{
			m_SceneController = new SceneController(SceneManager.GetActiveScene());

			InstantiatePreloadedAssets();

			m_SplashScreenState = new State(ShowUI<SplashScreen>);
			m_StateMachine.Run(m_SplashScreenState);

			CreateMenuNavigationSequence();
			CreateLevelSequences();
			SetStartingLevel(0);
		}

		void InstantiatePreloadedAssets()
		{
			foreach (var asset in m_PreloadedAssets)
			{
				Instantiate(asset);
			}
		}

		void CreateMenuNavigationSequence()
		{
			//Create states
			var splashDelay = new DelayState(m_SplashDelay);
			m_MainMenuState = new State(OnMainMenuDisplayed);
			m_LevelSelectState = new State(OnLevelSelectionDisplayed);

			//Connect the states
			m_SplashScreenState.AddLink(new Link(splashDelay));
			splashDelay.AddLink(new Link(m_MainMenuState));
			m_MainMenuState.AddLink(new EventLink(m_ContinueEvent, m_LevelSelectState));
			m_LevelSelectState.AddLink(new EventLink(m_BackEvent, m_MainMenuState));
		}

		void CreateLevelSequences()
		{
			m_LevelStates.Clear();

			//Create and connect all level states
			IState lastState = null;

			foreach (var level in m_Levels)
			{
				IState state = null;
				if (level is SceneRef sceneLevel)
				{
					state = CreateLevelState(sceneLevel.m_ScenePath);
				}
				else
				{
					state = CreateLevelState(level);
				}
				lastState = AddLevelPeripheralStates(state, m_LevelSelectState, lastState);

			}

			//Closing the loop: connect the last level to the level-selection state
			//暂时还没有设置最后一关要回到哪里，现在是每过一关都回到一次选关界面
			//var unloadLastScene = new UnloadLastSceneState(m_SceneController);
			//lastState?.AddLink(new EventLink(m_ContinueEvent, unloadLastScene));
			//unloadLastScene.AddLink(new Link(m_LevelSelectState));
		}

		/// <summary>
		/// Creates a level state from a scene
		/// </summary>
		/// <param name="scenePath"></param>
		/// <returns></returns>
		IState CreateLevelState(string scenePath)
		{
			return new LoadSceneState(m_SceneController, scenePath);
		}

		/// <summary>
		/// Creates a level state from a level data
		/// </summary>
		/// <param name="levelData"></param>
		/// <returns></returns>
		IState CreateLevelState(AbstractLevelData levelData)
		{
			return new LoadLevelFromDef(m_SceneController, levelData, m_LevelManagers);
		}

		IState AddLevelPeripheralStates(IState loadLevelState, IState quitState, IState lastState)
		{
			//Create states
			m_LevelStates.Add(loadLevelState);
			var GamePlayState = new State(() => OnGamePlayStarted(loadLevelState));
			var winState = new PauseState(() => OnWinScreenDisplayed(loadLevelState));
			var loseState = new PauseState(() => OnLevelWasLoaded());
			var pauseState = new PauseState(() => OnGamePause());
			var unloadLose = new UnloadLastSceneState(m_SceneController);
			var unloadPause = new UnloadLastSceneState(m_SceneController);

			//Connect the states
			lastState?.AddLink(new EventLink(m_ContinueEvent, loadLevelState));
			loadLevelState.AddLink(new Link(GamePlayState));

			GamePlayState.AddLink(new EventLink(m_WinEvent, winState));
			GamePlayState.AddLink(new EventLink(m_LoseEvent, loseState));
			GamePlayState.AddLink(new EventLink(m_PauseEvent, pauseState));

			loseState.AddLink(new EventLink(m_ContinueEvent, loadLevelState));
			loseState.AddLink(new EventLink(m_BackEvent, unloadLose));
			unloadLose.AddLink(new Link(quitState));

			pauseState.AddLink(new EventLink(m_ContinueEvent, GamePlayState));
			pauseState.AddLink(new EventLink(m_BackEvent, unloadPause));
			unloadPause.AddLink(new Link(m_MainMenuState));

			winState.AddLink(new EventLink(m_ContinueEvent, loadLevelState));
			winState.AddLink(new EventLink(m_BackEvent, unloadLose));

			return winState;
		}

		/// <summary>
		/// Changes the starting gameplay level in the sequence of levels by making a slight change to its links
		/// </summary>
		/// <param name="index">Index of the level to set as starting level</param>
		public void SetStartingLevel(int index)
		{
			m_LevelSelectState.RemoveAllLinks();
			m_LevelSelectState.AddLink(new EventLink(m_ContinueEvent, m_LevelStates[index]));
			m_LevelSelectState.AddLink(new EventLink(m_BackEvent, m_MainMenuState));
			m_LevelSelectState.EnableLinks();
		}

		void ShowUI<T>() where T : View
		{
			UIManager.Instance.Show<T>();
		}

		void OnMainMenuDisplayed()
		{
			ShowUI<MainScreen>();
			AudioManager.Instance.PlayMusic(SoundID.None);
		}

		void OnWinScreenDisplayed(IState currentLevel)
		{
			UIManager.Instance.Show<WinOrLoseScreen>();
			var currentLevelIndex = m_LevelStates.IndexOf(currentLevel);

			if (currentLevelIndex == -1)
				throw new Exception($"{nameof(currentLevel)} is invalid!");

			var levelProgress = SaveManager.Instance.LevelProgress;
			if (currentLevelIndex == levelProgress && currentLevelIndex < m_LevelStates.Count - 1)
				SaveManager.Instance.LevelProgress = levelProgress + 1;
		}

		void OnLevelSelectionDisplayed()
		{
			ShowUI<LevelSelectionScreen>();
			AudioManager.Instance.PlayMusic(SoundID.None);
		}



		void OnGamePlayStarted(IState current)
		{
			m_CurrentLevel = current;
			ShowUI<HUD>();
			AudioManager.Instance.StopMusic();
		}

		private void OnLevelWasLoaded()
		{
			ShowUI<WinOrLoseScreen>();
		}

		void OnGamePause()
		{
			ShowUI<TemplateUI>();
		}
	}
}
