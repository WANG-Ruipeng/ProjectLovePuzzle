using Doozy.Runtime.UIManager.Components;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PagesScreen : MonoBehaviour
{
	public int id;

	public UISelectable selectable;
	public SpriteRenderer spriteRenderer;
	public SpriteSet[] spriteset;
	public int currentPage = 0;
	public Action<int> finalHandle;
	List<SpriteRenderer> subSpriteRenders = new List<SpriteRenderer>();
	public void Awake()
	{
		selectable = GetComponent<UISelectable>();
		selectable.interactable = false;
		spriteRenderer.sortingLayerName = "UI";
		spriteRenderer.sortingOrder = 1;
	}
	public void ShowNextPage()
	{
		if (currentPage == spriteset.Length)//最后一个的时候触发
		{
			spriteRenderer.sprite = null;
			selectable.interactable = false;
			finalHandle?.Invoke(id);
			currentPage = 0;
			return;
		}
		if (currentPage == 0)
		{
			selectable.interactable = true;
		}

		spriteRenderer.sprite = spriteset[currentPage].sprite;
		var subSprite = spriteset[currentPage].subSprite;
		if (spriteset[currentPage].currentSubSprite < subSprite.Length)
		{
			var sr = GetEmptySpriteRender();
			sr.sprite = subSprite[spriteset[currentPage].currentSubSprite];
			spriteset[currentPage].currentSubSprite++;
		}
		else
		{
			spriteset[currentPage].currentSubSprite = 0;
			for (int i = 0; i < subSpriteRenders.Count; i++)
			{
				subSpriteRenders[i].sprite = null;
			}
			currentPage++;
			ShowNextPage();//再次调用加载下一页
		}
	}

	SpriteRenderer GetEmptySpriteRender()
	{
		int i;
		for (i = 0; i < subSpriteRenders.Count; i++)
			if (subSpriteRenders[i].sprite == null)
				return subSpriteRenders[i];
		GameObject child = new GameObject("SubSprite" + i);
		child.transform.parent = spriteRenderer.transform;
		var sr = child.AddComponent<SpriteRenderer>();
		sr.sortingLayerName = "UI";
		sr.sortingOrder = 2;
		subSpriteRenders.Add(sr);
		return sr;
	}
	[Serializable]
	public class SpriteSet
	{
		public Sprite sprite;
		public Sprite[] subSprite;
		[HideInInspector]
		public int currentSubSprite = 0;
	}

}
