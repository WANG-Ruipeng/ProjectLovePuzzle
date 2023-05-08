using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleShow : MonoBehaviour
{
	public int id;

	public void Show()
	{
		gameObject.SetActive(true);
	}

	public void Hide()
	{
		gameObject.SetActive(false);
	}

}
