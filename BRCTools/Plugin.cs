using BepInEx;
using UnityEngine;

namespace BRCTools
{
    [BepInPlugin(pluginGuid, pluginName, pluginVersion)]
    public class Plugin : BaseUnityPlugin
    {
        public const string pluginGuid      = "speedrunning.brc.brctools";
        public const string pluginName      = "BRCTools";
        public const string pluginVersion   = "0.2.2";

        private GameObject _mod;

        private void Awake()
        {
            // This mod is super jank code-wise but works.
            // I want to re-make it again now that I know a little more but it is a lot.

            _mod = new GameObject();
            _mod.AddComponent<ToolsConfig>();
            _mod.AddComponent<ToolsGame>();
            _mod.AddComponent<ToolsFunctions>();
            _mod.AddComponent<ToolsMenu>();
            _mod.AddComponent<ToolsBindings>();
            _mod.AddComponent<ToolsFun>();
            _mod.AddComponent<ToolsPatcher>();
            _mod.AddComponent<ToolsRef>();
            DontDestroyOnLoad(_mod);
        }
    }
}