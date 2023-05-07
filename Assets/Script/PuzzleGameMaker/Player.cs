using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    public StateMachine stateMachine;
    private SkeletonAnimation skeletonAnimation;
    public string playerName;

	private void Awake()
	{
		skeletonAnimation = GetComponent<SkeletonAnimation>();
		stateMachine = GetComponent<StateMachine>();
	}

	public void PlayIdleAnimation()
	{
		
	}
	public void ForceToIdle()
	{
        skeletonAnimation.state.SetAnimation(0, "idel", true);
		Debug.Log("Yes");
    }

	public void ClearState()
	{

	}

	public void OnWinAnimPlayRaised()
	{

	}

	public void OnCollectAnimPlayRaised()
	{

	}

	public void OnJumpAnimPlayRaised()
	{

	}

	public void OnWillDropAnimPlayRaised()
	{

	}

	public void OnDropAnimPlayRaised()
	{

	}

	public void OnIdleAnimPlayRaised()
	{

	}
}
