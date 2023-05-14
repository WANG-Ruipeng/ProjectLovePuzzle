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

	public void PlayIdleAnimation()
	{
		skeletonAnimation.state.SetAnimation(0, "idel", true);
	}
	public void PlayAlmostFallAnimation()
	{
		skeletonAnimation.state.SetAnimation(0, "aimost fall", true);
	}
	public void PlayFallAnimation(System.Action action = null)
	{
		skeletonAnimation.state.SetAnimation(0, "fall", false);
		if (action == null)
			return;
		Spine.AnimationState.TrackEntryDelegate cc = delegate
		{
			action();
		};
		cc += delegate { skeletonAnimation.AnimationState.Complete -= cc; };

		skeletonAnimation.AnimationState.Complete += cc;
	}
	public void PlayJumpAnimation()
	{
		skeletonAnimation.state.SetAnimation(0, "jump", false);
	}
	public void PlayVictoryAnimation(System.Action action = null)
	{
		skeletonAnimation.state.SetAnimation(0, "victory", false);
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
