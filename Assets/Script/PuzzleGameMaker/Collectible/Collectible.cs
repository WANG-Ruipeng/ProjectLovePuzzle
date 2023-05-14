using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Giro;
/// <summary>
/// Collectible只在关卡内作为被收集物被加载
/// </summary>
public class Collectible : MonoBehaviour
{
	public int id;
	public bool unlocked;
	public bool reusable;

	public void Awake()
	{
		if (SaveManager.Instance)//获取存档信息，如果本收藏品已被解锁则不再加载（销毁自身）
		{
			CollectibleSaveInfo info = SaveManager.Instance.LoadCollectibleInfo(id);
			unlocked = info.unlocked;
			if (unlocked)
			{
				Destroy(gameObject);
			}
		}
	}
	public virtual bool Check(PuzzlePiece puzzlePiece)
	{
		return true;
	}
	public virtual void Collected()//用来触发被收集后的动画之类的
	{
		Debug.Log(gameObject.name + "被收集了啊！");
		if (!reusable)
			unlocked = true;
		gameObject.SetActive(false);
	}
	public virtual void SetData(LevelDefinition.CollectibleInfo collectibleInfo)
	{
		reusable = collectibleInfo.reusable;
	}
}
