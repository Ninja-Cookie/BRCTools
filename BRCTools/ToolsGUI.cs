using Reptile;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static BRCTools.ToolsRef;

namespace BRCTools
{
    internal class ToolsGUI : MonoBehaviour
    {
        public static ToolsGUI Instance;

        public ToolsGUI()
        {
            Instance = this;
        }

        public void InitializeBase()
        {
            if (Properties.fonts.Count() > 0 && !Properties.fonts.Any(s => s == Properties.font.name))
                Properties.font = Font.CreateDynamicFontFromOSFont(Properties.fonts.Any(s => s == "Arial") ? "Arial" : Properties.fonts[0], Properties.fontSize);

            Debug.Log($"FONT LOADED: {Properties.font.name}");

            UpdateLinks();
            GUIUpdateScreenProps();
            GUICreateCanvas();
        }

        public void UpdateMenuSize()
        {
            float scale = 1f;
            if (Screen.height < Properties.menu_h)
            {
                scale = ((float)Screen.height / 1080);
                Debug.Log(scale);
                menu.transform.localScale = new Vector2(scale, scale);
            }
            else
            {
                menu.transform.localScale = new Vector2(1f, 1f);
            }

            menu.transform.position = new Vector2(menu.transform.position.x, ((Screen.height) - (((Screen.height) / 2) - ((Properties.menu_h * scale) / 2))) - Properties.shadowOffset);

            if (canvasObj != null)
            {
                canvasObj.RectTransform().sizeDelta = new Vector2(Screen.width, Screen.height);

                foreach (var obj in speedoObjects)
                    obj.RectTransform().sizeDelta = new Vector2(canvasObj.RectTransform().sizeDelta.x, canvasObj.RectTransform().sizeDelta.y * 0.35f);

                foreach (var obj in cordObjects)
                {
                    obj.RectTransform().sizeDelta = new Vector2(canvasObj.RectTransform().sizeDelta.x, canvasObj.RectTransform().sizeDelta.y);
                    obj.RectTransform().position = new Vector2(obj.RectTransform().position.x, (canvasObj.RectTransform().sizeDelta.y * 0.45f) - (obj.TryGetComponent(out Text text) && text.color == Color.black ? Properties.shadowOffset : 0f));
                }
            }
        }

        internal class Colors
        {
            public static Color background = new Color(0.2f, 0.2f, 0.3f, 0.7f);
            public static Color divider = new Color(0.2f, 0.2f, 0.3f, 0.6f);
            public static Color shadow = new Color(0f, 0f, 0f, 0.4f);
            public static Color strip = new Color(0.26f, 0.55f, 0.58f, 0.75f);
            public static Color field = new Color(0.13f, 0.19f, 0.26f, 0.75f);
            public static Color text = new Color(0.98f, 0.98f, 0.98f, 0.95f);
            public static Color textPlace = new Color(0.88f, 0.88f, 0.88f, 0.75f);
            public static Color textInac = new Color(0.6f, 0.6f, 0.6f, 0.95f);
            public static Color selector = new Color(0.1f, 0.1f, 0.1f, 0.95f);

            // Buttons

            public static Color buttonNormal = new Color(0.53f, 0.59f, 0.66f, 0.75f);
            public static Color keybind = new Color(buttonNormal.r * buttonNormal.grayscale, buttonNormal.g * buttonNormal.grayscale, buttonNormal.b * buttonNormal.grayscale, 0.8f);
            public static Color buttonToggleOff = new Color(0.69f, 0.40f, 0.40f, 0.75f);
            public static Color buttonToggleOn = new Color(0.40f, 0.69f, 0.40f, 0.75f);
            public static Color buttonInac = new Color(0.4f, 0.4f, 0.4f, 0.75f);

            public static ColorBlock button = new ColorBlock
            {
                // Normal Color
                normalColor = buttonNormal,

                // Hover Color
                highlightedColor = new Color(buttonNormal.r * 0.9f, buttonNormal.g * 0.9f, buttonNormal.b * 0.9f, buttonNormal.a),

                // Pressed Color
                pressedColor = new Color(buttonNormal.r * 0.8f, buttonNormal.g * 0.8f, buttonNormal.b * 0.8f, buttonNormal.a),

                // Disabled Color
                disabledColor = buttonInac,

                // Selection Color
                selectedColor = buttonNormal,

                // Alpha Multiplier
                colorMultiplier = 1f,

                // How Fast To Transition Colors
                fadeDuration = 0.05f,
            };

