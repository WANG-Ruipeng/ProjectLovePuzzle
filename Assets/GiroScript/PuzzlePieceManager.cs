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
    public PuzzleInfo left;
    public PuzzleInfo right;

    private void Awake()
    {
        left = new PuzzleInfo();
        right = new PuzzleInfo();
        left.isLeft = true;
        right.isLeft = false;
    }

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
    /// <summary>
    /// 目前只考虑每个边有三种情况，当玩家按下Lock的时候调用
    /// </summary>
    /// <returns></returns>
    public bool Check()
    {
        //↓需要获取isLocked属性（在puzzlePiece中添加public）
        //if (left.isLocked || right.isLocked) return false;
        int edgeCnt = PuzzleInfo.edgeCount;
        //左的拼图需要检测right edge的状态，右的拼图需要检测left edge 的状态
        if (left.edgeProp[(left.state + 1) % edgeCnt] == 0 || right.edgeProp[(right.state + edgeCnt - 1) % edgeCnt] == 0)
            return false;
        if (left.edgeProp[(left.state + 1) % edgeCnt] + right.edgeProp[(right.state + edgeCnt - 1) % edgeCnt] == 0)
            return true;
        return false;
    }

    /// <summary>
    /// 有一些希望加入到PuzzlePiece的属性，害怕冲突所以先写在这个封装类里
    /// </summary>
    public class PuzzleInfo
    {
        public PuzzlePiece puzzle;
        public int state = 0;//当前在上方的边的序号作为当前状态，序号从上方开始顺时针排序（上0右1下2左3）如何排序可以再规定
        public const int edgeCount = 4;
        public int[] edgeProp = new int[edgeCount];//，0表示平，1表示凸，-1表示凹
                                                   //↑通过读取文档或者提前在puzzlePiece里面的设置来获取
        public bool isLeft;//由PuzzlePiece从关卡设置或文档中读取
        public void ChangeState()//这个函数应该卸载PuzzlePiece里面，当旋转的时候调用
        {
            if (isLeft)
                state = (state + edgeCount - 1) % 4;//顺时针旋转，state--
            else
                state = (state + 1) % 4;            //逆时针旋转，state++
        }
        public void Reset(PuzzlePiece puzzle, int state)//或者Initialize
        {
            this.puzzle = puzzle;
            this.state = state;
            if (state > 0)
                puzzle.gameObject.transform.Rotate(new Vector3(0, 0, state % edgeCount * 90));
        }
    }
}
