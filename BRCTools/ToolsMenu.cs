using Reptile;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static BRCTools.ToolsConfig;
using static BRCTools.ToolsFunctions;
using static BRCTools.ToolsRef;

namespace BRCTools
{
    internal class ToolsMenu : ToolsGUI
    {
        void Start()
        {
            InitializeBase();
            CreateMenu();
            MenuBlock(true);

            if (Settings.shouldMenuStartShown)
            {
                MenuShow();
            }
            else
            {
                menuIsVisible = true; MenuHide();
            }

            toolsFunctions.UpdateMenuInfo();
            UpdateMenuSize();

            foreach (GameObject mo in menu2Objects)
            {
                if (mo.TryGetComponent(out Image img))
                    img.raycastTarget = true;

                mo.SetActive(false);
            }
        }

        private GameObject cursor;
        private void CreateMenu()
        {
            // Create base menu
            CreateBox(Properties.menu_current_x, Properties.menu_current_y = (Properties.current_screen_h / 2) - (Properties.menu_h / 2), Properties.menu_w, Properties.menu_h, Colors.background);
            CreateBox(Properties.menu_current_x, Properties.menu_current_y, Properties.menu_w, Properties.menu_strip_h, Colors.strip, false);
            CreateText(Properties.menu_current_x, Properties.menu_current_y, Properties.menu_w, Properties.menu_strip_h, $"BRCTools ({Plugin.pluginVersion}) - by Ninja Cookie");
            CreateText(Properties.menu_current_x, Properties.menu_current_y + Properties.menu_strip_h, Properties.menu_w, Properties.margin_h, $"+ Help From Bytez", Mathf.RoundToInt(Properties.fontSize * 0.75f), textColor: Color.yellow);
            toolsGUI.menuStatus = CreateText(Properties.menu_current_x, Properties.menu_current_y - Properties.margin_h, Properties.menu_w, Properties.margin_h, "", Properties.fontSize, false, textColor: Color.red).GetComponent<Text>();
            toolsFunctions.errorMessage = CreateText(0, 0, 1200, 600, "ERROR", Properties.fontSize * 2, false, toMenu: false, anchor: TextAnchor.UpperLeft, textColor: Color.red).GetComponent<Text>();
            toolsFunctions.errorMessage.gameObject.SetActive(false);
            menuObjects.Clear();

            // Elements
            Properties.line = Properties.menu_current_y + Properties.menu_strip_h + Properties.margin_h;
            int elementBaseX = Properties.menu_current_x + Properties.margin_w;
            int elementBaseW = Properties.menu_w - (Properties.margin_w * 2);

            // ------------------------------------------------------------------------

            // --- FILL BOOST ---
            // ------------------------------------------------------------------------
            CreateButton(elementBaseX, Properties.line, elementBaseW, Properties.menu_element_h, toolsFunctions.FuncRefill, "Fill Boost", KeyBinds.key_Refill);
            // ------------------------------------------------------------------------

            CreateLine();

            // --- INF BOOST ---
            // ------------------------------------------------------------------------
            CreateButton(elementBaseX, Properties.line, elementBaseW, Properties.menu_element_h, toolsFunctions.FuncInfBoost, "Infinite Boost", KeyBinds.key_InfBoost, nameof(Toggles.infBoost));
            // ------------------------------------------------------------------------

            CreateDivider();

            // --- END WANTED ---
            // ------------------------------------------------------------------------
            CreateButton(elementBaseX, Properties.line, elementBaseW, Properties.menu_element_h, toolsFunctions.FuncEndWanted, "End Wanted", KeyBinds.key_EndWanted);
            // ------------------------------------------------------------------------

            CreateLine();

            // --- DISABLE POLICE ---
            // ------------------------------------------------------------------------
            CreateButton(elementBaseX, Properties.line, elementBaseW, Properties.menu_element_h, toolsFunctions.FuncPolice, "Disable Wanted", KeyBinds.key_DisPolice, nameof(Toggles.police));
            // ------------------------------------------------------------------------

            CreateDivider();

            // --- DISABLE POLICE ---
            // ------------------------------------------------------------------------
            CreateButton(elementBaseX, Properties.line, elementBaseW, Properties.menu_element_h, toolsFunctions.FuncRefillHP, "Refill Health", KeyBinds.key_refillHP);
            // ------------------------------------------------------------------------

            CreateLine();

            // --- INVULN ---
            // ------------------------------------------------------------------------
            CreateButton(elementBaseX, Properties.line, elementBaseW, Properties.menu_element_h, toolsFunctions.FuncInvulnerable, "Invulnerable", KeyBinds.key_Invulnerable, nameof(Toggles.invulnerable));
            // ------------------------------------------------------------------------

            CreateDivider();

            // --- SAVE / LOAD PROPERTIES ---
            // ------------------------------------------------------------------------
            string[][] saveStrings = new string[][] {
                new string[] { "X Pos ...", nameof(Attributes.savedPosX), "Saved X:" },
                new string[] { "Y Pos ...", nameof(Attributes.savedPosY), "Saved Y:" },
                new string[] { "Z Pos ...", nameof(Attributes.savedPosZ), "Saved Z:" }
            };
            CreateTextFieldMulti(elementBaseX, Properties.line, elementBaseW, Properties.menu_element_h, info: saveStrings);

            CreateLine();

            string[][] propStrings = new string[][] {
                new string[] { "Saved Speed ...", nameof(Attributes.savedSpeed), "Saved Speed:" },
                new string[] { "Boost ...", nameof(Attributes.savedBoost), "Saved Boost:" }
            };
            CreateTextFieldMulti(elementBaseX, Properties.line, elementBaseW, Properties.menu_element_h, info: propStrings);

            CreateLine();

            CreateTextField(elementBaseX, Properties.line, elementBaseW - (elementBaseW / 5) - 1, Properties.menu_element_h, "Saved Storage ...", nameof(Attributes.savedStorage), "Saved Storage:");
            CreateButton(elementBaseX + (elementBaseW - (elementBaseW / 5)), Properties.line, elementBaseW / 5, Properties.menu_element_h, toolsFunctions.FuncSetStorage, "Set");

            CreateMidLine();

            CreateButton(elementBaseX, Properties.line, elementBaseW, Properties.menu_element_h, toolsFunctions.FuncSave, "Save Position", KeyBinds.key_Save);

            CreateLine();

            CreateButton(elementBaseX, Properties.line, elementBaseW, Properties.menu_element_h, toolsFunctions.FuncLoad, "Load Position", KeyBinds.key_Load);
            // ------------------------------------------------------------------------

            CreateDivider();

            // --- CHAR ---
            // ------------------------------------------------------------------------

            // --- CHAR CHANGE ---
            CreateSelector(elementBaseX, Properties.line, elementBaseW, Properties.menu_element_h, toolsFunctions.FuncChangePlayer, toolsFunctions.CharToName(Attributes.charIndex), KeyBinds.key_CharPrev, KeyBinds.key_CharNext, "Character:");

            CreateLine();

            // --- STYLE CHANGE ---
            CreateSelector(elementBaseX, Properties.line, elementBaseW, Properties.menu_element_h, toolsFunctions.FuncChangeStyle, toolsFunctions.StyleToName(Attributes.styleIndex), KeyBinds.key_StylePrev, KeyBinds.key_StyleNext, "Style:");

            CreateLine();

            // --- OUTFIT CHANGE ---
            CreateSelector(elementBaseX, Properties.line, elementBaseW, Properties.menu_element_h, toolsFunctions.FuncChangeOutfit, toolsFunctions.OutfitToName(Attributes.outfitIndex), KeyBinds.key_OutfitPrev, KeyBinds.key_OutfitNext, "Outfit:");

            // ------------------------------------------------------------------------

            CreateDivider();

            // --- LEVEL CHANGE ---
            // ------------------------------------------------------------------------
            CreateSelector(elementBaseX, Properties.line, elementBaseW, Properties.menu_element_h, toolsFunctions.FuncChangeLevel, toolsFunctions.LevelToName(Attributes.levelIndex), KeyBinds.key_LevelPrev, KeyBinds.key_LevelNext, "Selected Level:");

            CreateLineNoExtra();

            CreateButton(elementBaseX, Properties.line, elementBaseW, Properties.menu_element_h, toolsFunctions.FuncGoToLevel, "Go To Level", KeyBinds.key_GoToLevel);
            // ------------------------------------------------------------------------

            CreateLine();

            // --- CHAPTER CHANGE ---
            // ------------------------------------------------------------------------
            CreateSelector(elementBaseX, Properties.line, elementBaseW, Properties.menu_element_h, toolsFunctions.FuncChangeSave, toolsFunctions.SaveToName(Attributes.saveIndex), keybindDec: KeyBinds.key_prevSave, keybindInc: KeyBinds.key_nextSave, title: "Selected Save:");

            CreateLineNoExtra();

            CreateButton(elementBaseX, Properties.line, elementBaseW, Properties.menu_element_h, toolsFunctions.FuncLoadSaveFile, "Overwrite Slot With This Save", KeyBinds.key_goToSave);

            CreateLine();

            CreateButton(elementBaseX, Properties.line, elementBaseW, Properties.menu_element_h, toolsFunctions.FuncCreateSaveFile, "Create Save Of Here", KeyBinds.key_saveGame);
            // ------------------------------------------------------------------------

            CreateDivider();

            // --- DISABLE SAVING ---
            // ------------------------------------------------------------------------
            CreateButton(elementBaseX, Properties.line, elementBaseW, Properties.menu_element_h, toolsFunctions.FuncDisSaving, "Disable Auto-Saving To Slot", KeyBinds.key_DisSaving, nameof(Toggles.saving));
            // ------------------------------------------------------------------------

            CreateLine();

            // --- LOAD SAVE ---
            // ------------------------------------------------------------------------
            CreateButton(elementBaseX, Properties.line, elementBaseW, Properties.menu_element_h, toolsFunctions.FuncResGame, "Load Current Slot Data", KeyBinds.key_ResGame);
            // ------------------------------------------------------------------------

            CreateDivider();

            // --- RESET TO ENTRANCE ---
            // ------------------------------------------------------------------------
            CreateButton(elementBaseX, Properties.line, elementBaseW, Properties.menu_element_h, toolsFunctions.FuncResLevel, "Reset To Entrance", KeyBinds.key_ResLevel);
            // ------------------------------------------------------------------------

            CreateDivider();

            // --- FRAME / SCALE CHANGE ---
            // ------------------------------------------------------------------------
            string[][] timeStrings = new string[][] {
                new string[] { "Framerate ...", nameof(Attributes.framerate), "Framerate:" },
                new string[] { "Timescale ...", nameof(Attributes.timescale), "Timescale:" }
            };
            CreateTextFieldMulti(elementBaseX, Properties.line, elementBaseW, Properties.menu_element_h, info: timeStrings);

            CreateLineNoExtra();

            int ftButtons = Mathf.RoundToInt(elementBaseW / 2) - Properties.spacingHor;
            CreateButton(elementBaseX, Properties.line, ftButtons, Properties.menu_element_h, toolsFunctions.FuncLockFPS, "Lock FPS", KeyBinds.key_LockFPS, nameof(Toggles.tfps));
            CreateButton(elementBaseX + ftButtons + (Properties.spacingHor * 2), Properties.line, ftButtons, Properties.menu_element_h, toolsFunctions.FuncSetTimescale, "Set Scale", KeyBinds.key_SetTimescale, nameof(Toggles.timescale));
            // ------------------------------------------------------------------------

            CreateLine();

            // --- FRAME / SCALE CHANGE ---
            // ------------------------------------------------------------------------
            string[][] flyNocStrings = new string[][] {
                new string[] { "Speed ...", nameof(Attributes.noclip), "Noclip Speed:" },
                new string[] { "Speed ...", nameof(Attributes.fly), "Fly Speed:" }
            };
            CreateTextFieldMulti(elementBaseX, Properties.line, elementBaseW, Properties.menu_element_h, info: flyNocStrings);

            CreateLineNoExtra();

            CreateButton(elementBaseX, Properties.line, ftButtons, Properties.menu_element_h, toolsFunctions.FuncNoclip, "Noclip", KeyBinds.key_Noclip, nameof(Toggles.noclip));
            CreateButton(elementBaseX + ftButtons + (Properties.spacingHor * 2), Properties.line, ftButtons, Properties.menu_element_h, toolsFunctions.FuncFly, "Fly", KeyBinds.key_Fly, nameof(Toggles.fly));
            // ------------------------------------------------------------------------

            CreateDivider();

            // --- RESET GRAFFITI ---
            // ------------------------------------------------------------------------
            CreateButton(elementBaseX, Properties.line, elementBaseW, Properties.menu_element_h, toolsFunctions.FuncResetGraf, "Reset Graffiti", KeyBinds.key_ResGraf);
            // ------------------------------------------------------------------------

            CreateLine();

            // --- AUTO GRAFFITI ---
            // ------------------------------------------------------------------------
            CreateButton(elementBaseX, Properties.line, elementBaseW, Properties.menu_element_h, toolsFunctions.FuncAutoGraf, "Auto Graffiti", KeyBinds.key_autoGraf, nameof(Toggles.autograf));
            // ------------------------------------------------------------------------

            CreateDivider();

            // --- DISABLE CARS ---
            // ------------------------------------------------------------------------
            CreateButton(elementBaseX, Properties.line, elementBaseW, Properties.menu_element_h, toolsFunctions.FuncDisCars, "Disable Cars", KeyBinds.key_DisCars, nameof(Toggles.cars));
            // ------------------------------------------------------------------------

            CreateDivider();

            // --- SHOW TRIGGERS ---
            // ------------------------------------------------------------------------
            CreateButton(elementBaseX, Properties.line, elementBaseW, Properties.menu_element_h, toolsFunctions.FuncTriggers, "Show Triggers", KeyBinds.key_Triggers, nameof(Toggles.triggers));
            // ------------------------------------------------------------------------

            CreateLineNoExtra();

            CreateSelector(elementBaseX, Properties.line, ftButtons, Properties.menu_element_h, toolsFunctions.FuncChangeSpawn, toolsFunctions.SpawnToName(Attributes.spawnIndex), KeyBinds.key_spawnPrev, KeyBinds.key_spawnNext, "Spawn:");
            CreateRevertSelectorLine();
            CreateSelector(elementBaseX + ftButtons + (Properties.spacingHor * 2), Properties.line, ftButtons, Properties.menu_element_h, toolsFunctions.FuncChangeDreamSpawn, toolsFunctions.DreamSpawnToName(Attributes.dreamSpawnIndex), KeyBinds.key_dreamPrev, KeyBinds.key_dreamNext, "Dream:");

            CreateButton(elementBaseX, Properties.menu_current_y + Properties.menu_h - (Properties.menu_element_h + Properties.margin_h), elementBaseW, Properties.menu_element_h, ToggleExtraMenu, "More ...");

            // NO MENU //
            // ------------------------------------------------------------------------
            menuObjects = speedoObjects;

            float spacing = 5f;
            float spacingSpeed = 4f;
            shadow_speed_h = CreateText(Properties.shadowOffset, Properties.current_screen_h + Properties.shadowOffset, (int)canvasObj.RectTransform().sizeDelta.x, (int)canvasObj.RectTransform().sizeDelta.y, "", Properties.fontSizeScreen, false, 0, 0, false, textColor: Color.black);
            speed_h = CreateText(0, Properties.current_screen_h, (int)canvasObj.RectTransform().sizeDelta.x, (int)canvasObj.RectTransform().sizeDelta.y, "", Properties.fontSizeScreen, false, 0, 0, false);

            shadow_speed_v = CreateText(Properties.shadowOffset - (int)(Properties.fontSizeScreen * spacingSpeed), Properties.current_screen_h + Properties.shadowOffset, (int)canvasObj.RectTransform().sizeDelta.x, (int)canvasObj.RectTransform().sizeDelta.y, "", Properties.fontSizeScreen, false, 0, 0, false, textColor: Color.black);
            speed_v = CreateText(-(int)(Properties.fontSizeScreen * spacingSpeed), Properties.current_screen_h, (int)canvasObj.RectTransform().sizeDelta.x, (int)canvasObj.RectTransform().sizeDelta.y, "", Properties.fontSizeScreen, false, 0, 0, false);

            shadow_speed_s = CreateText(Properties.shadowOffset + (int)(Properties.fontSizeScreen * spacingSpeed), Properties.current_screen_h + Properties.shadowOffset, (int)canvasObj.RectTransform().sizeDelta.x, (int)canvasObj.RectTransform().sizeDelta.y, "", Properties.fontSizeScreen, false, 0, 0, false, textColor: Color.black);
            speed_s = CreateText((int)(Properties.fontSizeScreen * spacingSpeed), Properties.current_screen_h, (int)canvasObj.RectTransform().sizeDelta.x, (int)canvasObj.RectTransform().sizeDelta.y, "", Properties.fontSizeScreen, false, 0, 0, false);

            menuObjects = cordObjects;

            shadow_cord_x = CreateText(Properties.shadowOffset - (int)(Properties.fontSizeScreen * spacing), Properties.current_screen_h + Properties.shadowOffset, (int)canvasObj.RectTransform().sizeDelta.x, (int)canvasObj.RectTransform().sizeDelta.y, "", Properties.fontSizeScreen, false, 0, 0, false, textColor: Color.black);
            cord_x = CreateText(-(int)(Properties.fontSizeScreen * spacing), Properties.current_screen_h, (int)canvasObj.RectTransform().sizeDelta.x, (int)canvasObj.RectTransform().sizeDelta.y, "", Properties.fontSizeScreen, false, 0, 0, false);

            shadow_cord_y = CreateText(Properties.shadowOffset, Properties.current_screen_h + Properties.shadowOffset, (int)canvasObj.RectTransform().sizeDelta.x, (int)canvasObj.RectTransform().sizeDelta.y, "", Properties.fontSizeScreen, false, 0, 0, false, textColor: Color.black);
            cord_y = CreateText(0, Properties.current_screen_h, (int)canvasObj.RectTransform().sizeDelta.x, (int)canvasObj.RectTransform().sizeDelta.y, "", Properties.fontSizeScreen, false, 0, 0, false);

            shadow_cord_z = CreateText(Properties.shadowOffset + (int)(Properties.fontSizeScreen * spacing), Properties.current_screen_h + Properties.shadowOffset, (int)canvasObj.RectTransform().sizeDelta.x, (int)canvasObj.RectTransform().sizeDelta.y, "", Properties.fontSizeScreen, false, 0, 0, false, textColor: Color.black);
            cord_z = CreateText((int)(Properties.fontSizeScreen * spacing), Properties.current_screen_h, (int)canvasObj.RectTransform().sizeDelta.x, (int)canvasObj.RectTransform().sizeDelta.y, "", Properties.fontSizeScreen, false, 0, 0, false);
            // ------------------------------------------------------------------------

            // --- MENU 2 ---
            // ------------------------------------------------------------------------
            menuObjects = menu2Objects;

            Properties.line = Properties.menu_current_y + Properties.menu_strip_h + Properties.margin_h;

            CreateButton(elementBaseX, Properties.line, elementBaseW, Properties.menu_element_h, toolsFunctions.FuncUnlock, "Unlock All Abilities");

            CreateLine();

            CreateButton(elementBaseX, Properties.line, elementBaseW, Properties.menu_element_h, toolsFunctions.FuncUnlockChars, "Unlock All Characters");

            CreateLine();

            CreateButton(elementBaseX, Properties.line, elementBaseW, Properties.menu_element_h, toolsFunctions.FuncForcePolice, "Do Heckin Crime");

            //CreateLine();

            //CreateText(elementBaseX, Properties.line, elementBaseW, Properties.menu_element_h, $"More To Be Added...");

            CreateButton(elementBaseX, Properties.menu_current_y + Properties.menu_h - (Properties.menu_element_h + Properties.margin_h), elementBaseW, Properties.menu_element_h, ToggleExtraMenu, "Back ...");
            // ------------------------------------------------------------------------

            /*
            menuObjects = menu1Objects;
            cursor = CreateCursor(elementBaseX, Properties.line, elementBaseW, Properties.menu_element_h, new Color(1f, 0f, 0f, 0.7f));
            HandleCursor();
            */
        }

