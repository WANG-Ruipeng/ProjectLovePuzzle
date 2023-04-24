using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HyperCasual.Core;
using Giro;
using System;

public class MangaScreen : View
{
	public int index;
	[SerializeField]
	HyperCasualButton m_BackButton;

	[SerializeField]
	AbstractGameEvent m_BackEvent;

	[Serializable]
	public class Page//此处要怎么保存点开漫画页中出现的物体还需确认，包括位置等等之类的
	{
		public GameObject manga;
		public GameObject[] collectibles;
	}

	public Page[] pages;

	void ShowManga()//放漫画
	{
		//TODO:根据位置和观看所需操作来生成漫画
	}

	void ShowCollectible()//读存档，放收藏品
	{
		//TODO:根据位置和观看所需操作来生成收藏品
	}

	void OnEnable()
	{
		m_BackButton.AddListener(OnBackButtonClicked);
	}

	void OnDisable()
	{
		m_BackButton.RemoveListener(OnBackButtonClicked);
	}
	void OnBackButtonClicked()
	{
		m_BackEvent.Raise();
	}
}
