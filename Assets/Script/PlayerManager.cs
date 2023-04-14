using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HyperCasual.Core;


public class PlayerManager : MonoBehaviour
{

	static PlayerManager s_Instance;
	public static PlayerManager Instance => s_Instance;

	public Player boy;
	public Player girl;

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
		if (s_Instance != null && s_Instance != this)
		{
			Destroy(gameObject);
			return;
		}

		s_Instance = this;

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

		winAnimPlayListener.EventHandler += boy.OnWinAnimPlayRaised;
		collectAnimPlayListener.EventHandler += boy.OnCollectAnimPlayRaised;
		jumpAnimPlayListener.EventHandler += boy.OnJumpAnimPlayRaised;
		willDropAnimPlayListener.EventHandler += boy.OnWillDropAnimPlayRaised;
		dropAnimPlayListener.EventHandler += boy.OnDropAnimPlayRaised;
		idleAnimPlayListener.EventHandler += boy.OnIdleAnimPlayRaised;

		winAnimPlayListener.EventHandler += girl.OnWinAnimPlayRaised;
		collectAnimPlayListener.EventHandler += girl.OnCollectAnimPlayRaised;
		jumpAnimPlayListener.EventHandler += girl.OnJumpAnimPlayRaised;
		willDropAnimPlayListener.EventHandler += girl.OnWillDropAnimPlayRaised;
		dropAnimPlayListener.EventHandler += girl.OnDropAnimPlayRaised;
		idleAnimPlayListener.EventHandler += girl.OnIdleAnimPlayRaised;
	}


}
