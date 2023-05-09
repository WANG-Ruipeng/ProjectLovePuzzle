using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PuzzlePiece : MonoBehaviour
{
	//旋转播放动画的曲线
	public AnimationCurve RotateCurve;
	public bool RotateClockWise;
	SpriteRenderer spriteRenderer;

	//旋转状态
	private bool isRotating = false;
	public bool IsRotating
	{
		get { return isRotating; }
	}
	private bool isLocked = false;
	public bool IsLocked
	{
		get { return isLocked; }
	}
	float rotateTimeLength = 0;
	float rotateStartTime = 0;
	float rotateStartAngle = 0;

	//饱和度的变化选择sprite
	[Header("拼图素材随旋转变化")]
	public Sprite[] puzzleSprites;

	public int state = 0;//当前在上方的边的序号作为当前状态，序号从上方开始顺时针排序（上0右1下2左3）如何排序可以再规定
	public const int edgeCount = 4;
	public enum EdgeProp
	{
		convex = 1,
		flat = 0,
		concave = -1
	}//用数字直接表示边比较容易误解，这里改成枚举.0表示平，1表示凸，-1表示凹
	public EdgeProp[] edgeProps = new EdgeProp[edgeCount];
	public List<Collectible> collections = new List<Collectible>();

	public int rotateTime;
	private void Awake()
	{
		foreach (Transform tran in GetComponentsInChildren<Transform>())
		{//遍历当前物体及其所有子物体
			tran.gameObject.layer = LayerMask.NameToLayer("Puzzle");//更改物体的Layer层 NoWrieframeEffect
			tran.gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Puzzle";
		}

	}
	public void Reset()
	{
		transform.localPosition = new Vector3(100, 100, 0);
		isRotating = false;
		isLocked = false;
		state = 0;
		transform.localRotation = Quaternion.Euler(0, 0, 0);
		transform.localScale = new Vector3(1, 1, 1);
		rotateTime = 0;
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	public void ReleaseLockStatus()
	{
		isLocked = false;
	}

	public void SetLockStatus()
	{
		isLocked = true;
	}

	private void PlayRotateAnimation()
	{
		float progress = (Time.time - rotateStartTime);
		if (progress >= rotateTimeLength)
			isRotating = false;

		float rotateZ = Mathf.Clamp(RotateCurve.Evaluate(progress), 0, 1) * 90;
		//本来想写个归一化的但是太懒了，记得提醒策划拉曲线的时候在0~1之间的范围拉
		Vector3 rot = transform.rotation.eulerAngles;
		if (RotateClockWise)
		{
			rot.z = (rotateStartAngle - rotateZ) % 360;
		}
		else
		{
			rot.z = (rotateStartAngle + rotateZ) % 360;
		}
		Quaternion rotQuat = Quaternion.Euler(rot.x, rot.y, rot.z);
		transform.rotation = rotQuat;
	}

	public void ChangeState()
	{
		isRotating = true;
		rotateStartTime = Time.time;
		rotateStartAngle = transform.rotation.eulerAngles.z;

		if (RotateClockWise)
			state = (state + edgeCount - 1) % edgeCount;//顺时针旋转，state--
		else
			state = (state + 1) % edgeCount;            //逆时针旋转，state++
		rotateTime++;

		if (puzzleSprites.Length == 0 || spriteRenderer == null)
		{
			Debug.LogWarning("No sprites in the puzzleSprites array.");
			return;
		}
		spriteRenderer.sprite = puzzleSprites[Mathf.Clamp(rotateTime, 0, puzzleSprites.Length - 1)];
	}

	/// <summary>
	/// 初始化边的信息
	/// </summary>
	private void InitEdgeProp()
	{
		//edgeProps = new edgeProp[edgeCount] { edgeProp.concave, edgeProp.concave, edgeProp.concave, edgeProp.concave };
	}

	// Start is called before the first frame update
	void Start()
	{
		rotateTimeLength = RotateCurve.keys[RotateCurve.length - 1].time;
		isRotating = false;
		isLocked = false;
		//indicatorSprite = LockIndicator.GetComponent<SpriteRenderer>();
		InitEdgeProp();
	}

	// Update is called once per frame
	void Update()
	{
		if (!isLocked && isRotating)
		{
			PlayRotateAnimation();
		}

	}
}
