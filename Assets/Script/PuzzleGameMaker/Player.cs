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
    public void PlayFallAnimation()
    {
        skeletonAnimation.state.SetAnimation(0, "fall", true);
    }
    public void PlayJumpAnimation()
	{
        skeletonAnimation.state.SetAnimation(0, "jump", true);
    }
    public void PlayVictoryAnimation()
    {
        skeletonAnimation.state.SetAnimation(0, "victory", true);
    }
}
