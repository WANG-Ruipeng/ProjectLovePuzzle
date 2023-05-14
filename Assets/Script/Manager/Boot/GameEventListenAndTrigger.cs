using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HyperCasual.Core;
using Giro;
using Doozy.Runtime.Nody;
public class GameEventListenAndTrigger : MonoBehaviour
{
	public AbstractGameEvent winEvent;
	public AbstractGameEvent loseEvent;

	public FlowController flowController;
	public FlowNode enterWinScreenNode;
	public FlowNode enterLoseScreenNode;

	GenericGameEventListener winEventListener;
	GenericGameEventListener loseEventListener;

	public PagesScreen[] afterWinIllustration;

	SequenceManager gameLoader;
	int CurrentLevel => gameLoader.CurrentLevel;
	private void Awake()
	{
		winEventListener = new GenericGameEventListener();
		loseEventListener = new GenericGameEventListener();

		winEvent.AddListener(winEventListener);
		loseEvent.AddListener(loseEventListener);

		winEventListener.EventHandler += OnWinEventRaised;
		loseEventListener.EventHandler += OnLoseEventRaised;

		gameLoader = GetComponent<SequenceManager>();

		for (int i = 0; i < afterWinIllustration.Length; i++)
		{
			if (afterWinIllustration[i])
				afterWinIllustration[i].finalHandle += () => flowController.SetActiveNode(enterWinScreenNode);
		}
	}

	void OnWinEventRaised()
	{
		//在胜利动画播放完毕调用，实现于Player的胜利播放脚本
		if (CurrentLevel == SaveManager.LevelProgress)
		{
			SaveManager.LevelProgress = CurrentLevel + 1;
		}
		Time.timeScale = 0;
		if (CurrentLevel - 1 >= afterWinIllustration.Length || !afterWinIllustration[CurrentLevel])
		{
			flowController.SetActiveNode(enterWinScreenNode);
			return;
		}
		if (afterWinIllustration[CurrentLevel])
		{
			afterWinIllustration[CurrentLevel].ShowNextPage();
		}
	}

	void OnLoseEventRaised()
	{
		//TODO:等待失败动画播放完毕
		flowController.SetActiveNode(enterLoseScreenNode);
	}

}
