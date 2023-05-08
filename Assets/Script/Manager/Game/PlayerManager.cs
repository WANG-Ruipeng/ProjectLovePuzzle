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
    public void SetPlayerFall()
    {
        boy.PlayFallAnimation();
        girl.PlayFallAnimation();
    }
    public void SetPlayerJump()
    {
        boy.PlayJumpAnimation();
        girl.PlayJumpAnimation();
    }
    public void SetPlayerVictory()
    {
        boy.PlayVictoryAnimation();
        girl.PlayVictoryAnimation();
    }
}
