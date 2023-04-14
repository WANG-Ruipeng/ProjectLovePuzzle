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

	void ForceToIdle()
	{
		boyAnimator.Play("Idle");
		boyAnimator.Update(0);
	}

	void ClearState()
	{
		boyAnimator.SetBool("Idle", true);
		boyAnimator.SetBool("WillDrop", false);

		girlAnimator.SetBool("Idle", true);
		girlAnimator.SetBool("WillDrop", false);
	}

	void OnWinAnimPlayRaised()
	{
		if (!boyAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
		{
			ForceToIdle();
			ClearState();
		}
		boyAnimator.SetTrigger("Win");
		girlAnimator.SetTrigger("Win");
	}

	void OnCollectAnimPlayRaised()
	{
		if (!boyAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
		{
			ForceToIdle();
			ClearState();
		}
		boyAnimator.SetTrigger("Collect");
		girlAnimator.SetTrigger("Collect");
	}

	void OnJumpAnimPlayRaised()
	{
		if (!boyAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
		{
			ForceToIdle();
			ClearState();
		}
		boyAnimator.SetTrigger("Jump");
		girlAnimator.SetTrigger("Jump");
	}

	void OnWillDropAnimPlayRaised()
	{
		if (!boyAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
		{
			ForceToIdle();
			ClearState();
		}
		boyAnimator.SetBool("Idle", false);
		boyAnimator.SetBool("Idle", false);
		boyAnimator.SetBool("WillDrop", true);
		girlAnimator.SetBool("WillDrop", true);
	}

	void OnDropAnimPlayRaised()
	{
		if (!boyAnimator.GetCurrentAnimatorStateInfo(0).IsName("WillDrop"))
		{
#if UNITY_EDITOR
			Debug.LogError("还没有进入WillDrop状态，请检查代码");
#endif
			return;
		}
		boyAnimator.SetTrigger("Drop");
		girlAnimator.SetTrigger("Drop");
	}

	void OnIdleAnimPlayRaised()
	{
		ClearState();
	}
}
