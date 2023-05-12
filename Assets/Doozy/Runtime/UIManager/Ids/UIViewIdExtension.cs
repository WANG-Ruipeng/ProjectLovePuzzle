// Copyright (c) 2015 - 2021 Doozy Entertainment. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

//.........................
//.....Generated Class.....
//.........................
//.......Do not edit.......
//.........................

using System.Collections.Generic;
// ReSharper disable All
namespace Doozy.Runtime.UIManager.Containers
{
    public partial class UIView
    {
        public static IEnumerable<UIView> GetViews(UIViewId.Game id) => GetViews(nameof(UIViewId.Game), id.ToString());
        public static void Show(UIViewId.Game id, bool instant = false) => Show(nameof(UIViewId.Game), id.ToString(), instant);
        public static void Hide(UIViewId.Game id, bool instant = false) => Hide(nameof(UIViewId.Game), id.ToString(), instant);

        public static IEnumerable<UIView> GetViews(UIViewId.UIView id) => GetViews(nameof(UIViewId.UIView), id.ToString());
        public static void Show(UIViewId.UIView id, bool instant = false) => Show(nameof(UIViewId.UIView), id.ToString(), instant);
        public static void Hide(UIViewId.UIView id, bool instant = false) => Hide(nameof(UIViewId.UIView), id.ToString(), instant);
    }
}

namespace Doozy.Runtime.UIManager
{
    public partial class UIViewId
    {
        public enum Game
        {
            HUD,
            Pause
        }

        public enum UIView
        {
            HUD,
            LevelSelect,
            MainMenu,
            Manga1,
            Manga2,
            Manga3,
            Manga4,
            Pause,
            Setting,
            WIN
        }    
    }
}