        private int buttonIndexSelected = 0;
        private List<GameObject> selections { get { return menuObjects?.Where(x => x.TryGetComponent(out Button _)).ToList(); } }
        private void HandleCursor(int i = 0)
        {
            if (!toolsGUI.menuBlocked && selections?.Count() > 0)
            {
                buttonIndexSelected = buttonIndexSelected + i >= selections.Count() ? 0 : buttonIndexSelected + i < 0 ? selections.Count() - 1 : buttonIndexSelected + i;

                GameObject selection = selections[buttonIndexSelected];
                cursor.RectTransform().sizeDelta = selection.RectTransform().sizeDelta;
                cursor.transform.position = new Vector2(selection.transform.position.x + (selection.RectTransform().sizeDelta.x / 2), selection.transform.position.y - (selection.RectTransform().sizeDelta.y / 2));
            }
        }

        private void UpdateCursor()
        {
            // R3
            if (Input.GetKeyDown(KeyCode.JoystickButton9))
                toolsGUI.MenuToggleFocus();

            if (!toolsGUI.menuBlocked && cursor != null)
            {
                cursor.gameObject.SetActive(true);

                // Down D-Pad Input
                if (ToolsGame.Game.GetGameInput().GetButtonNew(56))
                    HandleCursor(1);

                // Up D-Pad Input
                else if (ToolsGame.Game.GetGameInput().GetButtonNew(21))
                    HandleCursor(-1);

                // Jump D-Pad Input
                if (ToolsGame.Game.GetGameInput().GetButtonNew(7))
                    selections[buttonIndexSelected]?.GetComponent<Button>().onClick.Invoke();
            }
            else
            {
                cursor.gameObject.SetActive(false);
            }
        }

