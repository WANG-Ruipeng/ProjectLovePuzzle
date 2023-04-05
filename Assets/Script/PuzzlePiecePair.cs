using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HyperCasual.Core;
using System;
using Giro;

public class PuzzlePiecePair : Moveable
{
    [Header("Basic information about two pieces")]
    public GameObject leftObj;
    public GameObject rightObj;



    public PuzzlePiece left;
    public PuzzlePiece right;

    public AbstractGameEvent CountdownEvent;

    public override void Init()
    {
        base.Init();

    }

    public override void Reset()
    {
        base.Reset();
        left = leftObj.GetComponentInChildren<PuzzlePiece>();
        right = rightObj.GetComponentInChildren<PuzzlePiece>();
        left.RotateClockWise = true;
        right.RotateClockWise = false;

        transform.position = new Vector3(0, 0, 0);
        leftObj.GetComponentInChildren<PuzzlePiece>().Reset();
        rightObj.GetComponentInChildren<PuzzlePiece>().Reset();
        isPlayingEnterAnim = false;
        isPlayingDownAnim = false;
        isPlayingCombineAnim = false;
        isPlayingExitAnim = false;
        enterAnimationStartTime = 0;
        downAnimationStartTime = 0;
        combineAnimationStartTime = 0;
        exitAnimationStartTime = 0;
    }
    private void Awake()
    {
        combineAnimationLength = combineAnimationCurve.keys[combineAnimationCurve.length - 1].time;
        enterAnimationLength = enterAnimationCurve.keys[enterAnimationCurve.length - 1].time;
        downAnimationLength = downAnimationCurve.keys[downAnimationCurve.length - 1].time;
    }

    public void StartPlayingCombineAnimation()
    {
        isPlayingCombineAnim = true;
        combineAnimationStartTime = Time.time;
        CountdownEvent.Raise();
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
    /// <param name="rightStartPos">The starting position of the right object.</param>
    /// <param name="rightEndPos">The ending position of the right object.</param>
    private void PlayScriptedAnimation(
        ref float animStartTime, ref float animLength, ref bool isPlaying, ref AnimationCurve animCurve,
        ref Vector3 leftStartPos, ref Vector3 leftEndPos, ref Vector3 rightStartPos, ref Vector3 rightEndPos)
    {
        float progress = (Time.time - animStartTime);
        if (progress >= animLength)
        {
            isPlaying = false;
            if ((leftEndPos - this.leftEndPos).magnitude < 0.01)//合并成功触发事件
            {
                StartPlayExitAnimation();
            }
            else if ((leftEndPos - this.leftCombineStartPos).magnitude < 0.01)//到达操作区
            {
                CountdownEvent.Raise();
            }
            else if ((leftEndPos - this.leftExitPos).magnitude < 0.01)
            {
                PuzzlePieceManager.Instance.PlayNextPuzzlePairAnimation();
                return;
            }
        }

        //Debug.Log(progress);
        float posPercent = Mathf.Clamp(animCurve.Evaluate(progress), 0, 1);
        Vector3 leftNowPos = Vector3.Lerp(leftStartPos, leftEndPos, posPercent);
        Vector3 rightNowPos = Vector3.Lerp(rightStartPos, rightEndPos, posPercent);
        leftObj.transform.position = leftNowPos;
        rightObj.transform.position = rightNowPos;
    }

    public override bool Check()
    {
        if (left.IsLocked && right.IsLocked)
        {
            int edgeCnt = PuzzlePiece.edgeCount;
            PuzzlePiece.EdgeProp leftStatus = left.edgeProps[(left.state + 1) % edgeCnt];
            PuzzlePiece.EdgeProp rightStatus = right.edgeProps[(right.state + edgeCnt - 1) % edgeCnt];
            //左的拼图需要检测right edge的状态，右的拼图需要检测left edge 的状态
            if (leftStatus == 0 || rightStatus == 0)
                return false;
            if ((int)leftStatus + (int)rightStatus == 0)
                return true;
            else
                return false;
        }
        return false;
    }

    public override void Collect()
    {
        int edgeCnt = PuzzlePiece.edgeCount;

        if (left.collections.Length > 0)
        {
            Collectible nowLeftColletible = left.collections[(left.state + 1) % edgeCnt];
            if (nowLeftColletible)
                Collect(nowLeftColletible);
        }
        if (right.collections.Length > 0)
        {
            Collectible nowRightCollectible = right.collections[(right.state + edgeCnt - 1) % edgeCnt];
            if (nowRightCollectible)
                Collect(nowRightCollectible);
        }

    }


    /// <summary>
    /// 触发收藏该物品后会发生的事情
    /// TODO: 调用存档系统，播放收藏物品的一系列相关动画，根据物品属性修改角色“好感度”
    /// </summary>
    /// <param name="collectible"></param>
    void Collect(Collectible collectible)
    {
        collectible.Collected();
        if (SaveManager.Instance)//Q:是否需要改为关卡胜利时才保存
        {
            SaveManager.Instance.SaveCollectibleInfo(collectible);
        }
    }

    protected override void OnUpdate()
    {
        if (isPlayingEnterAnim)
        {

            PlayScriptedAnimation(ref enterAnimationStartTime, ref enterAnimationLength, ref isPlayingEnterAnim, ref enterAnimationCurve,
                ref leftEnterStartPos, ref leftDownStartPos, ref rightEnterStartPos, ref rightDownStartPos);

            return;
        }

        if (isPlayingDownAnim)
        {
            PlayScriptedAnimation(ref downAnimationStartTime, ref downAnimationLength, ref isPlayingDownAnim, ref downAnimationCurve,
                ref leftDownStartPos, ref leftCombineStartPos, ref rightDownStartPos, ref rightCombineStartPos);
            return;
        }

        if (isPlayingCombineAnim)
        {
            PlayScriptedAnimation(ref combineAnimationStartTime, ref combineAnimationLength, ref isPlayingCombineAnim, ref combineAnimationCurve,
                ref leftCombineStartPos, ref leftEndPos, ref rightCombineStartPos, ref rightEndPos);
        }
        if (isPlayingExitAnim)
        {
            PlayScriptedAnimation(ref exitAnimationStartTime, ref exitAnimationLength, ref isPlayingExitAnim, ref exitAnimationCurve,
               ref leftEndPos, ref leftExitPos, ref rightEndPos, ref rightExitPos);
        }
    }
}