            public static ColorBlock buttonOn = new ColorBlock
            {
                // Normal Color
                normalColor = buttonToggleOn,

                // Hover Color
                highlightedColor = new Color(buttonToggleOn.r * 0.9f, buttonToggleOn.g * 0.9f, buttonToggleOn.b * 0.9f, buttonToggleOn.a),

                // Pressed Color
                pressedColor = new Color(buttonToggleOn.r * 0.8f, buttonToggleOn.g * 0.8f, buttonToggleOn.b * 0.8f, buttonToggleOn.a),

                // Disabled Color
                disabledColor = buttonInac,

                // Selection Color
                selectedColor = buttonToggleOn,

                // Alpha Multiplier
                colorMultiplier = 1f,

                // How Fast To Transition Colors
                fadeDuration = 0.05f,
            };

            public static ColorBlock buttonOff = new ColorBlock
            {
                // Normal Color
                normalColor = buttonToggleOff,

                // Hover Color
                highlightedColor = new Color(buttonToggleOff.r * 0.9f, buttonToggleOff.g * 0.9f, buttonToggleOff.b * 0.9f, buttonToggleOff.a),

                // Pressed Color
                pressedColor = new Color(buttonToggleOff.r * 0.8f, buttonToggleOff.g * 0.8f, buttonToggleOff.b * 0.8f, buttonToggleOff.a),

                // Disabled Color
                disabledColor = buttonInac,

                // Selection Color
                selectedColor = buttonToggleOff,

                // Alpha Multiplier
                colorMultiplier = 1f,

                // How Fast To Transition Colors
                fadeDuration = 0.05f,
            };
        }

        internal class Properties
        {
            public const int shadowOffset = 4;
            public const int menu_x = 20;
            public const int menu_y = 60;
            public const int menu_w = 250;
            public const int menu_h = 900;
            public static int menu_current_x = menu_x;
            public static int menu_current_y = menu_y;
            public const int menu_strip_h = 26;
            public const int menu_element_h = 16;//22;
            public static int line = 0;
            public static int current_screen_w = 1920;
            public static int current_screen_h = 1080;
            public const int margin_w = 14;
            public const int margin_h = 14;
            public const int spacingExtra = 4;
            public const int spacing = menu_element_h + spacingExtra;
            public const int spacingSmall = (int)(menu_element_h * 0.7f) + spacingExtra;
            public const int spacingHor = 1;
            public const int fontSize = 13;//15;
            public const int fontSizeScreen = 35;//15;
            public const int fontSizeKey = 11;//13;
            public static Font font = Font.CreateDynamicFontFromOSFont("Bahnschrift", fontSize);
            public static string[] fonts = Font.GetOSInstalledFontNames();
            public const float easeOvershoot = 1.2f;
            public const float animateSpeed = 0.25f;
        }

        public void GUIUpdateScreenProps()
        {
            if (Properties.current_screen_w != Screen.width)
                Properties.current_screen_w = Screen.width;

            if (Properties.current_screen_h != Screen.height)
                Properties.current_screen_h = Screen.height;
        }

        public void UpdateLinks()
        {
            if (menu != null)
            {
                var buttons = menu?.GetComponentsInChildren<Button>().Where(x => x.TryGetComponent(out ButtonInfo info) && info.property != null && info.isToggle);

                foreach (var button in buttons)
                {
                    ButtonInfo info = button.GetComponent<ButtonInfo>();
                    bool.TryParse(info.property?.GetValue(false)?.ToString(), out info.toggle);
                    button.colors = info.toggle ? Colors.buttonOn : Colors.buttonOff;
                }
            }
        }

        public void UpdateFields(string field, float value)
        {
            if (menu != null)
            {
                foreach (InputField input in menu?.GetComponentsInChildren<InputField>())
                {
                    InputFieldInfo info = input.GetComponent<InputFieldInfo>();
                    if (info != null && info.inputText != null && info.property.Name == field)
                    {
                        input.text = value.ToString();
                        info.lastValidInput = value.ToString();
                    }
                }
            }
        }

