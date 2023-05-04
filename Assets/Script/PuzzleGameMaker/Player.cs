using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	public string playerName;

	Animator animator;

	private void Awake()
	{
		animator = GetComponent<Animator>();
	}
	public void ForceToIdle()
	{
		animator.Play("Idle");
		animator.Update(0);
	}

	public void ClearState()
	{
		animator.SetBool("Idle", true);
		animator.SetBool("WillDrop", false);
	}

	public void OnWinAnimPlayRaised()
	{
		if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
		{
			ForceToIdle();
			ClearState();
		}
		animator.SetTrigger("Win");
	}

	public void OnCollectAnimPlayRaised()
	{
		if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
		{
			ForceToIdle();
			ClearState();
		}
		animator.SetTrigger("Collect");
	}

	public void OnJumpAnimPlayRaised()
	{
		if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
		{
			ForceToIdle();
			ClearState();
		}
		animator.SetTrigger("Jump");
	}

	public void OnWillDropAnimPlayRaised()
	{
		if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
		{
			ForceToIdle();
			ClearState();
		}
		animator.SetBool("Idle", false);
		animator.SetBool("Idle", false);
	}

	public void OnDropAnimPlayRaised()
	{
		if (!animator.GetCurrentAnimatorStateInfo(0).IsName("WillDrop"))
		{
#if UNITY_EDITOR
			Debug.LogError("还没有进入WillDrop状态，请检查代码");
#endif
			return;
		}
		animator.SetTrigger("Drop");
	}

	public void OnIdleAnimPlayRaised()
	{
		ClearState();
	}
}
