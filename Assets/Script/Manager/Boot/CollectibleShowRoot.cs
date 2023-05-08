using Giro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleShowRoot : MonoBehaviour
{
	public CollectibleShow[] collectiblesShow;

	public void Reset()
	{
		for (int i = 0; i < collectiblesShow.Length; i++)
		{
			int id = collectiblesShow[i].id;
			CollectibleSaveInfo saveInfo = SaveManager.Instance.LoadCollectibleInfo(id);
			if (saveInfo.unlocked)
			{
				collectiblesShow[i].gameObject.SetActive(true);
			}
			else
			{
				collectiblesShow[i].gameObject.SetActive(false);
			}
		}
	}
}