        public void UpdateSelectors(Func<int, string> func, string label)
        {
            if (menu != null)
            {
                foreach (Button buttonObj in menu?.GetComponentsInChildren<Button>())
                {
                    SelectorInfo selectorInfo = buttonObj.GetComponent<SelectorInfo>();
                    if (selectorInfo != null && selectorInfo.func.Method.Name == func.Method.Name)
                    {
                        Text labelText = selectorInfo.label.GetComponent<Text>();
                        if (selectorInfo.label != null && labelText != null && labelText.text != label)
                            labelText.text = label;

                        break;
                    }
                }
            }
        }

        private class ButtonInfo : MonoBehaviour
        {
            public bool isToggle = false;
            public bool toggle = false;
            public PropertyInfo property = null;
        }

        private class InputFieldInfo : MonoBehaviour
        {
            public PropertyInfo property = null;
            public Text inputText = null;
            public string lastValidInput = "";
        }

        private class SelectorInfo : MonoBehaviour
        {
            public Func<int, string> func = null;
            public GameObject label = null;
        }

        public string RemoveTextBetweenDelimiters(string inputString, char startDelimiter, char endDelimiter)
        {
            string pattern = $"\\{startDelimiter}.*?\\{endDelimiter}";
            return Regex.Replace(inputString, pattern, string.Empty);
        }

        public static GameObject canvas;
        public static GameObject menu;
        public static GameObject stripText;
        public static Canvas canvasObj;

        public static List<GameObject> menu1Objects = new List<GameObject>();
        public static List<GameObject> menu2Objects = new List<GameObject>();
        public static List<GameObject> speedoObjects = new List<GameObject>();
        public static List<GameObject> cordObjects = new List<GameObject>();
        public static List<GameObject> menuObjects  = menu1Objects;
        private void GUICreateCanvas()
        {
            canvas = new GameObject("Canvas", typeof(Canvas), typeof(GraphicRaycaster));
            Canvas canvasComp = canvas.GetComponent<Canvas>();
            canvasComp.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasComp.sortingOrder = 90000;

            canvasObj = canvasComp;
        }

        public void GUICreateBox(Vector2 pos, Vector2 size, Color color, float xOffset = 0f, float yOffset = 1f)
        {
            bool isMenu = menu == null;

            // Create Box
            GameObject squareObj = new GameObject("Square", typeof(Image));
            Image square = squareObj.GetComponent<Image>();
            RectTransform rectTransform = squareObj.GetComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(xOffset, yOffset);
            rectTransform.anchorMax = new Vector2(xOffset, yOffset);
            rectTransform.pivot = new Vector2(xOffset, yOffset);
            squareObj.transform.SetParent(!isMenu ? menu.transform : canvas.transform);
            square.raycastTarget = false;

            // Set Color
            square.color = color;

            // Set Properties
            rectTransform.position = pos;
            rectTransform.sizeDelta = size;

            if (isMenu)
            {
                squareObj.AddComponent<CanvasGroup>();
                menu = squareObj;
            }
            else
            {
                menuObjects.Add(squareObj);
            }
        }

        public GameObject GUICreateCursor(Vector2 pos, Vector2 size, Color color)
        {
            // Create Cursor
            GameObject      squareObj       = new GameObject("Cursor", typeof(Image));
            Image           square          = squareObj.GetComponent<Image>();
            RectTransform   rectTransform   = squareObj.GetComponent<RectTransform>();

            rectTransform.anchorMin         = new Vector2(0.5f, 0.5f);
            rectTransform.anchorMax         = new Vector2(0.5f, 0.5f);
            rectTransform.pivot             = new Vector2(0.5f, 0.5f);

            squareObj.transform.SetParent(menu.transform);

            square.raycastTarget = false;

            // Set Color
            square.color = color;

            // Set Properties
            rectTransform.position = pos;
            rectTransform.sizeDelta = size;

            //menuObjects.Add(squareObj);

            return squareObj;
        }

