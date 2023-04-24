using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Giro;
using HyperCasual.Core;
using System;

public class MangaButton : HyperCasualButton
{
	int m_Index;
	bool m_IsUnlocked;


	/// <param name="index">The index of the associated level</param>
	/// <param name="unlocked">Is the associated level locked?</param>
	public void SetData(int index, bool unlocked)
	{
		m_Index = index;
		m_IsUnlocked = unlocked;
		m_Button.interactable = m_IsUnlocked;
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		AddListener(OnClick);
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		RemoveListener(OnClick);
	}

	protected override void OnClick()
	{
		PlayButtonSound();
		UIManager.Instance.GetView<MangaScreen>().index = m_Index;
		UIManager.Instance.Show<MangaScreen>();
	}

}
