using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public GameObject[] puzzlePairs;

    float enterAnimationLength = 0;
    float downAnimationLength = 0;
    float combineAnimationLength = 0;

    PuzzlePiece left;
    PuzzlePiece right;

    bool isPlayingEnterAnim = false;
    float enterAnimationStartTime = 0;

    bool isPlayingDownAnim = false;
    float downAnimationStartTime = 0;

    bool isPlayingCombineAnim = false;
    float combineAnimationStartTime = 0;

    Animator animator;

    bool isMinimized = false;

    private void Awake()
    {
        left = leftObj.GetComponentInChildren<PuzzlePiece>();
        right = rightObj.GetComponentInChildren<PuzzlePiece>();
        left.RotateClockWise = true;
        right.RotateClockWise = false;
        
        animator = GetComponent<Animator>();
    }

    /// <summary>
    /// 目前只考虑每个边有三种情况，当玩家按下Lock的时候调用
    /// </summary>
    /// <returns></returns>
    public bool Check()
    {
        if (left.isLocked && right.isLocked)
        {
            int edgeCnt = PuzzlePiece.edgeCount;
            PuzzlePiece.edgeProp leftStatus = left.edgeProps[(left.state + 1) % edgeCnt];
            PuzzlePiece.edgeProp rightStatus = right.edgeProps[(right.state + edgeCnt - 1) % edgeCnt];
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

    /// <summary>
    /// 播放拼图从上方落入屏幕的动画
    /// </summary>
    private void PlayEnterSceneAnimation()
    {
        float progress = (Time.time - enterAnimationStartTime);
        if (progress >= enterAnimationLength)
            isPlayingEnterAnim = false;

        float posPercent = Mathf.Clamp(enterAnimationCurve.Evaluate(progress), 0, 1);
        Vector3 leftNowPos = Vector3.Lerp(leftEnterStartPos, leftDownStartPos, posPercent);
        Vector3 rightNowPos = Vector3.Lerp(rightEnterStartPos, rightDownStartPos, posPercent);
        leftObj.transform.position = leftNowPos;
        rightObj.transform.position = rightNowPos;
    }

    /// <summary>
    /// 播放拼图从上方落入等待区的动画
    /// </summary>
    private void PlayGoDownAnimation()
    {
        float progress = (Time.time - downAnimationStartTime);
        if (progress >= downAnimationLength)
            isPlayingDownAnim = false;

        float posPercent = Mathf.Clamp(downAnimationCurve.Evaluate(progress), 0, 1);
        Vector3 leftNowPos = Vector3.Lerp(leftDownStartPos, leftCombineStartPos, posPercent);
        Vector3 rightNowPos = Vector3.Lerp(rightDownStartPos, rightCombineStartPos, posPercent);
        leftObj.transform.position = leftNowPos;
        rightObj.transform.position = rightNowPos;
    }

    /// <summary>
    /// 播放拼图合在一起的动画
    /// </summary>
    private void PlayPieceCombineAnimation()
    {
        float progress = (Time.time - combineAnimationStartTime);
        if (progress >= combineAnimationLength)
        {
            isPlayingCombineAnim = false;
            PlayMinimizeAnimation();
        }

        float posPercent = Mathf.Clamp(combineAnimationCurve.Evaluate(progress), 0, 1);
        Vector3 leftNowPos = Vector3.Lerp(leftCombineStartPos, leftEndPos, posPercent);
        Vector3 rightNowPos = Vector3.Lerp(rightCombineStartPos, rightEndPos, posPercent);
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
        isMinimized = true;
    }

    private void Update()
    {
        if (isMinimized)
        {
            return;
        }

        if (isPlayingEnterAnim)
        {
            PlayEnterSceneAnimation();
            return;
        }

        if (isPlayingDownAnim)
        {
            PlayGoDownAnimation();
            return;
        }

        //debug用按键
        if (Input.GetKeyDown(KeyCode.A))
        {
            isPlayingEnterAnim = true;
            enterAnimationStartTime = Time.time;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            isPlayingDownAnim = true;
            downAnimationStartTime = Time.time;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            PlayMinimizeAnimation();
        }

        if (!left.isRotating)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                left.ChangeState();
            }

            if (Input.GetKeyDown(KeyCode.W))
            {
                if (left.isLocked)
                    left.ReleaseLockStatus();
                else
                    left.SetLockStatus();
            }
        }

        if (!right.isRotating) 
        {
            if (Input.GetKeyDown(KeyCode.O))
            {
                right.ChangeState();
            }

            if (Input.GetKeyDown(KeyCode.P))
            {
                if (right.isLocked)
                    right.ReleaseLockStatus();
                else
                    right.SetLockStatus();
            }
        }

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.P))
        {
            if (Check())
            {
                isPlayingCombineAnim = true;
                combineAnimationStartTime = Time.time;
            }
            else if(left.isLocked && right.isLocked)
            {
                left.ReleaseLockStatus();
                right.ReleaseLockStatus();
            }
        }

        if (isPlayingCombineAnim)
        {
            PlayPieceCombineAnimation();
        }
    }
}