        public void GUICreateInputField(Vector2 pos, Vector2 size, string placeholder, PropertyInfo controlFloat, float xOffset = 0f, float yOffset = 1f)
        {
            // Create Input Field
            GameObject  inputFieldObj   = new GameObject("Input", typeof(InputField), typeof(Image), typeof(InputFieldInfo), typeof(EventTrigger));
            InputField  input           = inputFieldObj.GetComponent<InputField>();
            Image       image           = inputFieldObj.GetComponent<Image>();
            EventTrigger eventTrigger   = inputFieldObj.GetComponent<EventTrigger>();
            InputFieldInfo fieldInfo    = inputFieldObj.GetComponent<InputFieldInfo>();

            RectTransform rectTransform = inputFieldObj.GetComponent<RectTransform>();
            rectTransform.anchorMin     = new Vector2(xOffset, yOffset);
            rectTransform.anchorMax     = new Vector2(xOffset, yOffset);
            rectTransform.pivot         = new Vector2(xOffset, yOffset);
            inputFieldObj.transform.SetParent(menu.transform);

            rectTransform.position = pos;
            rectTransform.sizeDelta = size;

            image.color = Colors.field;

            // Create Text
            GameObject inputTextObj = GUICreateText(pos, size, Colors.text, "", Mathf.RoundToInt(Properties.fontSize * 0.8f));
            Text inputText = inputTextObj.GetComponent<Text>();
            inputTextObj.transform.SetParent(inputFieldObj.transform);

            GameObject placeholderTextObj = GUICreateText(pos, size, Colors.textPlace, "", Mathf.RoundToInt(Properties.fontSize * 0.8f));
            Text placeholderText = placeholderTextObj.GetComponent<Text>();

            Graphic placeholderGraphic = placeholderTextObj.GetComponent<Graphic>();
            placeholderTextObj.transform.SetParent(placeholderTextObj.transform);

            // Set Properties
            input.targetGraphic = image;
            input.textComponent = inputText;
            input.placeholder = placeholderGraphic;
            placeholderText.text = placeholder;

            input.characterLimit = 16;

            // Events

            if (fieldInfo != null)
                fieldInfo.lastValidInput = inputText.text;

            if (controlFloat != null)
            {
                fieldInfo.property  = controlFloat;
                fieldInfo.inputText = inputText;
            }

            bool ending = false;
            EventTrigger.Entry event_Select = new EventTrigger.Entry() { eventID = EventTriggerType.UpdateSelected };
            event_Select.callback.AddListener((data) => { ToolsBindings.acceptKeys = ending; if (ending) { input.interactable = false; input.interactable = true; ending = false; } });

            eventTrigger.triggers = new List<EventTrigger.Entry>()
            {
                event_Select
            };

            input.onEndEdit.AddListener(SubmitChange);
            void SubmitChange(string value)
            {
                ToolsBindings.acceptKeys = true; ending = true;

                bool passInt = false;

                if (controlFloat != null)
                    passInt = controlFloat.PropertyType == typeof(int);

                string  finalText       = "";
                float   finalValue      = 0f;
                float   finalValueInt   = 0;
                bool    valid           = !passInt ? float.TryParse(value, out finalValue) : float.TryParse(value, out finalValueInt);

                if (valid)
                {
                    int valueInt        = Mathf.RoundToInt(finalValueInt);
                    float valueToPass   = !passInt ? finalValue : valueInt;

                    if (controlFloat != null)
                    {
                        if (!passInt)
                            controlFloat.SetValue(false, valueToPass);
                        else
                            controlFloat.SetValue(false, valueInt);
                    }

                    if (fieldInfo != null && controlFloat != null)
                        fieldInfo.lastValidInput = controlFloat.GetValue(false).ToString();

                    if (controlFloat != null)
                        finalText = controlFloat.GetValue(false).ToString();
                }
                else if (fieldInfo != null)
                {
                    finalText = fieldInfo.lastValidInput;
                }

                input.text = finalText;
            }

            menuObjects.Add(inputFieldObj);
        }

        public GameObject GUICreateText(Vector2 pos, Vector2 size, Color color, string text, int fontSize, float xOffset = 0f, float yOffset = 1f, bool toMenu = true, TextAnchor anchor = TextAnchor.MiddleCenter)
        {
            GameObject      textObject      = new GameObject("Text", typeof(Text));
            Text            textComp        = textObject.GetComponent<Text>();
            RectTransform   textTransform   = textObject.GetComponent<RectTransform>();
            textObject.transform.SetParent(toMenu ? menu.transform : canvas.transform);

            textComp.font                       = Properties.font;
            textComp.fontSize                   = fontSize;
            textComp.text                       = text;
            textComp.color                      = color;
            textTransform.anchorMin             = new Vector2(xOffset, yOffset);
            textTransform.anchorMax             = new Vector2(xOffset, yOffset);
            textTransform.pivot                 = new Vector2(xOffset, yOffset);
            textComp.alignment                  = anchor;
            textComp.raycastTarget              = false;

            textComp.rectTransform.position     = pos;
            textComp.rectTransform.sizeDelta    = size;

            if (stripText == null && color != Colors.shadow && text.Contains("Ninja Cookie")) { stripText = textObject; }

            menuObjects.Add(textObject);

            return textObject;
        }

