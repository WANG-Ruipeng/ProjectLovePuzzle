using System;
using System.Collections.Generic;
using Doozy.Runtime.UIManager.Containers;
using HyperCasual.Core;
using UnityEngine;
using UnityEngine.SceneManagement;
using Doozy.Runtime.Nody;
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
		[SerializeField]
		PagesScreen[] beforeLevelIllustrations;
		[SerializeField]
		FlowNode[] loadSceneNodes;
		[SerializeField]
		FlowController flowController;
		public AbstractLevelData[] Levels => m_Levels;

		public HUD hud;
		public int currentLevel;


		protected override void Awake()
		{
			base.Awake();
			for (int i = 0; i < beforeLevelIllustrations.Length; i++)
			{
				beforeLevelIllustrations[i].id = i;
				beforeLevelIllustrations[i].finalHandle += JumpToNode;
			}
		}
		/// <summary>
		/// Initializes the SequenceManager
		/// </summary>
		public void Initialize()
		{
			InstantiatePreloadedAssets();
		}

		void InstantiatePreloadedAssets()
		{
			foreach (var asset in m_PreloadedAssets)
			{
				Instantiate(asset);
			}
		}

		public void CheckFirstEntryAndLoadLevel(int ind)
		{
			int progress = SaveManager.LevelProgress;
			if (progress == ind)//如果首次进入则show插画
			{
				ShowIllustration(ind);
			}
			else//否则加载关卡
			{
				OnSceneLoad(ind);
			}
		}

		private void ShowIllustration(int ind)
		{
			beforeLevelIllustrations[ind].ShowNextPage();
		}
		private void JumpToNode(int ind)
		{
			flowController.SetActiveNode(loadSceneNodes[ind]);
		}
		public void OnSceneLoad(int ind)
		{
			Debug.Log("Load Level: " + m_Levels[ind].name);
			if (m_Levels[ind] == null)
				throw new Exception("level " + ind + " is null!");
			else if (m_Levels[ind] is SceneRef)
				return;
			else
			{
				// Load managers specific to the level
				foreach (var prefab in m_LevelManagers)
				{
					//GameObject.Instantiate(prefab);
					GameObject go = (GameObject)Resources.Load("LoadLevelFromDef/" + prefab.name);
					GameObject.Instantiate(Resources.Load("LoadLevelFromDef/" + prefab.name));
					Debug.Log(prefab.name + " loaded!");
				}

				GameManager.Instance.hud = hud;
				GameManager.Instance.LoadLevel(m_Levels[ind] as LevelDefinition);
				GameObject subCameraGo = GameObject.Find("Camera");
				if (subCameraGo)
				{
					Destroy(subCameraGo);
				}
				currentLevel = ind;
			}
		}

	}
}
