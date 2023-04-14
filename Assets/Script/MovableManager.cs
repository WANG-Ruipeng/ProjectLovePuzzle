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
	[Tooltip("注意所有坐标都是作用在更改在全局坐标系上，如果要统一全局和局部坐标系，需要Moveable及以上层级的物体均位于原点。")]
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

	List<Moveable> Moveables;

	int currentPieceNo = -2;//-2代表是目前关卡刚刚开始，拼图区内部还未有任何拼图

	public int totalRotateTime
	{
		get
		{
			int sum = 0;
			for (int i = 0; i < Moveables.Count; i++)
			{
				PuzzlePiecePair pair = Moveables[i] as PuzzlePiecePair;
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
	public void SetMoveableList(Moveable[] pzplist)
	{
		Moveables = new List<Moveable>(pzplist);
	}

	/// <summary>
	/// 初始化所有拼图
	/// </summary>
	public void InitPuzzles()
	{
		foreach (Moveable piecePair in Moveables)
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

		return Moveables[currentPieceNo] as PuzzlePiecePair;
	}

	public void Collect()
	{
		Moveables[currentPieceNo].Collect();
	}


	/// <summary>
	/// 目前只考虑每个边有三种情况，当玩家按下Lock的时候调用
	/// </summary>
	/// <returns>
	/// 拼合是否成功
	/// </returns>
	public bool Check()//TODO: 完善Check逻辑
	{
		return Moveables[currentPieceNo].Check();
	}

	/// <summary>
	/// 播放下一片拼图的动画，默认拼图数量大于5
	/// </summary>
	public void PlayNextPuzzlePairAnimation()
	{
		if (currentPieceNo == -2)
		{
			//Moveables = new List<Moveable>();
			Moveables[0].StartPlayingEnterAnimation();
			currentPieceNo++;
			return;
		}
		if (currentPieceNo == -1)
		{
			Moveables[1].StartPlayingEnterAnimation();
			Moveables[0].StartPlayingDownAnimation();
			currentPieceNo++;
			return;
		}
		if (currentPieceNo == Moveables.Count - 2)
		{
			Moveables[Moveables.Count - 1].StartPlayingDownAnimation();
			//Moveables[Moveables.Count - 2].StartPlayingCombineAnimation();
			currentPieceNo++;
			return;
		}
		if (currentPieceNo == Moveables.Count - 1)
		{
			//Moveables[Moveables.Count - 1].StartPlayingCombineAnimation();

			currentPieceNo++;
			GameManager.Instance.Win();
			return;
		}
		Moveables[currentPieceNo + 2].StartPlayingEnterAnimation();
		Moveables[currentPieceNo + 1].StartPlayingDownAnimation();
		//Moveables[currentPieceNo].StartPlayingCombineAnimation();
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
