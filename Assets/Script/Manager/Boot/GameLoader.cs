using System;
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
	public class GameLoader : AbstractSingleton<GameLoader>
	{
		[SerializeField]
		GameObject[] m_PreloadedAssets;
		[SerializeField]
		AbstractLevelData[] m_Levels;
		[SerializeField]
		GameObject[] m_LevelManagers;
		public AbstractLevelData[] Levels => m_Levels;

		public HUD hud;

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

			}
		}

	}
}
