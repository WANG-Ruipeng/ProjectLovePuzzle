using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HyperCasual.Core;
using System;

public class PuzzlePiecePair : MonoBehaviour
{
    [Header("Basic information about two pieces")]
    public int pieceNo;
    public GameObject leftObj;
    public GameObject rightObj;

    public Vector3 leftEnterStartPos = new Vector3(-1.56f, 10, 0);
    public Vector3 rightEnterStartPos = new Vector3(1.56f, 10, 0);
    public AnimationCurve enterAnimationCurve;

    public Vector3 leftDownStartPos = new Vector3(-1.56f, 4, 0);
    public Vector3 rightDownStartPos = new Vector3(1.56f, 4, 0);
    public AnimationCurve downAnimationCurve;

    public Vector3 leftCombineStartPos = new Vector3(-1.56f, 0, 0);
    public Vector3 rightCombineStartPos = new Vector3(1.56f, 0, 0);
    public Vector3 leftEndPos = new Vector3(-0.56f, 0, 0);
    public Vector3 rightEndPos = new Vector3(0.56f, 0, 0);
    public AnimationCurve combineAnimationCurve;

    float enterAnimationLength = 0;
    float downAnimationLength = 0;
    float combineAnimationLength = 0;

    public PuzzlePiece left;
    public PuzzlePiece right;

    public bool isPlayingEnterAnim = false;
    public float enterAnimationStartTime = 0;

    public bool isPlayingDownAnim = false;
    public float downAnimationStartTime = 0;

    public bool isPlayingCombineAnim = false;
    public float combineAnimationStartTime = 0;

    public AbstractGameEvent CountdownEvent;

    Animator animator;

    bool doNotUpdate = false;

    public void Reset()
    {
        leftObj.GetComponentInChildren<PuzzlePiece>().Reset();
        rightObj.GetComponentInChildren<PuzzlePiece>().Reset();
        isPlayingEnterAnim = false;
        isPlayingDownAnim = false;
        isPlayingCombineAnim = false;
        enterAnimationStartTime = 0;
        downAnimationStartTime = 0;
        combineAnimationStartTime = 0;
    }
    private void Awake()
    {
        left = leftObj.GetComponentInChildren<PuzzlePiece>();
        right = rightObj.GetComponentInChildren<PuzzlePiece>();
        left.RotateClockWise = true;
        right.RotateClockWise = false;

        animator = GetComponent<Animator>();
        combineAnimationLength = combineAnimationCurve.keys[combineAnimationCurve.length - 1].time;
        enterAnimationLength = enterAnimationCurve.keys[enterAnimationCurve.length - 1].time;
        downAnimationLength = downAnimationCurve.keys[downAnimationCurve.length - 1].time;
    }

    /// <summary>
    /// 设置所有动画参数，用于初始化此类的属性。
    /// </summary>
    /// <param name="leftEnterStartPos">左边入场起始位置</param>
    /// <param name="rightEnterStartPos">右边入场起始位置</param>
    /// <param name="enterAnimationCurve">入场动画曲线</param>
    /// <param name="leftDownStartPos">左边下落起始位置</param>
    /// <param name="rightDownStartPos">右边下落起始位置</param>
    /// <param name="downAnimationCurve">下落动画曲线</param>
    /// <param name="leftCombineStartPos">左边合并起始位置</param>
    /// <param name="rightCombineStartPos">右边合并起始位置</param>
    /// <param name="leftEndPos">左边结束位置</param>
    /// <param name="rightEndPos">右边结束位置</param>
    /// <param name="combineAnimationCurve">合并动画曲线</param>
    /// </summary>
    public void SetAllAnimationParamters(
        Vector3 leftEnterStartPos,
        Vector3 rightEnterStartPos,
        AnimationCurve enterAnimationCurve,
        Vector3 leftDownStartPos,
        Vector3 rightDownStartPos,
        AnimationCurve downAnimationCurve,
        Vector3 leftCombineStartPos,
        Vector3 rightCombineStartPos,
        Vector3 leftEndPos,
        Vector3 rightEndPos,
        AnimationCurve combineAnimationCurve)
    {
        this.leftEnterStartPos = leftEnterStartPos;
        this.rightEnterStartPos = rightEnterStartPos;
        this.enterAnimationCurve = enterAnimationCurve;
        this.leftDownStartPos = leftDownStartPos;
        this.rightDownStartPos = rightDownStartPos;
        this.downAnimationCurve = downAnimationCurve;
        this.leftCombineStartPos = leftCombineStartPos;
        this.rightCombineStartPos = rightCombineStartPos;
        this.leftEndPos = leftEndPos;
        this.rightEndPos = rightEndPos;

        combineAnimationLength = combineAnimationCurve.keys[combineAnimationCurve.length - 1].time;
        enterAnimationLength = enterAnimationCurve.keys[enterAnimationCurve.length - 1].time;
        downAnimationLength = downAnimationCurve.keys[downAnimationCurve.length - 1].time;
    }



    public void StartPlayingEnterAnimation()
    {
        isPlayingEnterAnim = true;
        enterAnimationStartTime = Time.time;
    }

    public void StartPlayingDownAnimation()
    {
        isPlayingDownAnim = true;
        downAnimationStartTime = Time.time;
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
                PlayMinimizeAnimation();
            }
            else if ((leftEndPos - this.leftCombineStartPos).magnitude < 0.01)//到达操作区
            {
                CountdownEvent.Raise();
            }
        }

        //Debug.Log(progress);
        float posPercent = Mathf.Clamp(animCurve.Evaluate(progress), 0, 1);
        Vector3 leftNowPos = Vector3.Lerp(leftStartPos, leftEndPos, posPercent);
        Vector3 rightNowPos = Vector3.Lerp(rightStartPos, rightEndPos, posPercent);
        leftObj.transform.position = leftNowPos;
        rightObj.transform.position = rightNowPos;
    }

    /// <summary>
    /// 播放缩小后的动画
    /// </summary>
    private void PlayMinimizeAnimation()
    {
        animator.SetBool("StartMinimize", true);
    }


    /// <summary>
    /// 缩小动画结束之后回调
    /// </summary>
    private void SetPieceMinimize()
    {
        doNotUpdate = true;
        PuzzlePieceManager.Instance.PlayNextPuzzlePairAnimation();
    }

    private void Update()
    {
        try
        {
            if (doNotUpdate)
            {
                return;
            }

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

            //debug用按键
            /*
            if (Input.GetKeyDown(KeyCode.A))
            {
                StartPlayingEnterAnimation();
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                StartPlayingDownAnimation();
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                StartPlayingCombineAnimation();
            }
            if (Input.GetKeyDown(KeyCode.F))
            {
                PlayMinimizeAnimation();
            }*/

            if (isPlayingCombineAnim)
            {
                PlayScriptedAnimation(ref combineAnimationStartTime, ref combineAnimationLength, ref isPlayingCombineAnim, ref combineAnimationCurve,
                    ref leftCombineStartPos, ref leftEndPos, ref rightCombineStartPos, ref rightEndPos);
            }
        }
        catch (Exception a)
        {
            Debug.Log("Input Manager not loaded!!");
        }
    }
}
