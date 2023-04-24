#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Giro;

public class ArchiveSettingEditor : EditorWindow
{
	int levelProgress;
	[MenuItem("Window/Save Setting Editor")]
	static void Init()
	{
		ArchiveSettingEditor window = (ArchiveSettingEditor)EditorWindow.GetWindow(typeof(ArchiveSettingEditor), false, "Save Setting Editor");
		window.Show();
	}


	private void OnGUI()
	{
		levelProgress = EditorGUILayout.IntField("Level Progress", levelProgress);
		if (GUILayout.Button("应用上述存档"))
		{
			SaveManager.LevelProgress = levelProgress;
		}
		GUILayout.Label("↓这真的会重置一切存档包括收藏品");
		if (GUILayout.Button("重建新存档"))
		{
			Archive.Recreate();
		}
	}

}

#endif