        GameObject speed_h;
        Text text_speed_h;
        GameObject shadow_speed_h;
        Text text_shadow_speed_h;

        GameObject speed_v;
        Text text_speed_v;
        GameObject shadow_speed_v;
        Text text_shadow_speed_v;

        GameObject speed_s;
        Text text_speed_s;
        GameObject shadow_speed_s;
        Text text_shadow_speed_s;

        GameObject cord_x;
        Text text_cord_x;
        GameObject shadow_cord_x;
        Text text_shadow_cord_x;

        GameObject cord_y;
        Text text_cord_y;
        GameObject shadow_cord_y;
        Text text_shadow_cord_y;

        GameObject cord_z;
        Text text_cord_z;
        GameObject shadow_cord_z;
        Text text_shadow_cord_z;

        void Update()
        {
            Player player = ToolsGame.Game.GetPlayer();

            if (speed_h != null)
            {
                if (text_speed_h == null) { text_speed_h = speed_h.GetComponent<Text>(); } if (text_shadow_speed_h == null) { text_shadow_speed_h = shadow_speed_h.GetComponent<Text>(); }
                HandleText(player, text_speed_h, text_shadow_speed_h, $"H: {player?.GetForwardSpeed().ToString("0.00")}");
            }

            if (speed_v != null)
            {
                if (text_speed_v == null) { text_speed_v = speed_v.GetComponent<Text>(); } if (text_shadow_speed_v == null) { text_shadow_speed_v = shadow_speed_v.GetComponent<Text>(); }
                HandleText(player, text_speed_v, text_shadow_speed_v, $"V: {player?.motor.velocity.y.ToString("0.00")}");
            }

            if (speed_s != null)
            {
                if (text_speed_s == null) { text_speed_s = speed_s.GetComponent<Text>(); } if (text_shadow_speed_s == null) { text_shadow_speed_s = shadow_speed_s.GetComponent<Text>(); }
                HandleText(player, text_speed_s, text_shadow_speed_s, $"S: {ToolsFunctions.Instance.GetStorageSpeed().ToString("0.00")}");
            }

            if (cord_x != null)
            {
                if (text_cord_x == null) { text_cord_x = cord_x.GetComponent<Text>(); }
                if (text_shadow_cord_x == null) { text_shadow_cord_x = shadow_cord_x.GetComponent<Text>(); }
                HandleText(player, text_cord_x, text_shadow_cord_x, $"X: {player?.transform.position.x.ToString("0.00")}");
            }

            if (cord_y != null)
            {
                if (text_cord_y == null) { text_cord_y = cord_y.GetComponent<Text>(); }
                if (text_shadow_cord_y == null) { text_shadow_cord_y = shadow_cord_y.GetComponent<Text>(); }
                HandleText(player, text_cord_y, text_shadow_cord_y, $"Y: {player?.transform.position.y.ToString("0.00")}");
            }

            if (cord_z != null)
            {
                if (text_cord_z == null) { text_cord_z = cord_z.GetComponent<Text>(); }
                if (text_shadow_cord_z == null) { text_shadow_cord_z = shadow_cord_z.GetComponent<Text>(); }
                HandleText(player, text_cord_z, text_shadow_cord_z, $"Z: {player?.transform.position.z.ToString("0.00")}");
            }

            //UpdateCursor();
        }

