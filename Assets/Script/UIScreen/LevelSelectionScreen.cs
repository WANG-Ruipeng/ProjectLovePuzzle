using System;
using System.Collections;
using System.Collections.Generic;
using HyperCasual.Core;
using UnityEngine;

namespace Giro
{
	/// <summary>
	/// This View contains level selection screen functionalities
	/// </summary>
	public class LevelSelectionScreen : View
	{
		[SerializeField]
		HyperCasualButton m_BackButton;
		[Space]
		[SerializeField]
		RectTransform m_LevelButtonsRoot;
		[SerializeField]
		RectTransform mangaButtonsRoot;
		[SerializeField]
		AbstractGameEvent m_NextLevelEvent;
		[SerializeField]
		AbstractGameEvent m_BackEvent;
#if UNITY_EDITOR
		[SerializeField]
		bool m_UnlockAllLevels;
#endif

		readonly List<LevelSelectButton> m_Buttons = new();
		readonly List<MangaButton> m_mangaButtons = new();

		void Start()
		{
			var buttons = m_LevelButtonsRoot.GetComponentsInChildren<LevelSelectButton>();
			foreach (LevelSelectButton button in buttons)
			{
				m_Buttons.Add(button);
			}
			var mangaButtons = mangaButtonsRoot.GetComponentsInChildren<MangaButton>();
			foreach (MangaButton button in mangaButtons)
			{
				m_mangaButtons.Add(button);
			}
			ResetButtonData();
		}

		void OnEnable()
		{
			ResetButtonData();

			m_BackButton.AddListener(OnBackButtonClicked);
		}

		void OnDisable()
		{
			m_BackButton.RemoveListener(OnBackButtonClicked);
		}

		void ResetButtonData()
		{
			var levelProgress = SaveManager.LevelProgress;
			for (int i = 0; i < m_Buttons.Count; i++)
			{
				var button = m_Buttons[i];
				var unlocked = i <= levelProgress;
#if UNITY_EDITOR
				unlocked = unlocked || m_UnlockAllLevels;
#endif
				button.SetData(i, unlocked, OnClick);
				m_mangaButtons[i].SetData(i, unlocked);
			}
		}
		void OnMangaButtonClicked(int mangaIndex)
		{

		}
		void OnClick(int startingIndex)
		{
			if (startingIndex < 0)
				throw new Exception("Button is not initialized");

			SequenceManager.Instance.SetStartingLevel(startingIndex);
			m_NextLevelEvent.Raise();
		}

		void OnBackButtonClicked()
		{
			m_BackEvent.Raise();
		}
	}
}
