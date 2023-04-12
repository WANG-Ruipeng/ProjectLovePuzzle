using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moveable : MonoBehaviour
{

	bool doNotUpdate = false;

	[Header("Basic information about two pieces")]
	public int ID;

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

	public Vector3 leftExitPos;
	public Vector3 rightExitPos;
	public AnimationCurve exitAnimationCurve;

	protected float enterAnimationLength = 100;
	protected float downAnimationLength = 0;
	protected float combineAnimationLength = 0;
	protected float exitAnimationLength = 0;

	public bool isPlayingEnterAnim = false;
	public float enterAnimationStartTime = 0;

	public bool isPlayingDownAnim = false;
	public float downAnimationStartTime = 0;

	public bool isPlayingCombineAnim = false;
	public float combineAnimationStartTime = 0;

	public bool isPlayingExitAnim = false;
	public float exitAnimationStartTime = 0;


	private void Awake()
	{
	}

	public virtual void Reset()
	{
		doNotUpdate = false;
		enterAnimationLength = enterAnimationCurve.keys[enterAnimationCurve.length - 1].time;
		downAnimationLength = downAnimationCurve.keys[downAnimationCurve.length - 1].time;
		exitAnimationLength = exitAnimationCurve.keys[exitAnimationCurve.length - 1].time;
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
		AnimationCurve combineAnimationCurve,
		Vector3 leftExitPos,
		Vector3 rightExitPos,
		AnimationCurve exitAnimationCurve)
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
		this.leftExitPos = leftExitPos;
		this.rightExitPos = rightExitPos;
		this.exitAnimationCurve = exitAnimationCurve;

		combineAnimationLength = combineAnimationCurve.keys[combineAnimationCurve.length - 1].time;
		enterAnimationLength = enterAnimationCurve.keys[enterAnimationCurve.length - 1].time;
		downAnimationLength = downAnimationCurve.keys[downAnimationCurve.length - 1].time;
		exitAnimationLength = exitAnimationCurve.keys[exitAnimationCurve.length - 1].time;
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
	public void StartPlayExitAnimation()
	{
		isPlayingExitAnim = true;
		exitAnimationStartTime = Time.time;
	}

	//TODO 判断是不是最后一个
	IEnumerator waitMinimizeAnimationStop()
	{
		yield return null;

	}

	public virtual void Collect()
	{

	}
	public virtual bool Check()
	{
		return true;
	}
	public virtual void Init()
	{

	}

	/// <summary>
	/// 缩小动画结束之后回调
	/// </summary>
	private void SetPieceMinimize()
	{
		doNotUpdate = true;
		MovableManager.Instance.PlayNextPuzzlePairAnimation();
	}

	virtual protected void OnUpdate()
	{

	}
	private void Update()
	{
		try
		{
			if (doNotUpdate)
			{
				return;
			}
			OnUpdate();


		}
		catch (Exception a)
		{
			Debug.Log("Input Manager not loaded!!");
		}
	}
}
