using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Doozy.Runtime.Signals;
using Giro;

public class MainScreen : MonoBehaviour, ISignalProvider
{
	public void CheckFirstEntry()
	{
		Debug.Log(SaveManager.FirstEntry);
		if (SaveManager.FirstEntry == true)
		{
			SendSignal();
		}
	}

	#region Signal
	public ProviderAttributes attributes
	{
		get
		{
			ProviderAttributes ret = new ProviderAttributes(ProviderType.Global, "MainMenuSignal", "FirstEntry", null);
			return ret;
		}
	}

	public SignalStream stream
	{
		get
		{
			SignalStream ret = new SignalStream(SignalsService.GetNewStreamKey());
			return ret;
		}
	}
	public bool isConnected
	{
		get => true;
	}
	public void OpenStream()
	{
	}
	public void CloseStream()
	{

	}
	public bool SendSignal()
	{
		Debug.Log("QWQ");
		return true;
	}
	public bool SendSignal<T>(T signalValue)
	{
		Debug.Log("QWQ");
		return true;
	}


	#endregion
}