        private void HandleText( Player player, Text textField, Text shadowText, string text )
        {
            if ( (Settings.shouldHideSpeed && (text.StartsWith("H") || text.StartsWith("V") || text.StartsWith("S"))) || (Settings.shouldHideCords && (text.StartsWith("X") || text.StartsWith("Y") || text.StartsWith("Z"))) )
                return;

            if (Settings.shouldTieLabels && (!menuIsVisible || menuIsClosing))
            {
                textField.text = "";
                shadowText.text = "";
                return;
            }

            BaseModule baseMod = ToolsGame.Game.GetBaseModule();
            if (player != null && baseMod != null && !baseMod.IsInGamePaused && baseMod.StageManager != null && !baseMod.StageManager.IsExtendingLoadingScreen)
                textField.text = text;
            else
                textField.text = "";
            shadowText.text = textField.text;
        }

        private bool showingExtra = false;
        private void ToggleExtraMenu()
        {
            showingExtra = !showingExtra;

            foreach (GameObject mo in menu1Objects)
                mo.gameObject.SetActive(!showingExtra);

            foreach (GameObject mo in menu2Objects)
                mo.gameObject.SetActive(showingExtra);

            UpdateLinks();
            ToolsFunctions.Instance.UpdateMenuInfo();

            /*
            menuObjects = showingExtra ? menu2Objects : menu1Objects;
            buttonIndexSelected = selections != null ? Mathf.Max(selections.Count() - 1, 0) : 0;
            HandleCursor();
            */
        }

