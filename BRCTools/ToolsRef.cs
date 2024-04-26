using UnityEngine;

namespace BRCTools
{
    internal class ToolsRef : MonoBehaviour
    {
        public static ToolsRef Instance;
        public static ToolsGame toolsGame;
        public static ToolsFunctions toolsFunctions;
        public static ToolsConfig toolsConfig;
        public static ToolsGUI toolsGUI;

        public ToolsRef()
        {
            Instance = this;
            toolsGame = ToolsGame.Instance;
            toolsFunctions = ToolsFunctions.Instance;
            toolsConfig = ToolsConfig.Instance;
            toolsGUI = ToolsGUI.Instance;
        }
    }
}
