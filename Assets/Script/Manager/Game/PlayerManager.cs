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
		boy.PlayIdleAnimation();
		girl.PlayIdleAnimation();
	}

	public void SetPlayerAlmostFall()
	{
		boy.PlayAlmostFallAnimation();
		girl.PlayAlmostFallAnimation();
	}
	public void SetPlayerFall(System.Action action = null)
	{
		boy.PlayFallAnimation(action);
		girl.PlayFallAnimation();
	}
	public void SetPlayerJump()
	{
		boy.PlayJumpAnimation();
		girl.PlayJumpAnimation();
	}
	public void SetPlayerVictory(System.Action action = null)
	{
		boy.PlayVictoryAnimation(action);
		girl.PlayVictoryAnimation();
	}
}
