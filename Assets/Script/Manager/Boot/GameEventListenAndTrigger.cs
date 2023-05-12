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

	SequenceManager gameLoader;
	private void Awake()
	{
		winEventListener = new GenericGameEventListener();
		loseEventListener = new GenericGameEventListener();

		winEvent.AddListener(winEventListener);
		loseEvent.AddListener(loseEventListener);

		winEventListener.EventHandler += OnWinEventRaised;
		loseEventListener.EventHandler += OnLoseEventRaised;

		gameLoader = GetComponent<SequenceManager>();
	}

	void OnWinEventRaised()
	{
		//TODO:等待胜利动画播放完毕
		int nextLevel = gameLoader.currentLevel + 1;
		if (nextLevel > SaveManager.LevelProgress)
		{
			SaveManager.LevelProgress = nextLevel;
		}
		flowController.SetActiveNode(enterWinScreenNode);
		Time.timeScale = 0;
	}

	void OnLoseEventRaised()
	{
		AudioManager.Instance.PlayEffect(SoundID.lose_and_fall);
		//TODO:等待失败动画播放完毕
		flowController.SetActiveNode(enterLoseScreenNode);
	}

}
