using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Giro;
using HyperCasual.Core;
using System;
using TMPro;
using UnityEngine.UI;

public class MangaButton : HyperCasualButton
{
	int m_Index;
	bool m_IsUnlocked;
	RectTransform rectTransform;
	public Sprite sourceImage;

	/// <param name="index">The index of the associated level</param>
	/// <param name="unlocked">Is the associated level locked?</param>
	public void SetData(int index, bool unlocked)
	{
		m_Index = index;
		m_IsUnlocked = unlocked;
		m_Button.interactable = m_IsUnlocked;
	}

	private void Awake()
	{
		rectTransform = GetComponent<RectTransform>();
		var t = GetComponent<Image>().sprite;
		sourceImage = t;
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
		var mangaScreen = UIManager.Instance.GetView<MangaScreen>();
		if (mangaScreen)
		{
			mangaScreen.index = m_Index;
			mangaScreen.startTrans = rectTransform;
			mangaScreen.sourceImage = sourceImage;
			UIManager.Instance.Show<MangaScreen>();

		}
	}

}
