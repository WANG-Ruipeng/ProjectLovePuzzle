using Doozy.Runtime.UIManager.Components;
using Giro;
using HyperCasual.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingScreen : MonoBehaviour
{
	public UIToggle music;
	public UIToggle effect;
	public UISlider volume;
	private void Awake()
	{
		var setting = SaveManager.LoadAudioSettings();
		music.SetIsOn(setting.EnableMusic, false);
		effect.SetIsOn(setting.EnableSfx, false);
		volume.SetValueWithoutNotify(setting.MasterVolume);

		music.OnValueChangedCallback.AddListener(SetMusic);
		effect.OnValueChangedCallback.AddListener(SetEffect);
		volume.OnValueChangedCallback.AddListener(SetVolume);
	}
	void SetMusic(bool m)
	{
		var setting = SaveManager.LoadAudioSettings();
		setting.EnableMusic = m;

		//enable setting to game
		AudioManager.Instance.EnableMusic = m;
		if (m)
		{
			AudioManager.Instance.ContinuePlay();
		}
		else
		{
			AudioManager.Instance.StopMusic();
		}
		//save the setting
		SaveManager.SaveAudioSettings(setting);
	}
	void SetEffect(bool e)
	{
		var setting = SaveManager.LoadAudioSettings();
		setting.EnableSfx = e;
		AudioManager.Instance.EnableSfx = e;
		SaveManager.SaveAudioSettings(setting);
	}
	void SetVolume(float v)
	{
		var setting = SaveManager.LoadAudioSettings();
		setting.MasterVolume = v;
		AudioManager.Instance.MasterVolume = v;
		SaveManager.SaveAudioSettings(setting);
	}
}
