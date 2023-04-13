using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HyperCasual.Core;


public class PlayerManager : MonoBehaviour
{
	public Player boy;
	public Player girl;

	Animator boyAnimator;
	Animator girlAnimator;

	GenericGameEventListener winAnimPlayListener;           //好多动画的EventListener
	GenericGameEventListener collectAnimPlayListener;
	GenericGameEventListener jumpAnimPlayListener;
	GenericGameEventListener willDropAnimPlayListener;
	GenericGameEventListener dropAnimPlayListener;
	GenericGameEventListener idleAnimPlayListener;

	public AbstractGameEvent winAnimPlay;
	public AbstractGameEvent collectAnimPlay;
	public AbstractGameEvent jumpAnimPlay;
	public AbstractGameEvent willDropAnimPlay;
	public AbstractGameEvent dropAnimPlay;
	public AbstractGameEvent idleAnimPlay;

	private void Awake()
	{
		boyAnimator = boy.GetComponent<Animator>();
		girlAnimator = girl.GetComponent<Animator>();

		winAnimPlayListener = new GenericGameEventListener();
		collectAnimPlayListener = new GenericGameEventListener();
		jumpAnimPlayListener = new GenericGameEventListener();
		willDropAnimPlayListener = new GenericGameEventListener();
		dropAnimPlayListener = new GenericGameEventListener();
		idleAnimPlayListener = new GenericGameEventListener();

		winAnimPlay.AddListener(winAnimPlayListener);
		collectAnimPlay.AddListener(collectAnimPlayListener);
		jumpAnimPlay.AddListener(jumpAnimPlayListener);
		willDropAnimPlay.AddListener(willDropAnimPlayListener);
		dropAnimPlay.AddListener(dropAnimPlayListener);
		idleAnimPlay.AddListener(idleAnimPlayListener);

		winAnimPlayListener.EventHandler += OnWinAnimPlayRaised;
		collectAnimPlayListener.EventHandler += OnCollectAnimPlayRaised;
		jumpAnimPlayListener.EventHandler += OnJumpAnimPlayRaised;
		willDropAnimPlayListener.EventHandler += OnWillDropAnimPlayRaised;
		dropAnimPlayListener.EventHandler += OnDropAnimPlayRaised;
		idleAnimPlayListener.EventHandler += OnIdleAnimPlayRaised;
	}
	void ClearState()
	{

	}

	void OnWinAnimPlayRaised()
	{

	}

	void OnCollectAnimPlayRaised()
	{

	}

	void OnJumpAnimPlayRaised()
	{

	}

	void OnWillDropAnimPlayRaised()
	{

	}

	void OnDropAnimPlayRaised()
	{

	}

	void OnIdleAnimPlayRaised()
	{

	}
}
