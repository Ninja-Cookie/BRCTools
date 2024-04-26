using UnityEngine;
using static BRCTools.ToolsGUI;
using static BRCTools.ToolsConfig.KeyBinds;
using static BRCTools.ToolsRef;

namespace BRCTools
{
    internal class ToolsBindings : MonoBehaviour
    {
        public static bool acceptKeys = true;
        void Update()
        {
            if (acceptKeys)
            {
                if (Input.GetKeyDown(key_MenuToggle))
                    if (menuIsVisible && !menuInAnimation) { toolsGUI.MenuHide(); } else if (!menuIsVisible && !menuInAnimation) { toolsGUI.MenuShow(); }

                if (Input.GetKeyDown(key_MenuMouse))
                    toolsGUI.MenuToggleFocus();

                keysToCheck.Invoke();
            }
        }

        public delegate void KeysToCheck();
        public static KeysToCheck keysToCheck;
    }
}
