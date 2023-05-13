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
			afterWinIllustration[i].finalHandle += () => flowController.SetActiveNode(enterWinScreenNode);
		}
	}

	void OnWinEventRaised()
	{
		//TODO:等待胜利动画播放完毕
		if (CurrentLevel > SaveManager.LevelProgress)
		{
			SaveManager.LevelProgress = CurrentLevel;
		}
		Time.timeScale = 0;
		if (CurrentLevel - 1 >= afterWinIllustration.Length)
		{
			flowController.SetActiveNode(enterWinScreenNode);
			return;
		}
		if (afterWinIllustration[CurrentLevel - 1])
		{
			afterWinIllustration[CurrentLevel - 1].ShowNextPage();
		}
	}

	void OnLoseEventRaised()
	{
		AudioManager.Instance.PlayEffect(SoundID.lose_and_fall);
		//TODO:等待失败动画播放完毕
		flowController.SetActiveNode(enterLoseScreenNode);
	}

}
