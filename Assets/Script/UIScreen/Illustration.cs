using Doozy.Runtime.UIManager.Containers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Illustration : MonoBehaviour
{
	public UIContainer container;
	private void Awake()
	{
		container.Show();
	}
}
