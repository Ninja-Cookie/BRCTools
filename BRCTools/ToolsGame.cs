using Reptile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace BRCTools
{
    internal class ToolsGame : MonoBehaviour
    {
        public static ToolsGame Instance;
        public ToolsGame()
        {
            Instance = this;
        }

        public BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;

        // Get A Field From a Class | Example: GetField(WorldHandler.instance.GetCurrentPlayer(), "inCharacterSelectSpot");
        public FieldInfo Field(object classObject, String field)
        {
            return classObject.GetType().GetField(field, bindingFlags);
        }

        // Get a Field's Value From a Class | Example: GetFieldValue(WorldHandler.instance.GetCurrentPlayer(), "inWalkZone");
        public T FieldValue<T>(object classObject, String field)
        {
            return (T)classObject.GetType().GetField(field, bindingFlags).GetValue(classObject);
        }

        // Set Field Value in a Class | Example: SetFieldValue(WorldHandler.instance.GetCurrentPlayer(), "inWalkZone", true);
        public void SetFieldValue<T>(object classObject, String field, object value)
        {
            classObject.GetType().GetField(field, bindingFlags).SetValue(classObject, (T)value);
        }

        // Invoke a Method From a Class | Example: InvokeMethod(WorldHandler.instance.GetCurrentPlayer(), "Jump", new object[] { null });
        public void InvokeMethod(object classObject, String method, object[] objects = null)
        {
            classObject.GetType().GetMethod(method, bindingFlags).Invoke(classObject, objects);
        }

        public class CompatibilityFunctions
        {
            public static object GetFieldValue(object classObject, String field)
            {
                return classObject.GetType().GetField(field, Instance.bindingFlags).GetValue(classObject);
            }

            public static void SetFieldValue(object classObject, String field, object value)
            {
                classObject.GetType().GetField(field, Instance.bindingFlags).SetValue(classObject, value);
            }
        }

        public class Game : ToolsGame
        {
            // INFO //
            // --------------------------------------------------------------------------
            private enum PType
            {
                pi,
                fi,
                mi
            }

            private static void CreateInfo<T>(PType pType, Dictionary<string, PropertyInfo> pi = null, Dictionary<string, FieldInfo> fi = null, Dictionary<string, MethodInfo> mi = null)
            {

                Type infoType = typeof(T);

                switch (pType)
                {
                    case PType.pi:
                        if (pi != null && pi.Count == 0)
                        {
                            foreach (var item in infoType.GetProperties(Instance.bindingFlags))
                            {
                                pi.Add(item.Name, item);
                            }
                        }
                        break;

                    case PType.fi:
                        if (fi != null && fi.Count == 0)
                        {
                            foreach (FieldInfo item in infoType.GetFields(Instance.bindingFlags))
                            {
                                fi.Add(item.Name, item);
                            }
                        }
                        break;

                    case PType.mi:
                        if (mi != null && mi.Count == 0)
                        {
                            int i = 1;
                            MethodInfo[] methods = infoType.GetMethods(Instance.bindingFlags).Where(wmi => wmi.DeclaringType == infoType).ToArray();
                            foreach (MethodInfo item in methods)
                            {
                                string name = item.Name;
                                if (mi.Keys.Any((x) => item.Name == x))
                                    name += $"{i++}";

                                mi.Add(name, item);
                            }
                        }
                    break;
                }
            }

            // EXAMPLE: Game.invoke(Game.GetPlayer(), "Jump", null, Game.miPlayer);
            public static void invoke(object invoker, string methodName, object[] parms, Dictionary<string, MethodInfo> miDictionarry)
            {
                if (invoker != null && miDictionarry != null && miDictionarry.TryGetValue(methodName, out var method))
                {
                    method.Invoke(invoker, parms);
                }
            }

            public static bool TryGetValue<T>(Dictionary<string, FieldInfo> fiDictionarry, object relatedObj, string field, out T result)
            {
                if (fiDictionarry != null && relatedObj != null && fiDictionarry.TryGetValue(field, out var fi))
                {
                    result = (T)fi.GetValue(relatedObj);
                    return true;
                }
                result = default;
                return false;
            }

            public static bool TryGetValue<T>(Dictionary<string, PropertyInfo> piDictionarry, object relatedObj, string field, out T result)
            {
                if (piDictionarry != null && relatedObj != null && piDictionarry.TryGetValue(field, out var pi))
                {
                    result = (T)pi.GetValue(relatedObj, null);
                    return true;
                }
                result = default;
                return false;
            }

            public static bool TrySetValue<T>(Dictionary<string, FieldInfo> fiDictionarry, object relatedObj, string field, T value)
            {
                if (fiDictionarry != null && relatedObj != null && fiDictionarry.TryGetValue(field, out var fi))
                {
                    try
                    {
                        fi.SetValue(relatedObj, value);
                        return true;
                    }
                    catch
                    {
                        Debug.LogError($"FAILED TO SET ( {relatedObj}:{field} ) with ( {value} )");
                        return false;
                    }
                }
                return false;
            }
            // --------------------------------------------------------------------------

            private static Core _Game_Core;
            public static Core GetCore()
            {
                return _Game_Core == null ? _Game_Core = Core.Instance : _Game_Core;
            }

            private static BaseModule _Game_BaseModule;
            public static BaseModule GetBaseModule()
            {
                return _Game_BaseModule == null ? _Game_BaseModule = GetCore()?.BaseModule : _Game_BaseModule;
            }

            private static Dictionary<string, PropertyInfo> _piBaseMod = new Dictionary<string, PropertyInfo>();
            public static Dictionary<string, PropertyInfo> piBaseMod { get { if (_piBaseMod.Count == 0) { CreateInfo<BaseModule>(PType.pi, pi: _piBaseMod); } return _piBaseMod; } }

            private static Dictionary<string, FieldInfo> _fiBaseMod = new Dictionary<string, FieldInfo>();
            public static Dictionary<string, FieldInfo> fiBaseMod { get { if (_fiBaseMod.Count == 0) { CreateInfo<BaseModule>(PType.fi, fi: _fiBaseMod); } return _fiBaseMod; } }

            private static Dictionary<string, MethodInfo> _miBaseMod = new Dictionary<string, MethodInfo>();
            public static Dictionary<string, MethodInfo> miBaseMod { get { if (_miBaseMod.Count == 0) { CreateInfo<BaseModule>(PType.mi, mi: _miBaseMod); } return _miBaseMod; } }

            private static SaveManager _Game_SaveManager;
            public static SaveManager GetSaveManager()
            {
                return _Game_SaveManager == null ? _Game_SaveManager = GetCore()?.SaveManager : _Game_SaveManager;
            }

            private static Dictionary<string, PropertyInfo> _piSaveManager = new Dictionary<string, PropertyInfo>();
            public static Dictionary<string, PropertyInfo> piSaveManager { get { if (_piSaveManager.Count == 0) { CreateInfo<SaveManager>(PType.pi, pi: _piSaveManager); } return _piSaveManager; } }

            private static Dictionary<string, FieldInfo> _fiSaveManager = new Dictionary<string, FieldInfo>();
            public static Dictionary<string, FieldInfo> fiSaveManager { get { if (_fiSaveManager.Count == 0) { CreateInfo<SaveManager>(PType.fi, fi: _fiSaveManager); } return _fiSaveManager; } }

            private static Dictionary<string, MethodInfo> _miSaveManager = new Dictionary<string, MethodInfo>();
            public static Dictionary<string, MethodInfo> miSaveManager { get { if (_miSaveManager.Count == 0) { CreateInfo<SaveManager>(PType.mi, mi: _miSaveManager); } return _miSaveManager; } }

            public static SaveSlotHandler GetSaveSlotHandler()
            {
                SaveManager saveManager = GetSaveManager();
                if(saveManager != null && TryGetValue(fiSaveManager, saveManager, "saveSlotHandler", out SaveSlotHandler saveSlotHandler))
                {
                    return saveSlotHandler;
                }
                return null;
            }

            private static Dictionary<string, PropertyInfo> _piSaveSlotHandler = new Dictionary<string, PropertyInfo>();
            public static Dictionary<string, PropertyInfo> piSaveSlotHandler { get { if (_piSaveSlotHandler.Count == 0) { CreateInfo<SaveSlotHandler>(PType.pi, pi: _piSaveSlotHandler); } return _piSaveSlotHandler; } }

            private static Dictionary<string, FieldInfo> _fiSaveSlotHandler = new Dictionary<string, FieldInfo>();
            public static Dictionary<string, FieldInfo> fiSaveSlotHandler { get { if (_fiSaveSlotHandler.Count == 0) { CreateInfo<SaveSlotHandler>(PType.fi, fi: _fiSaveSlotHandler); } return _fiSaveSlotHandler; } }

            private static Dictionary<string, MethodInfo> _miSaveSlotHandler = new Dictionary<string, MethodInfo>();
            public static Dictionary<string, MethodInfo> miSaveSlotHandler { get { if (_miSaveSlotHandler.Count == 0) { CreateInfo<SaveSlotHandler>(PType.mi, mi: _miSaveSlotHandler); } return _miSaveSlotHandler; } }

            private static GameInput _Game_GameInput;
            public static GameInput GetGameInput()
            {
                return _Game_GameInput == null ? _Game_GameInput = GetCore()?.GameInput : _Game_GameInput;
            }

            private static AudioManager _Game_AudioManager;
            public static AudioManager GetAudioManager()
            {
                return _Game_AudioManager == null ? _Game_AudioManager = GetCore()?.AudioManager : _Game_AudioManager;
            }

            private static GameVersion _Game_GameVersion;
            public static GameVersion GetGameVersion()
            {
                return _Game_GameVersion == null ? _Game_GameVersion = GetCore()?.GameVersion : _Game_GameVersion;
            }

            private static UIManager _Game_UIManager;
            public static UIManager GetUIManager()
            {
                return _Game_UIManager == null ? _Game_UIManager = GetCore()?.UIManager : _Game_UIManager;
            }

            
            private static WorldHandler _Game_WorldHandler;
            public static WorldHandler GetWorldHandler()
            {
                return _Game_WorldHandler == null ? _Game_WorldHandler = WorldHandler.instance : _Game_WorldHandler;
            }

            private static Dictionary<string, PropertyInfo> _piWorldHandler = new Dictionary<string, PropertyInfo>();
            public static Dictionary<string, PropertyInfo> piWorldHandler { get { if (_piWorldHandler.Count == 0) { CreateInfo<WorldHandler>(PType.pi, pi: _piWorldHandler); } return _piWorldHandler; } }

            private static Dictionary<string, FieldInfo> _fiWorldHandler = new Dictionary<string, FieldInfo>();
            public static Dictionary<string, FieldInfo> fiWorldHandler { get { if (_fiWorldHandler.Count == 0) { CreateInfo<WorldHandler>(PType.fi, fi: _fiWorldHandler); } return _fiWorldHandler; } }

            private static Dictionary<string, MethodInfo> _miWorldHandler = new Dictionary<string, MethodInfo>();
            public static Dictionary<string, MethodInfo> miWorldHandler { get { if (_miWorldHandler.Count == 0) { CreateInfo<WorldHandler>(PType.mi, mi: _miWorldHandler); } return _miWorldHandler; } }

            // PLAYER //
            // --------------------------------------------------------------------------
            private static Player _Game_Player;
            public static Player GetPlayer()
            {
                return _Game_Player == null ? _Game_Player = GetWorldHandler()?.GetCurrentPlayer() : _Game_Player;
            }

            private static Dictionary<string, PropertyInfo> _piPlayer = new Dictionary<string, PropertyInfo>();
            public static Dictionary<string, PropertyInfo> piPlayer { get { if (_piPlayer.Count == 0) { CreateInfo<Player>(PType.pi, pi: _piPlayer); } return _piPlayer; } }

            private static Dictionary<string, FieldInfo> _fiPlayer = new Dictionary<string, FieldInfo>();
            public static Dictionary<string, FieldInfo> fiPlayer { get { if (_fiPlayer.Count == 0) { CreateInfo<Player>(PType.fi, fi: _fiPlayer); } return _fiPlayer; } }

            private static Dictionary<string, MethodInfo> _miPlayer = new Dictionary<string, MethodInfo>();
            public static Dictionary<string, MethodInfo> miPlayer { get { if (_miPlayer.Count == 0) { CreateInfo<Player>(PType.mi, mi: _miPlayer); } return _miPlayer; } }
            // --------------------------------------------------------------------------

            public static WallrunLineAbility GetWallrunLineAbility()
            {
                Player player = GetPlayer();
                if (player != null && TryGetValue(fiPlayer, player, "wallrunAbility", out WallrunLineAbility wallrunLineAbility)) {
                    return wallrunLineAbility;
                }
                return null;
            }

            private static Dictionary<string, PropertyInfo> _piWallrun = new Dictionary<string, PropertyInfo>();
            public static Dictionary<string, PropertyInfo> piWallrun { get { if (_piWallrun.Count == 0) { CreateInfo<WallrunLineAbility>(PType.pi, pi: _piWallrun); } return _piWallrun; } }

            private static Dictionary<string, FieldInfo> _fiWallrun = new Dictionary<string, FieldInfo>();
            public static Dictionary<string, FieldInfo> fiWallrun { get { if (_fiWallrun.Count == 0) { CreateInfo<WallrunLineAbility>(PType.fi, fi: _fiWallrun); } return _fiWallrun; } }

            private static Dictionary<string, MethodInfo> _miWallrun = new Dictionary<string, MethodInfo>();
            public static Dictionary<string, MethodInfo> miWallrun { get { if (_miWallrun.Count == 0) { CreateInfo<WallrunLineAbility>(PType.mi, mi: _miWallrun); } return _miWallrun; } }
        }
    }
}
