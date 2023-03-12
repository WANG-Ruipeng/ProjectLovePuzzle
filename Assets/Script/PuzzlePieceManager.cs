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

    [Header("Enter scene animation settings")]
    [Tooltip("注意所有坐标都是作用在更改在全局坐标系上，如果要统一全局和局部坐标系，需要PuzzlePiecePair及以上层级的物体均位于原点。")]
    public Vector3 leftEnterStartPos = new Vector3(-1.56f, 10, 0);
    public Vector3 rightEnterStartPos = new Vector3(1.56f, 10, 0);
    public AnimationCurve enterAnimationCurve;

    [Header("Combine animation settings")]
    public Vector3 leftCombineStartPos  = new Vector3(-1.56f, 0, 0);
    public Vector3 rightCombineStartPos = new Vector3(1.56f, 0, 0);
    public Vector3 leftEndPos = new Vector3(-0.56f, 0, 0);
    public Vector3 rightEndPos = new Vector3(0.56f, 0, 0);
    public AnimationCurve combineAnimationCurve;

    PuzzlePiece left;
    PuzzlePiece right;

    bool isPlayingEnterAnim = false;
    float enterAnimationLength = 0;
    float enterAnimationStartTime = 0;
    
    bool isPlayingCombineAnim = false;
    float combineAnimationLength = 0;
    float combineAnimationStartTime = 0;

    Animator animator;
    //bool isPlayingMinimizeAnim = false;

    private void Awake()
    {
        left = leftObj.GetComponentInChildren<PuzzlePiece>();
        right = rightObj.GetComponentInChildren<PuzzlePiece>();
        left.RotateClockWise = true;
        right.RotateClockWise = false;
        combineAnimationLength = combineAnimationCurve.keys[combineAnimationCurve.length - 1].time;
        enterAnimationLength = enterAnimationCurve.keys[enterAnimationCurve.length - 1].time;
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
    /// 播放拼图从上方落下的动画
    /// </summary>
    private void PlayEnterSceneAnimation()
    {
        float progress = (Time.time - enterAnimationStartTime);
        if (progress >= enterAnimationLength)
            isPlayingEnterAnim = false;

        float posPercent = Mathf.Clamp(enterAnimationCurve.Evaluate(progress), 0, 1);
        Vector3 leftNowPos = Vector3.Lerp(leftEnterStartPos, leftCombineStartPos, posPercent);
        Vector3 rightNowPos = Vector3.Lerp(rightEnterStartPos, rightCombineStartPos, posPercent);
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
        Vector3 leftNowPos = Vector3.Lerp(leftCombineStartPos, leftEndPos , posPercent);
        Vector3 rightNowPos = Vector3.Lerp(rightCombineStartPos, rightEndPos , posPercent);
        leftObj.transform.position = leftNowPos;
        rightObj.transform.position = rightNowPos;
    }

    /// <summary>
    /// 播放缩小后的动画
    /// </summary>
    private void PlayMinimizeAnimation() 
    {
        animator.SetBool("isMinimized",true);
    }

    private void Update()
    {
        if (isPlayingEnterAnim)
        {
            PlayEnterSceneAnimation();
            return;
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            isPlayingEnterAnim = true;
            enterAnimationStartTime = Time.time;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            PlayMinimizeAnimation();
        }

        //这里负责在每次按下固定按键以后判定是否成功，逻辑写在这里而不是PuzzlePiece里面是因为减少脚本之间的依赖关系
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.P))
        {
            if (Check())
            {
                isPlayingCombineAnim = true;
                combineAnimationStartTime = Time.time;
            }
            else if(left.isLocked && right.isLocked)
            {
                //拼图失败之后
                //Debug.Log("Failed!");
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