        private void CreateDivider()
        {
            Properties.line += (Properties.spacing * 2) - Properties.spacingSmall;
        }

        private void CreateLine()
        {
            Properties.line += Properties.spacing;
        }

        private void CreateLineNoExtra()
        {
            Properties.line += Properties.spacing - Properties.spacingExtra;
        }

        private void CreateMidLine()
        {
            Properties.line += (int)(Properties.spacing * 1.4f);
        }

        private void CreateRevertSelectorLine()
        {
            Properties.line -= Properties.spacingSmall;
        }

        private void CreateBox(int x, int y, int w, int h, Color color, bool shadow = true, float xOffset = 0f, float yOffset = 1f)
        {
            if (shadow)
                GUICreateBox(Pos(x + Properties.shadowOffset, y + Properties.shadowOffset), Size(w, h), Colors.shadow, xOffset, yOffset);

            GUICreateBox(Pos(x, y), Size(w, h), color, xOffset, yOffset);
        }

        private GameObject CreateCursor(int x, int y, int w, int h, Color color)
        {
            return GUICreateCursor(Pos(x, y), Size(w, h), color);
        }

        private void CreateSelector(int x, int y, int w, int h, Func<int, string> func, string text, KeyCode keybindDec = KeyCode.None, KeyCode keybindInc = KeyCode.None, string title = "", bool shadow = true, float xOffset = 0f, float yOffset = 1f)
        {
            if (keybindDec != KeyCode.None) { ToolsBindings.keysToCheck += delegate { if (Input.GetKeyDown(keybindDec)) { func(-1); } }; }
            if (keybindInc != KeyCode.None) { ToolsBindings.keysToCheck += delegate { if (Input.GetKeyDown(keybindInc)) { func(1); } }; }

            string keyDec = keybindDec != KeyCode.None ? toolsConfig.GetKeyString(keybindDec) : "";
            string keyInc = keybindInc != KeyCode.None ? toolsConfig.GetKeyString(keybindInc) : "";
            int newWidthD = keyDec.Length > 1 ? (int)(h * 0.5f) * keyDec.Length : h;
            int newWidthI = keyInc.Length > 1 ? (int)(h * 0.5f) * keyInc.Length : h;
            int newWidth = newWidthD + newWidthI;
            int boxWidth = w - newWidth;

            if (title != "")
            {
                CreateText(x, y, boxWidth, h, title, (int)(Properties.fontSize * 0.9f));
                y += Properties.spacingSmall;
                Properties.line = y;
            }

            if (shadow)
                GUICreateBox(Pos(x + Properties.shadowOffset, y + Properties.shadowOffset), Size(w, h), Colors.shadow, xOffset, yOffset);

            GUICreateBox(Pos(x, y), Size(boxWidth, h), Colors.selector, xOffset, yOffset);

            GameObject label = CreateText(x, y, boxWidth, h, text, shadow: false, xOffset: xOffset, yOffset: yOffset);

            GUICreateSelector(Pos(x + (boxWidth), y), Size(newWidthD, h), Colors.buttonOff, func, label, -1, text);
            GUICreateSelector(Pos(x + (boxWidth) + (newWidthD), y), Size(newWidthI, h), Colors.buttonOn, func, label, 1, text);

            if (keyDec != "") { CreateKeybind(x + (boxWidth), y, newWidthD, h, Colors.keybind, Colors.text, keyDec, hasBox: false); }
            if (keyInc != "") { CreateKeybind(x + (boxWidth) + (newWidthD), y, newWidthI, h, Colors.keybind, Colors.text, keyInc, hasBox: false); }
        }

