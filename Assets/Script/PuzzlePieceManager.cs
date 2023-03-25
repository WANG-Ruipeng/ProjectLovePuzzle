using Giro;
using HyperCasual.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzlePieceManager : MonoBehaviour
{
    public static PuzzlePieceManager Instance => s_Instance;
    static PuzzlePieceManager s_Instance;

    [Header("Enter scene animation settings")]
    [Tooltip("注意所有坐标都是作用在更改在全局坐标系上，如果要统一全局和局部坐标系，需要PuzzlePiecePair及以上层级的物体均位于原点。")]
    public float seceondEnterDelay = 1.5f;
    public Vector3 leftEnterStartPos = new Vector3(-1.56f, 10, 0);
    public Vector3 rightEnterStartPos = new Vector3(1.56f, 10, 0);
    public AnimationCurve enterAnimationCurve;

    [Header("Go down animation settings")]
    public Vector3 leftDownStartPos = new Vector3(-1.56f, 4, 0);
    public Vector3 rightDownStartPos = new Vector3(1.56f, 4, 0);
    public AnimationCurve downAnimationCurve;

    [Header("Combine animation settings")]
    public Vector3 leftCombineStartPos = new Vector3(-1.56f, 0, 0);
    public Vector3 rightCombineStartPos = new Vector3(1.56f, 0, 0);
    public Vector3 leftEndPos = new Vector3(-0.56f, 0, 0);
    public Vector3 rightEndPos = new Vector3(0.56f, 0, 0);
    public AnimationCurve combineAnimationCurve;

    List<PuzzlePiecePair> puzzlePiecePairs;

    int currentPieceNo = -2;//-2代表是目前关卡刚刚开始，拼图区内部还未有任何拼图

    public void Reset()
    {
        currentPieceNo = -2;
        PlayNextPuzzlePairAnimation();
        StartCoroutine(SecondEnter());


    }
    IEnumerator SecondEnter()
    {
        yield return new WaitForSeconds(seceondEnterDelay);
        PlayNextPuzzlePairAnimation();

    }
    public void SetPuzzlePiecePairList(PuzzlePiecePair[] pzplist)
    {
        puzzlePiecePairs = new List<PuzzlePiecePair>(pzplist);
    }

    /// <summary>
    /// 初始化所有拼图
    /// </summary>
    void InitPuzzles()
    {
        foreach (PuzzlePiecePair piecePair in puzzlePiecePairs)
        {
            piecePair.SetAllAnimationParamters(leftEnterStartPos, rightEnterStartPos, enterAnimationCurve,
                leftDownStartPos, rightDownStartPos, downAnimationCurve,
                leftCombineStartPos, rightCombineStartPos,
                leftEndPos, rightEndPos, combineAnimationCurve);
        }
    }

    public PuzzlePiecePair GetCurrentPuzzlePair()
    {
        return puzzlePiecePairs[currentPieceNo];
    }

    /// <summary>
    /// 目前只考虑每个边有三种情况，当玩家按下Lock的时候调用
    /// </summary>
    /// <returns>
    /// 拼合是否成功
    /// </returns>
    public bool Check()
    {
        if (puzzlePiecePairs[currentPieceNo].left.IsLocked && puzzlePiecePairs[currentPieceNo].right.IsLocked)
        {
            int edgeCnt = PuzzlePiece.edgeCount;
            PuzzlePiece.edgeProp leftStatus = puzzlePiecePairs[currentPieceNo].left.edgeProps[(puzzlePiecePairs[currentPieceNo].left.state + 1) % edgeCnt];
            PuzzlePiece.edgeProp rightStatus = puzzlePiecePairs[currentPieceNo].right.edgeProps[(puzzlePiecePairs[currentPieceNo].right.state + edgeCnt - 1) % edgeCnt];
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
    /// 播放下一片拼图的动画，默认拼图数量大于5
    /// </summary>
    public void PlayNextPuzzlePairAnimation()
    {
        if (currentPieceNo == -2)
        {
            //puzzlePiecePairs = new List<PuzzlePiecePair>();
            InitPuzzles();
            puzzlePiecePairs[0].StartPlayingEnterAnimation();
            currentPieceNo++;
            return;
        }
        if (currentPieceNo == -1)
        {
            puzzlePiecePairs[1].StartPlayingEnterAnimation();
            puzzlePiecePairs[0].StartPlayingDownAnimation();
            currentPieceNo++;
            return;
        }
        if (currentPieceNo == puzzlePiecePairs.Count - 2)
        {
            puzzlePiecePairs[puzzlePiecePairs.Count - 1].StartPlayingDownAnimation();
            puzzlePiecePairs[puzzlePiecePairs.Count - 1].isFinalStep = true;
            //puzzlePiecePairs[puzzlePiecePairs.Count - 2].StartPlayingCombineAnimation();
            currentPieceNo++;
            return;
        }
        if (currentPieceNo == puzzlePiecePairs.Count - 1)
        {
            //puzzlePiecePairs[puzzlePiecePairs.Count - 1].StartPlayingCombineAnimation();
            currentPieceNo++;
            return;
        }
        puzzlePiecePairs[currentPieceNo + 2].StartPlayingEnterAnimation();
        puzzlePiecePairs[currentPieceNo + 1].StartPlayingDownAnimation();
        //puzzlePiecePairs[currentPieceNo].StartPlayingCombineAnimation();
        currentPieceNo++;
    }

    private void Awake()
    {
        if (s_Instance != null && s_Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        s_Instance = this;

    }
}
