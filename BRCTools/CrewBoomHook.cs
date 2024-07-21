using System;
using UnityEngine;
using System.Linq;
using System.Reflection;
using System.IO;
using Reptile;
using System.Globalization;

namespace BRCTools
{
    internal class CrewBoomHook : MonoBehaviour
    {
        public static BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static;

        private const string _CrewBoomName = "CrewBoom";

        private static bool _CrewBoomInstalled = false;
        public  static bool CrewBoomInstalled
        {
            get
            {
                return !_CrewBoomInstalled ? _CrewBoomInstalled = FindObjectsOfType<BepInEx.BaseUnityPlugin>().Any(x => x.Info.Metadata.GUID == _CrewBoomName) : _CrewBoomInstalled;
            }
        }

        private static BepInEx.BaseUnityPlugin _CrewBoomPlugin = null;
        public  static BepInEx.BaseUnityPlugin CrewBoomPlugin
        {
            get
            {
                return CrewBoomInstalled && _CrewBoomPlugin == null ? _CrewBoomPlugin = FindObjectsOfType<BepInEx.BaseUnityPlugin>().FirstOrDefault(x => x.Info.Metadata.GUID == _CrewBoomName) : _CrewBoomPlugin;
            }
        }

        private static Type _CrewBoom_CharacterDatabase = null;
        public  static Type CrewBoom_CharacterDatabase
        {
            get
            {
                if (CrewBoomInstalled && _CrewBoom_CharacterDatabase == null)
                {
                    if (BepInEx.Utility.TryParseAssemblyName(CrewBoomPlugin.Info.Metadata.Name, out AssemblyName assemblyName) && BepInEx.Utility.TryResolveDllAssembly(assemblyName, Path.GetDirectoryName(CrewBoomPlugin.Info.Location), out Assembly assembly))
                    {
                        return _CrewBoom_CharacterDatabase = Type.GetType($"{_CrewBoomName}.CharacterDatabase, {assembly}");
                    }
                }
                return _CrewBoom_CharacterDatabase;
            }
        }

        private static int _NewCharacterCount = 0;
        public  static int NewCharacterCount
        {
            get
            {
                return _NewCharacterCount == 0 && CrewBoom_CharacterDatabase != null ? _NewCharacterCount = (int)CrewBoom_CharacterDatabase.GetProperty("NewCharacterCount", flags).GetValue(null, null) : _NewCharacterCount;
            }
        }

        public static int RealCharactersMax
        {
            get
            {
                int charMax = (int)Characters.MAX;

                if (CrewBoom_CharacterDatabase != null && NewCharacterCount != 0)
                    charMax += NewCharacterCount + 1;

                return charMax;
            }
        }

        public static string GetCharacterName(Characters character)
        {
            if (character != Characters.MAX && character != Characters.NONE)
            {
                if ((int)character > (int)Characters.MAX && CrewBoom_CharacterDatabase != null)
                {
                    string characterName = string.Empty;
                    object[] characterNameObjects = new object[] { character, characterName };

                    CrewBoom_CharacterDatabase.GetMethod("GetCharacterName", flags)?.Invoke(null, characterNameObjects);

                    return (string)characterNameObjects[1];
                }
                else if ((int)character < (int)Characters.MAX)
                {
                    IGameTextLocalizer localizer = Core.Instance?.Localizer;

                    if (localizer != null)
                    {
                        string charName = localizer.GetCharacterName(character)?.ToLower().FirstCharToUpper();

                        TextInfo textInfo = new CultureInfo(CultureInfo.CurrentCulture.ToString(), false)?.TextInfo;

                        if (textInfo != null)
                            charName = textInfo.ToTitleCase(charName);

                        if (character == Characters.headManNoJetpack)
                            charName = $"{charName} (No Jetpack)";

                        return charName;
                    }
                    else if (ToolsConfig.Instance.charToName.TryGetValue(character.ToString(), out string charName))
                    {
                        return charName;
                    }
                }
            }
            return character.ToString();
        }
    }
}
