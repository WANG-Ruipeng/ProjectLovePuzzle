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

    List<GameObject> puzzlePiecePairsObj = new List<GameObject>();
    List<PuzzlePiecePair> puzzlePiecePairs = new List<PuzzlePiecePair>();

    float enterAnimationLength = 0;
    float downAnimationLength = 0;
    float combineAnimationLength = 0;

    /// <summary>
    /// 负责获取
    /// </summary>
    void GetAllPiecePairs()
    {
        
        foreach (Transform childTransform in transform)
        {
            if (childTransform.name.StartsWith("PuzzlePair_"))
            {
                puzzlePiecePairsObj.Add(childTransform.gameObject);
            }
        }

        foreach(GameObject piece in puzzlePiecePairsObj)
        {
            puzzlePiecePairs.Add(piece.GetComponent<PuzzlePiecePair>());
        }
    }

    void InitPaint()
    {

    }

    private void Start()
    {
        combineAnimationLength = combineAnimationCurve.keys[combineAnimationCurve.length - 1].time;
        enterAnimationLength = enterAnimationCurve.keys[enterAnimationCurve.length - 1].time;
        downAnimationLength = downAnimationCurve.keys[downAnimationCurve.length - 1].time;

        GetAllPiecePairs();
    }
}
