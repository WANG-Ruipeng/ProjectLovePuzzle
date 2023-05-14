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


		[Header("平台(事件点)屏幕位置定位")]
		[Header("从屏幕外进入预备区的开始位置")]
		public Vector3 pos1 = new Vector3(-1.56f, 10, 0);

		[Header("预备区的位置")]
		public Vector3 pos2 = new Vector3(-1.56f, 4, 0);

		[Header("事件播放区的位置")]
		public Vector3 pos3 = new Vector3(-1.56f, 0, 0);
		[Header("离开事件播放区后的位置")]
		public Vector3 pos4;
		[Header("从屏幕外到达预备区的曲线")]
		public AnimationCurve platformEnterAnimationCurve;
		[Header("从预备区到达事件播放区的曲线")]
		public AnimationCurve platformDownAnimationCurve;

		[Header("离开曲线")]
		public AnimationCurve platformExitAnimationCurve;


		[Header("拼图屏幕位置定位")]
		[Header("从屏幕外进入预备区的开始位置")]
		public Vector3 lPos1 = new Vector3(-1.56f, 10, 0);
		public Vector3 rPos1 = new Vector3(1.56f, 10, 0);

		[Header("预备区的位置")]
		public Vector3 lPos2 = new Vector3(-1.56f, 4, 0);
		public Vector3 rPos2 = new Vector3(1.56f, 4, 0);

		[Header("操作区合并前的位置")]
		public Vector3 lPos3 = new Vector3(-1.56f, 0, 0);
		public Vector3 rPos3 = new Vector3(1.56f, 0, 0);
		[Header("操作区合并后的位置")]
		public Vector3 lPos4 = new Vector3(-0.56f, 0, 0);
		public Vector3 rPos4 = new Vector3(0.56f, 0, 0);
		[Header("离开操作区后的位置")]
		public Vector3 lPos5;
		public Vector3 rPos5;
		[Header("从屏幕外到达预备区的曲线")]
		public AnimationCurve puzzlesEnterAnimationCurve;
		[Header("从预备区到达操作区的曲线")]
		public AnimationCurve puzzlesDownAnimationCurve;
		[Header("合并曲线")]
		public AnimationCurve puzzlesCombineAnimationCurve;
		[Header("离开曲线")]
		public AnimationCurve puzzlesExitAnimationCurve;

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
			[Tooltip("注意：如果需要添加收藏品，请确保收藏品上挂载了对应的脚本")]
			public CollectibleInfo[] lCollectibleInfos;
			[Header("右")]
			public int rPos;
			public GameObject rStepPrefab;
			[Tooltip("注意：如果需要添加收藏品，请确保收藏品上挂载了对应的脚本")]
			public CollectibleInfo[] rCollectibleInfos;
		}

		[Serializable]
		public class CollectibleInfo
		{
			[Header("不需则不填")]
			public GameObject prefab;
			[Header("是否复用（必填）")]
			public bool reusable;
			[Header("收集所需旋转次数（边收集物）")]
			public int onEdge;
			[Header("收集至少需要旋转次数（旋转收集物）")]
			public int minRotateTime;
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