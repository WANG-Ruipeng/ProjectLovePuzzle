#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Giro;

public class ArchiveSettingEditor : EditorWindow
{
	int levelProgress;
	bool enableMusic;
	bool enableSfx;
	float masterVolume;
	[MenuItem("Window/Save Setting Editor")]
	static void Init()
	{
		ArchiveSettingEditor window = (ArchiveSettingEditor)EditorWindow.GetWindow(typeof(ArchiveSettingEditor), false, "Save Setting Editor");
		window.Show();
	}


	private void OnGUI()
	{
		levelProgress = EditorGUILayout.IntField("Level Progress", levelProgress);
		enableMusic = EditorGUILayout.Toggle("BGM", enableMusic);
		enableSfx = EditorGUILayout.Toggle("effect sound", enableSfx);
		masterVolume = EditorGUILayout.FloatField("masterVolume", masterVolume);
		if (GUILayout.Button("应用上述存档"))
		{
			SaveManager.LevelProgress = levelProgress;
			HyperCasual.Core.AudioSettings audioSettings = new HyperCasual.Core.AudioSettings();
			audioSettings.EnableMusic = enableMusic;
			audioSettings.EnableSfx = enableSfx;
			audioSettings.MasterVolume = masterVolume;
			SaveManager.SaveAudioSettings(audioSettings);
		}
		GUILayout.Label("↓这真的会重置一切存档包括收藏品");
		if (GUILayout.Button("重建新存档"))
		{
			Archive.Recreate();
			Archive.Load();
		}
	}

}

#endif