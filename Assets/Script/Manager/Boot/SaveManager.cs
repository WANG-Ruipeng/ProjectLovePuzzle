using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HyperCasual.Core;
using AudioSettings = HyperCasual.Core.AudioSettings;
namespace Giro
{
	/// <summary>
	/// 音量之类的全局设定使用原有的轮子保存
	/// 关卡进度、收藏品等游戏核心存档使用xml保存（Archive类）
	/// </summary>
	public class SaveManager : MonoBehaviour
	{

		public static SaveManager Instance => s_Instance;
		static SaveManager s_Instance;

		const string k_AudioSettings = "AudioSettings";
		void Awake()
		{
			s_Instance = this;
			Archive.Load();
		}
		public static int LevelProgress
		{
			get => Archive.LevelProgress;
			set => Archive.WriteLevelProgress(value);
		}

		/// <summary>
		/// 保存收藏品数据调用SaveCollectibleInfo
		/// </summary>
		public static CollectibleSaveInfo[] CollectiblesInfo => Archive.CollectiblesInfo;

		public CollectibleSaveInfo LoadCollectibleInfo(int id)
		{
			if (Archive.hasLoad)
				return Archive.CollectiblesInfo[id];
			else
			{
				Debug.LogError("Didn't load the Archive");
				return null;
			}
		}

		public void SaveCollectibleInfo(Collectible collectible)
		{
			Archive.WriteCollectibleInfo(collectible.id, collectible.unlocked);
		}

		public AudioSettings LoadAudioSettings()
		{
			return PlayerPrefsUtils.Read<AudioSettings>(k_AudioSettings);
		}
		public void SaveAudioSettings(AudioSettings audioSettings)
		{
			PlayerPrefsUtils.Write(k_AudioSettings, audioSettings);
		}
	}
}