        private void CreateTextFieldMulti(int x, int y, int w, int h, bool shadow = true, float xOffset = 0f, float yOffset = 1f, params string[][] info)
        {
            int baseWidth   = w;
            int fitWidth    = info.Length;
            int newWidth    = Mathf.RoundToInt(baseWidth / fitWidth) - Properties.spacingHor;
            int itemIndex   = 0;
            foreach(string[] item in info)
            {
                if (item.Length >= 2)
                    CreateTextField(x + ((newWidth + (Properties.spacingHor * 2)) * itemIndex), y, newWidth, h, item[0], item[1], item.Length >= 3 ? item[2] : "", shadow, xOffset, yOffset);

                itemIndex++;
            }
        }

        private void CreateTextField(int x, int y, int w, int h, string placeholder, string controlFloat, string title = "", bool shadow = true, float xOffset = 0f, float yOffset = 1f)
        {
            if (title != "")
            {
                CreateText(x, y, w, h, title, (int)(Properties.fontSize * 0.9f));
                y += Properties.spacingSmall;
                Properties.line = y;
            }

            if (shadow)
                GUICreateBox(Pos(x + Properties.shadowOffset, y + Properties.shadowOffset), Size(w, h), Colors.shadow, xOffset, yOffset);

            PropertyInfo floatFieldInfo = typeof(Attributes).GetProperty(controlFloat, BindingFlags.Public | BindingFlags.Static);

            GUICreateInputField(Pos(x, y), Size(w, h), placeholder, floatFieldInfo, xOffset, yOffset);
        }

