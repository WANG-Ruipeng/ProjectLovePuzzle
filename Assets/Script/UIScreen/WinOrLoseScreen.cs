using HyperCasual.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Giro;
public class WinOrLoseScreen : View
{
    public HyperCasualButton restart;
    public HyperCasualButton backToSelectWindow;
    public AbstractGameEvent restartEvent;
    public AbstractGameEvent backToSelectWindowEvent;

    private void OnEnable()
    {
        restart.AddListener(OnRestartClicked);
        backToSelectWindow.AddListener(OnBackToSelectWindow);
    }
    private void OnDisable()
    {
        restart.RemoveListener(OnRestartClicked);
        backToSelectWindow.RemoveListener(OnBackToSelectWindow);
    }

    void OnRestartClicked()
    {
        restartEvent.Raise();
    }
    void OnBackToSelectWindow()
    {
        backToSelectWindowEvent.Raise();
    }

}