        public void GUICreateButton(Vector2 pos, Vector2 size, ColorBlock colors, UnityAction action, PropertyInfo field = null, float xOffset = 0f, float yOffset = 1f)
        {
            // Create Button
            GameObject      buttonObject    = new GameObject("Button", typeof(Image), typeof(Button), typeof(EventTrigger), typeof(ButtonInfo));
            Button          button          = buttonObject.GetComponent<Button>();
            EventTrigger    eventTrigger    = buttonObject.GetComponent<EventTrigger>();
            RectTransform   transform       = buttonObject.GetComponent<RectTransform>();
            ButtonInfo      buttonInfo      = buttonObject.GetComponent<ButtonInfo>();
            buttonObject.transform.SetParent(menu.transform);

            transform.anchorMin = new Vector2(xOffset, yOffset);
            transform.anchorMax = new Vector2(xOffset, yOffset);
            transform.pivot     = new Vector2(xOffset, yOffset);

            if (field != null)
            {
                bool toggleState = false;
                if (bool.TryParse(field?.GetValue(false)?.ToString(), out toggleState))
                {
                    buttonInfo.isToggle = field != null;
                    buttonInfo.toggle = toggleState;
                    buttonInfo.property = field;
                }
            }

            // Params
            transform.position  = pos;
            transform.sizeDelta = size;
            button.colors       = colors;

            // Events
            EventTrigger.Entry event_buttonSelect = new EventTrigger.Entry() { eventID = EventTriggerType.UpdateSelected };
            event_buttonSelect.callback.AddListener((data) => { RefreshInteraction(); });

            eventTrigger.triggers = new List<EventTrigger.Entry>()
            {
                event_buttonSelect
            };

            button.onClick.AddListener(Click);
            void Click() { action.Invoke(); }

            void RefreshInteraction() { if (button.interactable) { button.interactable = false; button.interactable = true; } }

            menuObjects.Add(buttonObject);
        }

        public void GUICreateSelector(Vector2 pos, Vector2 size, ColorBlock colors, Func<int, string> func, GameObject label, int mod, string text, float xOffset = 0f, float yOffset = 1f)
        {
            // Create Button
            GameObject      buttonObject    = new GameObject("ButtonSelector", typeof(Image), typeof(Button), typeof(EventTrigger), typeof(SelectorInfo));
            Button          button          = buttonObject.GetComponent<Button>();
            EventTrigger    eventTrigger    = buttonObject.GetComponent<EventTrigger>();
            RectTransform   transform       = buttonObject.GetComponent<RectTransform>();
            SelectorInfo    selectorInfo    = buttonObject.GetComponent<SelectorInfo>();
            buttonObject.transform.SetParent(menu.transform);

            if (selectorInfo != null )
            {
                selectorInfo.func   = func;
                selectorInfo.label  = label;
            }

            transform.anchorMin = new Vector2(xOffset, yOffset);
            transform.anchorMax = new Vector2(xOffset, yOffset);
            transform.pivot     = new Vector2(xOffset, yOffset);

            // Params
            transform.position  = pos;
            transform.sizeDelta = size;
            button.colors       = colors;

            // Events
            EventTrigger.Entry event_buttonSelect = new EventTrigger.Entry() { eventID = EventTriggerType.UpdateSelected };
            event_buttonSelect.callback.AddListener((data) => { RefreshInteraction(); });

            eventTrigger.triggers = new List<EventTrigger.Entry>()
            {
                event_buttonSelect
            };

            button.onClick.AddListener(Click);
            void Click() { label.GetComponent<Text>().text = func(mod); }

            void RefreshInteraction() { if (button.interactable) { button.interactable = false; button.interactable = true; } }

            menuObjects.Add(buttonObject);
        }

        public Vector2 Pos(int x, int y)
        {
            return new Vector2(x, Properties.current_screen_h - y);
        }

        public Vector2 Size(int w, int h)
        {
            return new Vector2(w, h);
        }