        private void CreateButton(int x, int y, int w, int h, UnityAction action, string text = "", KeyCode keybind = KeyCode.None, string field = "", bool shadow = true, float xOffset = 0f, float yOffset = 1f)
        {
            int newWidth = 0;
            string key = "";
            bool isKeybind = false;
            if (keybind != KeyCode.None)
            {
                key = toolsConfig.GetKeyString(keybind);
                newWidth = key.Length > 1 ? (int)(h * 0.5f) * key.Length : h;
                isKeybind = true;

                ToolsBindings.keysToCheck += delegate { if (Input.GetKeyDown(keybind)) { action.Invoke(); } };
            }

            bool toggle = false;
            PropertyInfo pi = null;
            if (field != "")
            {
                pi = typeof(Toggles).GetProperty(field, BindingFlags.Static | BindingFlags.Public);
                bool.TryParse(pi?.GetValue(false)?.ToString(), out toggle);
            }

            int newW = w - newWidth - (newWidth > 0 ? 1 : 0);

            if (shadow)
                GUICreateBox(Pos(x + Properties.shadowOffset, y + Properties.shadowOffset), Size(w, h), Colors.shadow, xOffset, yOffset);

            GUICreateButton(Pos(x, y), Size(newW, h), pi != null ? toggle ? Colors.buttonOn : Colors.buttonOff : text != "◄" ? Colors.button : Colors.buttonOff, action, pi, xOffset, yOffset);

            CreateText(x, y, newW, h, text);

            if (isKeybind)
                CreateKeybind(x + (w - newWidth), y, newWidth, h, Colors.keybind, Colors.text, key);
        }

