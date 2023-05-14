using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
	private SkeletonAnimation skeletonAnimation;
	public string playerName;

	private void Awake()
	{
		skeletonAnimation = GetComponent<SkeletonAnimation>();
	}

	public void PlayIdleAnimation(float playSpeed)
	{
		var trackEntity = skeletonAnimation.state.SetAnimation(0, "IDEL", true);
		trackEntity.TimeScale = playSpeed;
	}
	public void PlayAlmostFallAnimation(float playSpeed)
	{
        var trackEntity = skeletonAnimation.state.SetAnimation(0, "ALMOST FALL", true);
        trackEntity.TimeScale = playSpeed;
    }
	public void PlayFallAnimation(float playSpeed,System.Action action = null)
	{
        var trackEntity = skeletonAnimation.state.SetAnimation(0, "FALL", false);
        trackEntity.TimeScale = playSpeed;
        if (action == null)
			return;
		Spine.AnimationState.TrackEntryDelegate cc = delegate
		{
			action();
		};
		cc += delegate { skeletonAnimation.AnimationState.Complete -= cc; };

		skeletonAnimation.AnimationState.Complete += cc;
	}
	public void PlayJumpAnimation(float playSpeed)
	{
        var trackEntity = skeletonAnimation.state.SetAnimation(0, "JUMP", false);
        trackEntity.TimeScale = playSpeed;
    }
	public void PlayVictoryAnimation(float playSpeed,System.Action action = null)
	{
        var trackEntity = skeletonAnimation.state.SetAnimation(0, "VICTORY", false);
        trackEntity.TimeScale = playSpeed;
        if (action == null)
			return;
		Spine.AnimationState.TrackEntryDelegate cc = delegate
		{
			action();
		};
		cc += delegate { skeletonAnimation.AnimationState.Complete -= cc; };

		skeletonAnimation.AnimationState.Complete += cc;

	}
}
