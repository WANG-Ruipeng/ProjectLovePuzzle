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
	public bool firtAlmostDown;
	public float almostFallTime;

	public float idleAnimationPlaySpeed = 1;
    public float almostFallAnimationPlaySpeed = 1;
    public float fallAnimationPlaySpeed = 1;
	public float jumpAnimationPlaySpeed = 1;
    public float victoryAnimationPlaySpeed = 1;

    private void Awake()
	{
		if (s_Instance != null && s_Instance != this)
		{
			Destroy(gameObject);
			return;
		}

		s_Instance = this;

	}

	public void SetPlayerIdle()
	{
		boy.PlayIdleAnimation(idleAnimationPlaySpeed);
		girl.PlayIdleAnimation(idleAnimationPlaySpeed);
	}

	public void SetPlayerAlmostFall()
	{
		boy.PlayAlmostFallAnimation(almostFallAnimationPlaySpeed);
		girl.PlayAlmostFallAnimation(almostFallAnimationPlaySpeed);
	}
	public void SetPlayerFall(System.Action action = null)
	{
		boy.PlayFallAnimation(fallAnimationPlaySpeed,action);
		girl.PlayFallAnimation(fallAnimationPlaySpeed);
	}
	public void SetPlayerJump()
	{
		boy.PlayJumpAnimation(jumpAnimationPlaySpeed);
		girl.PlayJumpAnimation(jumpAnimationPlaySpeed);
	}
	public void SetPlayerVictory(System.Action action = null)
	{
		boy.PlayVictoryAnimation(victoryAnimationPlaySpeed,action);
		girl.PlayVictoryAnimation(victoryAnimationPlaySpeed);
	}
}
