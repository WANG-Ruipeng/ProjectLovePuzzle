using Doozy.Runtime.UIManager.Components;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PagesScreen : MonoBehaviour
{
	public int id;
	public SpriteRenderer spriteRenderer;
	public Sprite[] sprites;
	public int currentPage = 0;
	public Action<int> finalHandle;

	public void ShowNextPage()
	{
		if (currentPage == 0)
		{

		}
		if (currentPage == sprites.Length)//最后一个的时候触发
		{
			spriteRenderer.sprite = null;
			finalHandle?.Invoke(id);
			return;
		}
		spriteRenderer.sprite = sprites[currentPage++];
	}

}
