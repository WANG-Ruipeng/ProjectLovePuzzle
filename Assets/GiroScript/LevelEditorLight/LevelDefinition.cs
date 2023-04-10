using System;
using System.Collections;
using System.Collections.Generic;
using HyperCasual.Core;
using UnityEngine;

namespace Giro
{
	/// <summary>
	/// A scriptable object that stores all information
	/// needed to load and set up a Runner level.
	/// </summary>
	[CreateAssetMenu(fileName = "Data", menuName = "Giro/LevelDefinition", order = 1)]
	public class LevelDefinition : AbstractLevelData
	{
		public Background background;
		public GameObject puzzlePiecePairPrefab;
		[Header("倒计时长")]
		public float maxCountdown;

		[Header("最开始两块拼图进入屏幕的时间间隔")]
		public float seceondEnterDelay = 1.5f;
		[Tooltip("注意所有坐标都是作用在更改在全局坐标系上，如果要统一全局和局部坐标系，需要PuzzlePiecePair及以上层级的物体均位于原点。")]
		[Header("注意调整位置前应该先把会用到的拼图的缩放、轴心等关键信息设置好，以免出现错位")]
		[Header("从屏幕外进入预备区的开始位置")]
		public Vector3 leftEnterStartPos = new Vector3(-1.56f, 10, 0);
		public Vector3 rightEnterStartPos = new Vector3(1.56f, 10, 0);

		[Header("预备区的位置")]
		public Vector3 leftDownStartPos = new Vector3(-1.56f, 4, 0);
		public Vector3 rightDownStartPos = new Vector3(1.56f, 4, 0);

		[Header("操作区的位置")]
		public Vector3 leftCombineStartPos = new Vector3(-1.56f, 0, 0);
		public Vector3 rightCombineStartPos = new Vector3(1.56f, 0, 0);
		public Vector3 leftEndPos = new Vector3(-0.56f, 0, 0);
		public Vector3 rightEndPos = new Vector3(0.56f, 0, 0);
		[Header("离开操作区后的位置")]
		public Vector3 leftExitPos;
		public Vector3 rightExitPos;
		[Header("从屏幕外到达预备区的曲线")]
		public AnimationCurve enterAnimationCurve;
		[Header("从预备区到达操作区的曲线")]
		public AnimationCurve downAnimationCurve;
		[Header("合并曲线")]
		public AnimationCurve combineAnimationCurve;
		[Header("离开曲线")]
		public AnimationCurve exitAnimationCurve;

		[Header("拼图旋转曲线")]
		public AnimationCurve rotateCurve;

		[Header("按顺序填入关卡中每一步用到的拼图")]
		public PuzzleStep[] puzzleSteps;

		[System.Serializable]
		public class PuzzleStep
		{
			public bool isPlatform;
			public GameObject platformObj;
			[Header("拼图片信息（事件点不需填写）")]
			[Header("左")]
			public int lPos;
			public GameObject lStepPrefab;
			[Tooltip("注意：如果需要添加收藏品，请将收藏品的元素个数调整至与边数一样多，然后在需要添加的边上拖入收藏品，其余的设为None")]
			public GameObject[] lCollectiblePrefabs;
			[Header("右")]
			public int rPos;
			public GameObject rStepPrefab;
			[Tooltip("注意：如果需要添加收藏品，请将收藏品的元素个数调整至与边数一样多，然后在需要添加的边上拖入收藏品，其余的设为None")]
			public GameObject[] rCollectiblePrefabs;
		}

		/// <summary>
		/// Store all values from updatedLevel into this LevelDefinition.
		/// </summary>
		/// <param name="updatedLevel">
		/// The LevelDefinition that has been edited in the Runner Level Editor Window.
		/// </param>
		public void SaveValues(LevelDefinition updatedLevel)
		{
			try
			{
				background = updatedLevel.background;
				puzzlePiecePairPrefab = updatedLevel.puzzlePiecePairPrefab;
				maxCountdown = updatedLevel.maxCountdown;
				puzzleSteps = updatedLevel.puzzleSteps;
			}
			catch (Exception e)
			{
				Debug.Log(e.ToString() + "!!   please check this level or connect to programmer");
			}
		}
	}
}