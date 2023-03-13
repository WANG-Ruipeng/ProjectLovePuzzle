using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HyperCasual.Core;
using AudioSettings = HyperCasual.Core.AudioSettings;
namespace Giro
{
    public class SaveManager : MonoBehaviour
    {
        public static SaveManager Instance => s_Instance;
        static SaveManager s_Instance;

        const string k_AudioSettings = "AudioSettings";
        const string k_LevelProgress = "LevelProgress";
        void Awake()
        {
            s_Instance = this;
        }
        public int LevelProgress
        {
            get => PlayerPrefs.GetInt(k_LevelProgress);
            set => PlayerPrefs.SetInt(k_LevelProgress, value);
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