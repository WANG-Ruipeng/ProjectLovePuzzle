using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Platform : Moveable
{

	public override void Reset()
	{
		base.Reset();

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
		}

		//Debug.Log(progress);
		float posPercent = Mathf.Clamp(animCurve.Evaluate(progress), 0, 1);
		Vector3 leftNowPos = Vector3.Lerp(leftStartPos, leftEndPos, posPercent);
		transform.position = leftNowPos;
	}

    public override void StartPlayingDownAnimation()
    {
		//TODO:Play cutscene!!!!!!!

        base.StartPlayingDownAnimation();
		
    }
    protected override void OnUpdate()
	{

		if (isPlayingEnterAnim)
		{
			PlayScriptedAnimation(ref enterAnimationStartTime, ref enterAnimationLength, ref isPlayingEnterAnim, ref enterAnimationCurve,
				ref leftEnterStartPos, ref leftDownStartPos);

			return;
		}

		if (isPlayingDownAnim)
		{
			
			PlayScriptedAnimation(ref downAnimationStartTime, ref downAnimationLength, ref isPlayingDownAnim, ref downAnimationCurve,
				ref leftDownStartPos, ref leftCombineStartPos);
			return;
		}

        if (isPlayingExitAnim)
        {
            PlayScriptedAnimation(ref exitAnimationStartTime, ref exitAnimationLength, ref isPlayingExitAnim, ref exitAnimationCurve,
               ref leftEndPos, ref rightEndPos);
        }
    }


}
