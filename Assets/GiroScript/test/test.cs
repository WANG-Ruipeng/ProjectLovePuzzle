using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HyperCasual.Core;
public class test : MonoBehaviour
{
    Button button;
    public AbstractGameEvent pause;
    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        pause.Raise();
        Debug.Log("P");
    }
}
