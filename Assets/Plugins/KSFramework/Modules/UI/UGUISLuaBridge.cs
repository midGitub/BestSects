﻿using UnityEngine;
using System.Collections;
using KEngine;
using KEngine.UI;
using UnityEngine.EventSystems;

namespace KSFramework
{
    public class UGUISLuaBridge : IUIBridge
    {
        public EventSystem EventSystem;

        public virtual void InitBridge()
        {
            if (EventSystem.current == null)
            {
                EventSystem = new GameObject("EventSystem").AddComponent<EventSystem>();
                EventSystem.gameObject.AddComponent<StandaloneInputModule>();
#if !UNITY_5
                EventSystem.gameObject.AddComponent<TouchInputModule>();
#else
                EventSystem.gameObject.GetComponent<StandaloneInputModule>().forceModuleActive = true;
#endif
            }
        }

        public virtual UIController CreateUIController(GameObject uiObj, string uiTemplateName)
        {
            //做修改，使c#的uicontroller也能使用，但是优先使用slua的
            UIController uiBase = uiObj.AddComponent<LuaUIController>();
            
            KEngine.Debuger.Assert(uiBase);
            return uiBase;
        }

        public virtual void UIObjectFilter(UIController controller, GameObject uiObject)
        {
        }

        public virtual IEnumerator LoadUIAsset(UILoadState loadState, UILoadRequest request)
        {
            string path = string.Format("ui/{0}.prefab", loadState.TemplateName);
            var assetLoader = StaticAssetLoader.Load(path);
            loadState.UIResourceLoader = assetLoader; // 基本不用手工释放的
            while (!assetLoader.IsCompleted)
                yield return null;

            request.Asset = assetLoader.TheAsset;
        }
    }

}
