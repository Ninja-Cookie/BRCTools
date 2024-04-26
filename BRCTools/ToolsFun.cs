using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using static BRCTools.ToolsGUI;

namespace BRCTools
{
    internal class ToolsFun : MonoBehaviour
    {
        private Color rainbowColor;

        private int currentIndex = 0;
        private Color[] colors = new Color[]
        {
        Color.red,
        Color.yellow,
        Color.green,
        Color.cyan,
        Color.blue,
        Color.magenta
        };

        private float timer = 0.0f;
        private float transitionInterval = 0.2f;

        private float updateTimer = 0.0f;
        private float updateInterval = 0.05f;

        Text nText;
        string pluginVersion = Plugin.pluginVersion;

        private void Start()
        {
            StartCoroutine(FindText());
        }

        private IEnumerator FindText()
        {
            while (nText == null)
            {
                if (stripText != null)
                    nText = stripText.GetComponent<Text>();

                yield return null;
            }
        }

        private void Update()
        {
            if (menuIsVisible && updateTimer >= updateInterval)
            {
                rainbowColor = Color.Lerp(colors[currentIndex], colors[(currentIndex + 1) % colors.Length], timer / transitionInterval);

                if (timer >= transitionInterval)
                {
                    currentIndex = (currentIndex + 1) % colors.Length;
                    rainbowColor = colors[currentIndex];
                    timer = 0.0f;
                }

                timer += Time.deltaTime;

                if (nText != null)
                    nText.text = $"BRCTools ({pluginVersion}) - by <color=#{ColorUtility.ToHtmlStringRGB(rainbowColor)}>Ninja Cookie</color>";

                updateTimer = 0.0f;
            }
            updateTimer += Time.deltaTime;
        }
    }
}
