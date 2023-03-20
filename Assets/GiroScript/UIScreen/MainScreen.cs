using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HyperCasual.Core;
using Giro;
using UnityEngine.UI;

public class MainScreen : View
{
    [SerializeField]
    HyperCasualButton startButton;

    [SerializeField]
    AbstractGameEvent ContinueEvent;
    void OnEnable()
    {
        startButton.AddListener(OnStartButtonClicked);
    }
    private void OnDisable()
    {
        startButton.RemoveListener(OnStartButtonClicked);

    }
    void OnStartButtonClicked()
    {
        ContinueEvent.Raise();
    }

}
