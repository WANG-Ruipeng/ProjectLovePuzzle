using Giro;
using HyperCasual.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableManager : MonoBehaviour
{
	public static MovableManager Instance => s_Instance;
	static MovableManager s_Instance;

	[Header("Enter scene animation settings")]
	[Tooltip("注意所有坐标都是作用在更改在全局坐标系上，如果要统一全局和局部坐标系，需要movable及以上层级的物体均位于原点。")]
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

	public Vector3 leftExitPos = new Vector3();
	public Vector3 rightExitPos = new Vector3();
	public AnimationCurve exitAnimationCurve;

	List<Movable> movables;

	public int StepNum => movables.Count;
	public int Progress
	{
		get
		{
			if (currentPieceNo < 0) return 0;
			return currentPieceNo;
		}
	}
	int currentPieceNo = -2;//-2代表是目前关卡刚刚开始，拼图区内部还未有任何拼图

	public int totalRotateTime
	{
		get
		{
			int sum = 0;
			for (int i = 0; i < movables.Count; i++)
			{
				PuzzlePiecePair pair = movables[i] as PuzzlePiecePair;
				if (pair)
				{
					sum += pair.left.rotateTime;
					sum += pair.right.rotateTime;
				}
			}
			return sum;
		}
	}

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
	public void SetmovableList(Movable[] pzplist)
	{
		movables = new List<Movable>(pzplist);
	}

	/// <summary>
	/// 初始化所有拼图
	/// </summary>
	public void InitPuzzles()
	{
		foreach (Movable piecePair in movables)
		{
			piecePair.SetAllAnimationParamters(leftEnterStartPos, rightEnterStartPos, enterAnimationCurve,
				leftDownStartPos, rightDownStartPos, downAnimationCurve,
				leftCombineStartPos, rightCombineStartPos,
				leftEndPos, rightEndPos, combineAnimationCurve,
				leftExitPos, rightExitPos, exitAnimationCurve);
		}
	}

	public PuzzlePiecePair GetCurrentPuzzlePair()
	{

		return movables[currentPieceNo] as PuzzlePiecePair;
	}

	public void Collect()
	{
		movables[currentPieceNo].Collect();
	}


	/// <summary>
	/// 目前只考虑每个边有三种情况，当玩家按下Lock的时候调用
	/// </summary>
	/// <returns>
	/// 拼合是否成功
	/// </returns>
	public bool Check()//TODO: 完善Check逻辑
	{
		return movables[currentPieceNo].Check();
	}

	/// <summary>
	/// 播放下一片拼图的动画，默认拼图数量大于5
	/// </summary>
	public void PlayNextPuzzlePairAnimation()
	{
		if (currentPieceNo == -2)
		{
			//movables = new List<movable>();
			movables[0].PlayNextAnimation();
			currentPieceNo++;
			return;
		}
		if (currentPieceNo == -1)
		{
			movables[1].PlayNextAnimation();
			movables[0].PlayNextAnimation();
			currentPieceNo++;
			return;
		}
		if (currentPieceNo == movables.Count - 2)
		{
			movables[movables.Count - 1].PlayNextAnimation();
			currentPieceNo++;
			return;
		}
		if (currentPieceNo == movables.Count - 1)
		{
			currentPieceNo++;
			GameManager.Instance.Win();
			return;
		}
		movables[currentPieceNo + 2].PlayNextAnimation();
		movables[currentPieceNo + 1].PlayNextAnimation();
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
