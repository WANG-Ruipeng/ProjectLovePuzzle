// Copyright (c) 2015 - 2021 Doozy Entertainment. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

//.........................
//.....Generated Class.....
//.........................
//.......Do not edit.......
//.........................

using UnityEngine;
// ReSharper disable All

namespace Doozy.Runtime.Signals
{
    public partial class Signal
    {
        public static bool Send(StreamId.UIView id, string message = "") => SignalsService.SendSignal(nameof(StreamId.UIView), id.ToString(), message);
        public static bool Send(StreamId.UIView id, GameObject signalSource, string message = "") => SignalsService.SendSignal(nameof(StreamId.UIView), id.ToString(), signalSource, message);
        public static bool Send(StreamId.UIView id, SignalProvider signalProvider, string message = "") => SignalsService.SendSignal(nameof(StreamId.UIView), id.ToString(), signalProvider, message);
        public static bool Send(StreamId.UIView id, Object signalSender, string message = "") => SignalsService.SendSignal(nameof(StreamId.UIView), id.ToString(), signalSender, message);
        public static bool Send<T>(StreamId.UIView id, T signalValue, string message = "") => SignalsService.SendSignal(nameof(StreamId.UIView), id.ToString(), signalValue, message);
        public static bool Send<T>(StreamId.UIView id, T signalValue, GameObject signalSource, string message = "") => SignalsService.SendSignal(nameof(StreamId.UIView), id.ToString(), signalValue, signalSource, message);
        public static bool Send<T>(StreamId.UIView id, T signalValue, SignalProvider signalProvider, string message = "") => SignalsService.SendSignal(nameof(StreamId.UIView), id.ToString(), signalValue, signalProvider, message);
        public static bool Send<T>(StreamId.UIView id, T signalValue, Object signalSender, string message = "") => SignalsService.SendSignal(nameof(StreamId.UIView), id.ToString(), signalValue, signalSender, message);   
    }

    public partial class StreamId
    {
        public enum UIView
        {
            LevelSelect,
            LoadLevelFromDef,
            MainMenu
        }         
    }
}
