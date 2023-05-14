using Doozy.Runtime.UIManager.Components;
using Doozy.Runtime.UIManager.Containers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PagesScreen : MonoBehaviour
{

	public UISelectable selectable;
	public Image spriteRenderer;
	public SpriteSet[] spriteset;
	public int currentPage = 0;
	public UIView illustration;

	public Action finalHandle;
	List<Image> subSpriteRenders = new List<Image>();
	public void Awake()
	{
		selectable = GetComponent<UISelectable>();
		selectable.interactable = false;
		GetEmptySpriteRender();
		for (int i = 0; i < spriteset.Length; i++)
		{
			spriteset[i].currentSubSprite = -1;
		}
		spriteRenderer.maskable = false;
		gameObject.SetActive(false);
	}
	public void ShowNextPage()
	{
		if (currentPage == spriteset.Length)//最后一个的时候触发
		{
			spriteRenderer.sprite = null;
			spriteRenderer.color = Color.black;
			selectable.interactable = false;
			finalHandle?.Invoke();
			currentPage = 0;
			illustration.Hide();
			gameObject.SetActive(false);
			return;
		}
		spriteRenderer.sprite = spriteset[currentPage].sprite;
		spriteRenderer.type = Image.Type.Simple;
		if (currentPage == 0)
		{
			illustration.Show();
			spriteRenderer.color = Color.white;
			gameObject.SetActive(true);
			selectable.interactable = true;
		}

		var subSprite = spriteset[currentPage].subSprite;
		if (spriteset[currentPage].currentSubSprite < subSprite.Length)
		{
			if (spriteset[currentPage].currentSubSprite == -1)
			{
				spriteset[currentPage].currentSubSprite++;
				return;
			}
			var sr = GetEmptySpriteRender();
			sr.sprite = subSprite[spriteset[currentPage].currentSubSprite];
			spriteRenderer.type = Image.Type.Simple;
			SetAlpha(sr, 1);
			spriteset[currentPage].currentSubSprite++;
		}
		else
		{
			spriteset[currentPage].currentSubSprite = -1;
			for (int i = 0; i < subSpriteRenders.Count; i++)
			{
				subSpriteRenders[i].sprite = null;
				ResetAlpha(subSpriteRenders[i]);
			}
			currentPage++;
			ShowNextPage();//再次调用加载下一页
		}
	}

	Image GetEmptySpriteRender()
	{
		int i;
		for (i = 0; i < subSpriteRenders.Count; i++)
			if (subSpriteRenders[i].sprite == null)
				return subSpriteRenders[i];
		GameObject child = new GameObject("SubSprite" + i);

		child.transform.parent = spriteRenderer.transform;
		child.transform.localScale = new Vector3(1, 1, 1);
		var sr = child.AddComponent<Image>();
		var c = sr.color;
		ResetAlpha(sr);
		sr.maskable = false;
		subSpriteRenders.Add(sr);
		return sr;
	}

	void ResetAlpha(Image sr)
	{
		var c = sr.color;
		c.a = 0;
		sr.color = c;
	}
	void SetAlpha(Image sr, float a)
	{
		var c = sr.color;
		c.a = a;
		sr.color = c;
	}

	[Serializable]
	public class SpriteSet
	{
		public Sprite sprite;
		public Sprite[] subSprite;
		[HideInInspector]
		public int currentSubSprite;
	}

}