        public bool menuBlocked = false;
        public void MenuBlock(bool block = true, bool changeAlpha = true)
        {
            menuBlocked = block;

            if (menuStatus != null)
                menuStatus.text = block ? $"Mouse Input Disabled ({toolsConfig.GetKeyString(ToolsConfig.KeyBinds.key_MenuMouse)})" : "";

            foreach(Image img in menu?.GetComponentsInChildren<Image>())
            {
                if (img.gameObject.name != "Square")
                {
                    if (block)
                        img.raycastTarget = false;
                    else
                        img.raycastTarget = true;
                }
            }

            if (changeAlpha)
                MenuAlphaControl(block);
        }

        private void MenuAlphaControl(bool lessen)
        {
            CanvasGroup group = menu != null ? menu.GetComponent<CanvasGroup>() : null;
            if (lessen && group != null)
                group.alpha = 0.85f;
            else if (!lessen && group != null)
                group.alpha = 1f;
        }

        public Text menuStatus;
        public void MenuShow()
        {
            if (!menuInAnimation && !menuIsVisible)
                StartCoroutine(MenuAnimate(MenuAnimations.ShowMenu, -(Properties.menu_x + Properties.menu_w), Properties.menu_x, Properties.animateSpeed));
        }

        public void MenuHide()
        {
            if (!menuInAnimation && menuIsVisible)
            {
                MenuToggleFocus(false);
                StartCoroutine(MenuAnimate(MenuAnimations.HideMenu, Properties.menu_x, -(Properties.menu_x + Properties.menu_w), Properties.animateSpeed));
            }
        }

        public void MenuToggleFocus(bool shouldFocus = true)
        {
            if (!menuIsVisible || menuInAnimation)
                return;

            if (shouldFocus && menuBlocked)
            {
                MenuBlock(false);
                Core.OnAlwaysUpdate += ForceCursorActive;
            }
            else
            {
                MenuBlock(true, shouldFocus);

                BaseModule baseMod = ToolsGame.Game.GetBaseModule();
                if (baseMod != null)
                    baseMod.RestoreMouseInputState();

                ToolsGame.Game.GetGameInput()?.EnableMouse();
                Core.OnAlwaysUpdate -= ForceCursorActive;
            }
        }

        private void ForceCursorActive()
        {
            Cursor.visible = true;
            ToolsGame.Game.GetGameInput()?.DisableMouse();
        }

        public static bool menuIsVisible = false;
        public static bool menuIsClosing = false;
        public static bool menuInAnimation = false;
        private IEnumerator MenuAnimate(MenuAnimations animation, float start, float end, float duration)
        {
            if (animation == MenuAnimations.ShowMenu) { menu.gameObject.SetActive(true); menuIsVisible = true; MenuToggleFocus(ToolsConfig.Settings.shouldFocusMenuOnShow); }
            menuInAnimation = true;

            if (animation == MenuAnimations.HideMenu)
                menuIsClosing = true;

            float elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                float easedValue = animation == MenuAnimations.ShowMenu ? EaseOutBack(elapsedTime, start, end - start, duration, Properties.easeOvershoot) : EaseInBack(elapsedTime, start, end - start, duration, Properties.easeOvershoot);
                MoveMenu(Mathf.RoundToInt(easedValue));

                yield return null;
                elapsedTime += Time.deltaTime;
            }
            MoveMenu(Mathf.RoundToInt(end));

            if (animation == MenuAnimations.HideMenu) { MenuAlphaControl(true); menu.gameObject.SetActive(false); menuIsVisible = false; }
            menuInAnimation = false;
            menuIsClosing = false;
        }

        private float EaseOutBack(float t, float b, float c, float d, float s = 1.70158f)
        {
            t = t / d - 1f;
            return c * (t * t * ((s + 1f) * t + s) + 1f) + b;
        }

        private float EaseInBack(float t, float b, float c, float d, float s = 1.70158f)
        {
            t = t / d;
            return c * t * t * ((s + 1f) * t - s) + b;
        }

        private void MoveMenu(int x)
        {
            if (menu != null)
            {
                menu.transform.position = new Vector2(x, menu.transform.position.y);
                Properties.menu_current_x = x;
            }
        }

        private enum MenuAnimations
        {
            ShowMenu,
            HideMenu
        }
    }
}
