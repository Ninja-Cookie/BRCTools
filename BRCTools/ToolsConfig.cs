using BepInEx;
using BepInEx.Configuration;
using Reptile;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace BRCTools
{
    internal class ToolsConfig : MonoBehaviour
    {
        public static ToolsConfig Instance;

        public ToolsConfig()
        {
            Instance = this;
        }

        const string            folderName      = "BRCTools";
        const string            saveFolderName  = "SAVES";
        const string            descFileName    = "descriptions";
        const string            keybindFileName = "Keybinds";
        const string            settingFileName = "Settings";
        public static string    ext_saves       = ".brctools";
        public static string    folder_config   = Path.Combine(Paths.ConfigPath,    folderName);
        public static string    folder_saves    = Path.Combine(folder_config,       saveFolderName);
        public static string    file_desc       = Path.Combine(folder_saves,        $"{descFileName}.txt");

        public static HashSet<string> desc_contents = new HashSet<string>();

        private static List<string> saves = new List<string>();
        public static Dictionary<string, string> files_saves = new Dictionary<string, string>();

        ConfigFile keybinds;
        ConfigFile settings;

        private void HandleSettingsEntry(string entry, ref bool setting)
        {
            settings.Bind<bool>("Settings", entry, setting);
            settings.TryGetEntry<bool>(new ConfigDefinition("Settings", entry), out ConfigEntry<bool> entryOut);
            if (entryOut != null) { setting = entryOut.Value; }
        }

        void Awake()
        {
            if (!Directory.Exists(folder_config))
                Directory.CreateDirectory(folder_config);

            if (Directory.Exists(folder_config))
            {
                settings = new ConfigFile(GetAsCFGPath(settingFileName), true);

                HandleSettingsEntry("Start Menu Open", ref Settings.shouldMenuStartShown);
                HandleSettingsEntry("Enable Mouse On Showing Menu", ref Settings.shouldFocusMenuOnShow);
                HandleSettingsEntry("Auto Graffiti Uses All Graffiti", ref Settings._shouldUseAllGraffitis);
                HandleSettingsEntry("Start Coords Hidden", ref Settings.shouldHideCords);
                HandleSettingsEntry("Start Speed Hidden", ref Settings.shouldHideSpeed);
                HandleSettingsEntry("Coord & Speed Visibility Based On Menu", ref Settings.shouldTieLabels);
                HandleSettingsEntry("Allow All Cutscene Skipping", ref Settings.shouldAllowCutsceneSkip);
                HandleSettingsEntry("Speed Up Cutscene Skip If All", ref Settings.speedUpCutsceneSkip);
                HandleSettingsEntry("Style Includes Special Skateboard", ref Settings.shouldHaveSpecial);

                keybinds = new ConfigFile(GetAsCFGPath(keybindFileName), true);

                Dictionary<string, string> keyDescriptions = new Dictionary<string, string>()
                {
                    { nameof(KeyBinds.key_MenuToggle),      "Toggle Menu" },
                    { nameof(KeyBinds.key_MenuMouse),       "Toggle Mouse Input" },
                    { nameof(KeyBinds.key_Refill),          "Refill Boost" },
                    { nameof(KeyBinds.key_Invulnerable),    "Become Invulnerable" },
                    { nameof(KeyBinds.key_EndWanted),       "End Wanted State" },
                    { nameof(KeyBinds.key_InfBoost),        "Infinite Boost" },
                    { nameof(KeyBinds.key_DisPolice),       "Disable Police" },
                    { nameof(KeyBinds.key_CharPrev),        "Previous Character" },
                    { nameof(KeyBinds.key_CharNext),        "Next Character" },
                    { nameof(KeyBinds.key_Save),            "Save Position" },
                    { nameof(KeyBinds.key_Load),            "Load Position" },
                    { nameof(KeyBinds.key_StylePrev),       "Previous Style" },
                    { nameof(KeyBinds.key_StyleNext),       "Next Style" },
                    { nameof(KeyBinds.key_OutfitPrev),      "Previous Outfit" },
                    { nameof(KeyBinds.key_OutfitNext),      "Next Outfit" },
                    { nameof(KeyBinds.key_LevelPrev),       "Previous Level" },
                    { nameof(KeyBinds.key_LevelNext),       "Next Level" },
                    { nameof(KeyBinds.key_GoToLevel),       "Load Level" },
                    { nameof(KeyBinds.key_LockFPS),         "Lock FPS" },
                    { nameof(KeyBinds.key_SetTimescale),    "Set Timescale" },
                    { nameof(KeyBinds.key_Noclip),          "Toggle Noclip" },
                    { nameof(KeyBinds.key_Fly),             "Toggle Flying" },
                    { nameof(KeyBinds.key_Triggers),        "Toggle Triggers" },
                    { nameof(KeyBinds.key_DisSaving),       "Disable Saving" },
                    { nameof(KeyBinds.key_DisCars),         "Disable Cars" },
                    { nameof(KeyBinds.key_ResGraf),         "Reset Graffiti" },
                    { nameof(KeyBinds.key_ResGame),         "Load Current Save" },
                    { nameof(KeyBinds.key_ResLevel),        "Restart Level" },
                    { nameof(KeyBinds.key_prevSave),        "Previous Save" },
                    { nameof(KeyBinds.key_nextSave),        "Next Save" },
                    { nameof(KeyBinds.key_goToSave),        "Load Selected Save" },
                    { nameof(KeyBinds.key_autoGraf),        "Auto Graffiti" },
                    { nameof(KeyBinds.key_refillHP),        "Refill Health" },
                    { nameof(KeyBinds.key_saveGame),        "Create Save File" },
                    { nameof(KeyBinds.key_spawnPrev),       "Previous Spawn" },
                    { nameof(KeyBinds.key_spawnNext),       "Next Spawn" },
                    { nameof(KeyBinds.key_dreamPrev),       "Previous Dream Spawn" },
                    { nameof(KeyBinds.key_dreamNext),       "Next Dream Spawn" }
                };
                int failsafe = 0;
                foreach (FieldInfo key in typeof(KeyBinds).GetFields(BindingFlags.Static | BindingFlags.Public))
                {
                    string keyDesc = $"NO_DESC_{failsafe++}";

                    if (keyDescriptions.TryGetValue(key.Name, out string result))
                        keyDesc = result;
                    else
                        keyDescriptions.Add(key.Name, keyDesc);

                    keybinds.Bind<string>
                    (
                        "Keybinds",
                        keyDesc,
                        GetKeyString((KeyCode)key.GetValue(null))
                    );
                }
                failsafe = 0;
                foreach (var item in keybinds.Keys)
                {
                    KeyCode realKey = KeyCode.None;
                    if (keybinds.TryGetEntry(item, out ConfigEntry<string> keybind))
                        realKey = GetStringKey(keybind.Value.ToUpper());

                    string field = keyDescriptions.FirstOrDefault(key => key.Value == item.Key).Key;

                    if (field != null)
                        typeof(KeyBinds).GetField(field, BindingFlags.Static | BindingFlags.Public).SetValue(null, realKey);
                }

                UpdateSaveFiles();
            }
        }

        public void UpdateSaveFiles()
        {
            saves.Clear();
            files_saves.Clear();

            // Get File Names
            if (!Directory.Exists(folder_saves))
                Directory.CreateDirectory(folder_saves);

            if (Directory.Exists(folder_saves))
            {
                foreach (var file in Directory.GetFiles(folder_saves))
                {
                    if (file.EndsWith(ext_saves))
                    {
                        string filePath = Path.Combine(folder_saves, file);
                        saves.Add(filePath);
                    }
                }
                saves.Sort();

                // Get File Descriptions
                if (!File.Exists(file_desc))
                    File.CreateText(file_desc);

                if (saves.Count > 0 && File.Exists(file_desc))
                {
                    foreach (var save in saves)
                    {
                        string saveFile = $"{save.Remove(0, save.LastIndexOf(@"\") + 1)}:";

                        string line = File.ReadAllLines(file_desc).FirstOrDefault(x => x.StartsWith(saveFile));
                        if (line == default) { line = $"{saveFile} {saveFile.Substring(0, saveFile.Length - 1).Replace(ext_saves, string.Empty)}"; File.AppendAllText(file_desc, $"\n{line}"); }

                        string description = line.Remove(0, saveFile.Length); description = description.Replace("\t", string.Empty);
                        string testEmptyString = description.Replace(" ", string.Empty);

                        while (description.Length > 0 && testEmptyString != "" && description.StartsWith(" "))
                            description = description.Remove(0, 1);

                        if (description.Count() > 30)
                            description = description.Substring(0, 30);

                        if (!files_saves.Keys.ToArray().Contains(save))
                            files_saves.Add(save, description);
                    }
                    desc_contents = File.ReadAllLines(file_desc).ToHashSet<string>();
                    files_saves = files_saves.OrderBy(x => x.Key, StringComparer.OrdinalIgnoreCase).ToDictionary(x => x.Key, y => y.Value);
                }
            }
        }

        private string GetAsCFGPath(string file)
        {
            return Path.Combine(folder_config, $"{file}.cfg");
        }

        public class KeyBinds
        {
            public static KeyCode key_MenuToggle    = KeyCode.Semicolon;
            public static KeyCode key_MenuMouse     = KeyCode.P;
            public static KeyCode key_Refill        = KeyCode.R;
            public static KeyCode key_Invulnerable  = KeyCode.I;
            public static KeyCode key_EndWanted     = KeyCode.K;
            public static KeyCode key_InfBoost      = KeyCode.U;
            public static KeyCode key_DisPolice     = KeyCode.O;
            public static KeyCode key_CharPrev      = KeyCode.None;
            public static KeyCode key_CharNext      = KeyCode.None;
            public static KeyCode key_Save          = KeyCode.H;
            public static KeyCode key_Load          = KeyCode.J;
            public static KeyCode key_StylePrev     = KeyCode.None;
            public static KeyCode key_StyleNext     = KeyCode.None;
            public static KeyCode key_OutfitPrev    = KeyCode.None;
            public static KeyCode key_OutfitNext    = KeyCode.None;
            public static KeyCode key_LevelPrev     = KeyCode.LeftBracket;
            public static KeyCode key_LevelNext     = KeyCode.RightBracket;
            public static KeyCode key_GoToLevel     = KeyCode.Alpha0;
            public static KeyCode key_LockFPS       = KeyCode.L;
            public static KeyCode key_SetTimescale  = KeyCode.T;
            public static KeyCode key_Noclip        = KeyCode.Backslash;
            public static KeyCode key_Fly           = KeyCode.Slash;
            public static KeyCode key_Triggers      = KeyCode.X;
            public static KeyCode key_DisSaving     = KeyCode.B;
            public static KeyCode key_DisCars       = KeyCode.C;
            public static KeyCode key_ResGraf       = KeyCode.N;
            public static KeyCode key_ResGame       = KeyCode.V;
            public static KeyCode key_ResLevel      = KeyCode.G;
            public static KeyCode key_prevSave      = KeyCode.Minus;
            public static KeyCode key_nextSave      = KeyCode.Equals;
            public static KeyCode key_goToSave      = KeyCode.Backspace;
            public static KeyCode key_autoGraf      = KeyCode.M;
            public static KeyCode key_refillHP      = KeyCode.Z;
            public static KeyCode key_saveGame      = KeyCode.Period;
            public static KeyCode key_spawnPrev     = KeyCode.None;
            public static KeyCode key_spawnNext     = KeyCode.None;
            public static KeyCode key_dreamPrev     = KeyCode.None;
            public static KeyCode key_dreamNext     = KeyCode.None;
        }

        public class Settings
        {
            public static bool shouldFocusMenuOnShow = false;

            public static bool shouldMenuStartShown = true;

            internal static bool _shouldUseAllGraffitis = false;
            public static bool shouldUseAllGraffitis { get { return _shouldUseAllGraffitis; } set { _shouldUseAllGraffitis = value; ToolsFunctions.used_graffiti.Clear(); } }

            public static bool shouldHideCords = false;
            public static bool shouldHideSpeed = false;
            public static bool shouldTieLabels = false;

            public static bool shouldAllowCutsceneSkip = true;
            public static bool speedUpCutsceneSkip = false;

            public static bool shouldHaveSpecial = false;
        }

        public string GetKeyString(KeyCode keycode)
        {
            if (keyToString.TryGetValue(keycode, out string result))
                return result;
            return keycode.ToString().Length > 4 ? keycode.ToString().Substring(0, 4).ToUpper() : keycode.ToString().ToUpper();
        }

        public KeyCode GetStringKey(string key)
        {
            int i = 0;
            foreach(string stringKey in keyToString.Values)
            {
                if (stringKey == key)
                {
                    return keyToString.Keys.ToArray()[i];
                }
                i++;
            }

            if (System.Enum.TryParse(key, out KeyCode realKey))
                return realKey;

            return KeyCode.None;
        }

        private Dictionary<KeyCode, string> keyToString = new Dictionary<KeyCode, string>
        {
            { KeyCode.Alpha0,       "0" },
            { KeyCode.Alpha1,       "1" },
            { KeyCode.Alpha2,       "2" },
            { KeyCode.Alpha3,       "3" },
            { KeyCode.Alpha4,       "4" },
            { KeyCode.Alpha5,       "5" },
            { KeyCode.Alpha6,       "6" },
            { KeyCode.Alpha7,       "7" },
            { KeyCode.Alpha8,       "8" },
            { KeyCode.Alpha9,       "9" },
            { KeyCode.Keypad0,      "N0" },
            { KeyCode.Keypad1,      "N1" },
            { KeyCode.Keypad2,      "N2" },
            { KeyCode.Keypad3,      "N3" },
            { KeyCode.Keypad4,      "N4" },
            { KeyCode.Keypad5,      "N5" },
            { KeyCode.Keypad6,      "N6" },
            { KeyCode.Keypad7,      "N7" },
            { KeyCode.Keypad8,      "N8" },
            { KeyCode.Keypad9,      "N9" },
            { KeyCode.KeypadPeriod, "N." },
            { KeyCode.KeypadDivide, "N/" },
            { KeyCode.KeypadMultiply,"N*" },
            { KeyCode.KeypadMinus,  "N-" },
            { KeyCode.KeypadEnter,  "NRTN" },
            { KeyCode.KeypadEquals, "N=" },
            { KeyCode.Backspace,    "BKS" },
            { KeyCode.Delete,       "DEL" },
            { KeyCode.Tab,          "TAB" },
            { KeyCode.Clear,        "CLR" },
            { KeyCode.Return,       "RTN" },
            { KeyCode.Pause,        "PSE" },
            { KeyCode.Escape,       "ESC" },
            { KeyCode.Space,        "SPC" },
            { KeyCode.UpArrow,      "UP" },
            { KeyCode.DownArrow,    "DWN" },
            { KeyCode.RightArrow,   "LFT" },
            { KeyCode.LeftArrow,    "RGT" },
            { KeyCode.Exclaim,      "!" },
            { KeyCode.DoubleQuote,  "\"" },
            { KeyCode.Hash,         "#" },
            { KeyCode.Dollar,       "$" },
            { KeyCode.Percent,      "%" },
            { KeyCode.Ampersand,    "&" },
            { KeyCode.Quote,        "'" },
            { KeyCode.LeftParen,    "(" },
            { KeyCode.RightParen,   ")" },
            { KeyCode.Asterisk,     "*" },
            { KeyCode.Plus,         "+" },
            { KeyCode.Comma,        "," },
            { KeyCode.Minus,        "-" },
            { KeyCode.Period,       "." },
            { KeyCode.Slash,        "/" },
            { KeyCode.Colon,        ":" },
            { KeyCode.Semicolon,    ";" },
            { KeyCode.Less,         "<" },
            { KeyCode.Equals,       "=" },
            { KeyCode.Greater,      ">" },
            { KeyCode.Question,     "?" },
            { KeyCode.At,           "@" },
            { KeyCode.LeftBracket,  "[" },
            { KeyCode.Backslash,    "\\"},
            { KeyCode.RightBracket, "]" },
            { KeyCode.Caret,        "^" },
            { KeyCode.Underscore,   "_" },
            { KeyCode.BackQuote,    "`" },
            { KeyCode.LeftCurlyBracket,"{" },
            { KeyCode.Pipe,         "|" },
            { KeyCode.RightCurlyBracket,"}" },
            { KeyCode.Tilde,        "~" },
            { KeyCode.Mouse0,       "M0" },
            { KeyCode.Mouse1,       "M1" },
            { KeyCode.Mouse2,       "M2" },
            { KeyCode.Mouse3,       "M3" },
            { KeyCode.Mouse4,       "M4" },
            { KeyCode.Mouse5,       "M5" },
            { KeyCode.Mouse6,       "M6" },
            { KeyCode.None,         ""   }
        };

        public Dictionary<string, string> charToName = new Dictionary<string, string>
        {
            { Characters.girl1.ToString(),              "Vinyl" },
            { Characters.frank.ToString(),              "Frank" },
            { Characters.ringdude.ToString(),           "Coil" },
            { Characters.metalHead.ToString(),          "Red" },
            { Characters.blockGuy.ToString(),           "Tryce" },
            { Characters.spaceGirl.ToString(),          "Bel" },
            { Characters.angel.ToString(),              "Rave" },
            { Characters.eightBall.ToString(),          "DOT EXE" },
            { Characters.dummy.ToString(),              "Solace" },
            { Characters.dj.ToString(),                 "DJ Cyber" },
            { Characters.medusa.ToString(),             "Eclipse" },
            { Characters.boarder.ToString(),            "Devil Theory" },
            { Characters.headMan.ToString(),            "Faux" },
            { Characters.prince.ToString(),             "Flesh Prince" },
            { Characters.jetpackBossPlayer.ToString(),  "Irene Rietveld" },
            { Characters.legendFace.ToString(),         "Felix" },
            { Characters.oldheadPlayer.ToString(),      "Old Head" },
            { Characters.robot.ToString(),              "Base" },
            { Characters.skate.ToString(),              "Jay" },
            { Characters.wideKid.ToString(),            "Mesh" },
            { Characters.futureGirl.ToString(),         "Futurism" },
            { Characters.pufferGirl.ToString(),         "Rise" },
            { Characters.bunGirl.ToString(),            "Shine" },
            { Characters.headManNoJetpack.ToString(),   "Faux (No Jetpack)" },
            { Characters.eightBallBoss.ToString(),      "DOT EXE (Boss)" },
            { Characters.legendMetalHead.ToString(),    "Felix (Red)" }
        };

        public Dictionary<string, string> styleToName = new Dictionary<string, string>
        {
            { MoveStyle.BMX.ToString(),                   "BMX" },
            { MoveStyle.SKATEBOARD.ToString(),            "Skateboard" },
            { MoveStyle.INLINE.ToString(),                "Inline" },
            { MoveStyle.SPECIAL_SKATEBOARD.ToString(),    "Special" }
        };

        public Dictionary<int, string> outfitToName = new Dictionary<int, string>
        {
            { 0, "Spring" },
            { 1, "Summer" },
            { 2, "Autumn" },
            { 3, "Winter" }
        };

        public Dictionary<int, string> levelToName = new Dictionary<int, string>
        {
            { (int)Stage.Prelude,   "Prelude" },
            { (int)Stage.hideout,   "Hideout" },
            { (int)Stage.downhill,  "Versum Hill" },
            { (int)Stage.square,    "Square" },
            { (int)Stage.tower,     "Brink Terminal" },
            { (int)Stage.Mall,      "Millennium Mall" },
            { (int)Stage.pyramid,   "Pyramid" },
            { (int)Stage.osaka,     "Mataan" },
        };

        public Dictionary<int, string> chapterToName = new Dictionary<int, string>
        {
            { 1,    "Chapter 1" },
            { 2,    "Chapter 2" },
            { 3,    "Chapter 3" },
            { 4,    "Chapter 4" },
            { 5,    "Chapter 5" },
            { 6,    "Chapter 6" },
        };
    }
}