        private void CreateKeybind(int x, int y, int w, int h, Color backColor, Color textColor, string key, bool shadow = true, float xOffset = 0f, float yOffset = 1f, bool hasBox = true)
        {
            if (hasBox)
                CreateBox(x, y, w, h, backColor, false, xOffset, yOffset);

            CreateText(x, y, w, h, key, shadow: true, xOffset: xOffset, yOffset: yOffset);
        }

        private GameObject CreateText(int x, int y, int w, int h, string text, int fontSize = 0, bool shadow = true, float xOffset = 0f, float yOffset = 1f, bool toMenu = true, TextAnchor anchor = TextAnchor.MiddleCenter, Color textColor = default)
        {
            if (textColor == default)
                textColor = Colors.text;

            if (fontSize == 0)
                fontSize = Properties.fontSize;

            if (shadow)
                GUICreateText(Pos(x + (Properties.shadowOffset / 2), y + (Properties.shadowOffset / 2)), Size(w, h), Colors.shadow, RemoveTextBetweenDelimiters(text, '<', '>'), fontSize, xOffset: xOffset, yOffset: yOffset, toMenu, anchor);

            return GUICreateText(Pos(x, y), Size(w, h), textColor, text, fontSize, xOffset, yOffset, toMenu, anchor);
        }
    }
}
