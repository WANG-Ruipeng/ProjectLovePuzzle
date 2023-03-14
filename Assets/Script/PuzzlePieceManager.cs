using Giro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzlePieceManager : MonoBehaviour
{

    [Header("Enter scene animation settings")]
    [Tooltip("注意所有坐标都是作用在更改在全局坐标系上，如果要统一全局和局部坐标系，需要PuzzlePiecePair及以上层级的物体均位于原点。")]
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

    int currentPieceNo = -2;//-1代表是目前关卡刚刚开始，拼图区内部还未有任何拼图

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

    /// <summary>
    /// 播放下一片拼图的动画，默认拼图数量大于5
    /// </summary>
    void PlayNextPuzzlePairAnimation() 
    { 
        if(currentPieceNo == -2)
        {
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
        puzzlePiecePairs[currentPieceNo + 2].StartPlayingEnterAnimation();
        puzzlePiecePairs[currentPieceNo + 1].StartPlayingDownAnimation();
        puzzlePiecePairs[currentPieceNo].StartPlayingCombineAnimation();
        currentPieceNo++;
        if (currentPieceNo == puzzlePiecePairs.Count - 2)
        {
            puzzlePiecePairs[puzzlePiecePairs.Count - 1].StartPlayingDownAnimation();
            puzzlePiecePairs[puzzlePiecePairs.Count - 2].StartPlayingCombineAnimation();
            currentPieceNo++;
            return;
        }
        if (currentPieceNo == puzzlePiecePairs.Count - 1)
        {
            puzzlePiecePairs[puzzlePiecePairs.Count - 1].StartPlayingCombineAnimation();
            currentPieceNo++;
            return;
        }
    }

    private void Start()
    {
        puzzlePiecePairs = new List<PuzzlePiecePair>(LevelManager.Instance.puzzlePieceInScene);
        InitPuzzles();
    }
}
