using BepInEx;
using UnityEngine;

namespace BRCTools
{
    [BepInPlugin(pluginGuid, pluginName, pluginVersion)]
    public class Plugin : BaseUnityPlugin
    {
        public const string pluginGuid      = "speedrunning.brc.brctools";
        public const string pluginName      = "BRCTools";
        public const string pluginVersion   = "0.1.1";

        private GameObject _mod;

        private void Awake()
        {
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