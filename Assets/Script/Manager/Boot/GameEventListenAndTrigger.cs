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

	private void Awake()
	{
		winEventListener = new GenericGameEventListener();
		loseEventListener = new GenericGameEventListener();

		winEvent.AddListener(winEventListener);
		loseEvent.AddListener(loseEventListener);

		winEventListener.EventHandler += OnWinEventRaised;
		loseEventListener.EventHandler += OnLoseEventRaised;


	}

	void OnWinEventRaised()
	{
		//TODO:等待胜利动画播放完毕
		flowController.SetActiveNode(enterWinScreenNode);
	}

	void OnLoseEventRaised()
	{
		//TODO:等待失败动画播放完毕
		flowController.SetActiveNode(enterLoseScreenNode);
	}

}
