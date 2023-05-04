using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Platform : Movable
{

	public GameObject leftInteractObj;
	public GameObject rightInteractObj;
	public GameObject background;
	Animator animator;
	public override void Reset()
	{
		base.Reset();
		animator = GetComponent<Animator>();
		animator.SetBool("StartPlayInteractAnim", false);
		animator.Play("PieceDef");
		screenPos = ScreenPos.unentered;
		gameObject.transform.position = new Vector3(100, 100, 0);
		isPlayingEnterAnim = false;
		isPlayingDownAnim = false;
		isPlayingCombineAnim = false;
		isPlayingExitAnim = false;
		enterAnimationStartTime = 0;
		downAnimationStartTime = 0;
		combineAnimationStartTime = 0;
		exitAnimationStartTime = 0;
	}


	public void SetParameter(Vector3 EnterStartPos, Vector3 DownStartPos,
		 Vector3 EndPos, Vector3 ExitPos,
		AnimationCurve enterCurve, AnimationCurve downCurve, AnimationCurve exitCurve)
	{
		this.leftEnterStartPos = EnterStartPos;

		this.leftDownStartPos = DownStartPos;

		this.leftEndPos = EndPos;

		this.leftExitPos = ExitPos;

		enterAnimationCurve = enterCurve;
		downAnimationCurve = downCurve;
		exitAnimationCurve = exitCurve;
	}


	/// <summary>
	/// Plays a scripted animation by updating the positions of two objects over time, based on an animation curve.
	/// </summary>
	/// <param name="animStartTime">The start time of the animation.</param>
	/// <param name="animLength">The length of the animation.</param>
	/// <param name="isPlaying">A bool indicating whether the animation is currently playing.</param>
	/// <param name="animCurve">The animation curve used to calculate the position of the objects over time.</param>
	/// <param name="leftStartPos">The starting position of the left object.</param>
	/// <param name="leftEndPos">The ending position of the left object.</param>
	private void PlayScriptedAnimation(
		ref float animStartTime, ref float animLength, ref bool isPlaying, ref AnimationCurve animCurve,
		ref Vector3 leftStartPos, ref Vector3 leftEndPos)
	{
		float progress = (Time.time - animStartTime);
		if (progress >= animLength)
		{
			isPlaying = false;
			if ((leftEndPos - this.leftExitPos).magnitude < 0.01)//退出播放完
			{
				MovableManager.Instance.PlayNextPuzzlePairAnimation();
			}
		}

		//Debug.Log(progress);
		float posPercent = Mathf.Clamp(animCurve.Evaluate(progress), 0, 1);
		Vector3 leftNowPos = Vector3.Lerp(leftStartPos, leftEndPos, posPercent);
		transform.position = leftNowPos;
	}


	public void StartPlayingInteractAnimation()
	{
		///TODO:Play cutscene!!!!!!!动画顺序：
		///1.玩家向上跳跃进入画面
		///2.获得收集品的动画
		animator.SetBool("StartPlayInteractAnim", true);
		//Debug.Log("Platform down.");
		///3.播放收集品获得的显示
		///4.拼图下落到操作区（已经在animator里调用了call back）

	}

	public override void PlayNextAnimation()
	{
		switch (screenPos)
		{
			case ScreenPos.unentered:
				StartPlayingEnterAnimation();
				Debug.Log("Platform enter.");
				screenPos = ScreenPos.upScreen;
				break;
			case ScreenPos.upScreen:
				StartPlayingInteractAnimation();
				Debug.Log("Platform interact.");
				screenPos = ScreenPos.downScreen;
				break;
			default:

				break;
		}
	}

	void OnInteractAnimEnd()
	{
		StartPlayingExitAnimation();
	}

	protected override void OnUpdate()
	{

		if (isPlayingEnterAnim)
		{
			PlayScriptedAnimation(ref enterAnimationStartTime, ref enterAnimationLength, ref isPlayingEnterAnim, ref enterAnimationCurve,
				ref leftEnterStartPos, ref leftDownStartPos);

			return;
		}

		if (isPlayingExitAnim)
		{
			PlayScriptedAnimation(ref exitAnimationStartTime, ref exitAnimationLength, ref isPlayingExitAnim, ref exitAnimationCurve,
			   ref leftDownStartPos, ref leftExitPos);
		}
	}


}
