using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//挂载在外部的拼图控制器，目前只考虑判定是否匹配
//考虑是否用这个Manager控制拼图的进入画面，退出画面等等比较宏观的事件，但还不确定这样写是否合适，所以暂时只写了一小部分逻辑
//假设已知PuzzlePiece的所有信息，通过LoadPuzzle导入到这个脚本中
public class PuzzlePieceManager : MonoBehaviour
{

    //static PuzzlePieceManager instance;
    ////从隔壁抄来的单例
    //public static PuzzlePieceManager Instance
    //{
    //    get
    //    {
    //        if (instance == null)
    //        {
    //            instance = FindObjectOfType<PuzzlePieceManager>();
    //            if (instance == null)
    //            {
    //                GameObject obj = new GameObject();
    //                obj.name = "PuzzlePieceManager";
    //                instance = obj.AddComponent<PuzzlePieceManager>();
    //            }
    //        }
    //        return instance;
    //    }
    //}
    [Header("Basic information about two pieces")]
    public GameObject leftObj;
    public GameObject rightObj;
    public Vector3 leftStartPos;
    public Vector3 rightStartPos;

    [Header("Conbine animation settings")]
    public Vector3 leftEndPos = new Vector3(-0.56f, 0, 0);
    public Vector3 rightEndPos = new Vector3(-0.56f, 0, 0);
    public AnimationCurve conbineAnimationCurve;

    PuzzlePiece left;
    PuzzlePiece right;
    bool isPlayingConbineAnim = false;
    float conbineAnimationLength;
    float conbineAnimationStartTime;

    private void Awake()
    {
        left = leftObj.GetComponentInChildren<PuzzlePiece>();
        right = rightObj.GetComponentInChildren<PuzzlePiece>();
        leftStartPos = leftObj.transform.position;
        rightStartPos = rightObj.transform.position;
        left.RotateClockWise = true;
        right.RotateClockWise = false;
        conbineAnimationLength = conbineAnimationCurve.keys[conbineAnimationCurve.length - 1].time;
    }

    /*
    public void LoadPuzzle(PuzzlePiece left, PuzzlePiece right)
    {
        this.left.Reset(left, 0);
        this.right.Reset(right, 0);
        test();//从PuzzlePiece中获取边的信息
    }
    void test()
    {
        left.edgeProp = new int[] { 0, 0, 0, -1 };
        right.edgeProp = new int[] { 0, 0, 0, 1 };
    }
    */

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
    /// 播放拼图合在一起的动画
    /// </summary>
    private void PlayPieceCombineAnimation()
    {
        float progress = (Time.time - conbineAnimationStartTime);
        if (progress >= conbineAnimationLength)
            isPlayingConbineAnim = false;

        float posPercent = Mathf.Clamp(conbineAnimationCurve.Evaluate(progress), 0, 1);
        Vector3 leftNowPos = Vector3.Lerp(leftEndPos, leftStartPos, posPercent);
        Vector3 rightNowPos = Vector3.Lerp(rightEndPos, rightStartPos, posPercent);
        leftObj.transform.position = leftNowPos;
        rightObj.transform.position = rightNowPos;
    }

    private void Update()
    {
        //这里负责在每次按下固定按键以后判定是否成功，逻辑写在这里而不是PuzzlePiece里面是因为减少脚本之间的依赖关系
        if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.P))
        {
            if (Check())
            {
                isPlayingConbineAnim = true;
                conbineAnimationStartTime = Time.time;
            }
            else
            {
                //Debug.Log("Failed!");
            }
        }

        if (isPlayingConbineAnim)
        {
            PlayPieceCombineAnimation();
        }
    }
}
