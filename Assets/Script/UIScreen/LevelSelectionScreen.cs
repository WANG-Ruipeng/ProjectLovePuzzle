using System;
using System.Collections;
using System.Collections.Generic;
using HyperCasual.Core;
using UnityEngine;
using Doozy.Runtime.UIManager.Components;
namespace Giro
{
	/// <summary>
	/// This View contains level selection screen functionalities
	/// </summary>
	public class LevelSelectionScreen : MonoBehaviour
	{
		public UIButton[] levelButtons;

		public void SetButtonActivated()
		{
			int progress = SaveManager.LevelProgress;
			if (progress < 4)
				for (int i = 0; i <= progress; i++)
				{
					levelButtons[i].interactable = true;
				}
			for (int i = progress + 1; i < 4; i++)
			{
				levelButtons[i].interactable = false;
			}
			
		}
	}
}
