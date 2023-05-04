using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Doozy.Runtime.Signals;
using Giro;
using Doozy.Runtime.Nody;

public class MainScreen : MonoBehaviour
{
	public FlowController flow;
	public FlowNode firstEntryNode;
	public FlowNode levelSelectNode;
	public void CheckFirstEntry()
	{
		Debug.Log(SaveManager.FirstEntry);
		if (SaveManager.FirstEntry == true)
		{
			flow.SetActiveNode(firstEntryNode);
			SaveManager.FirstEntry = false;
		}
		else
		{
			flow.SetActiveNode(levelSelectNode);
		}
	}
}
