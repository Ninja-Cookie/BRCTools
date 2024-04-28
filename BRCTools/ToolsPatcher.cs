using HarmonyLib;
using Reptile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;
using static BRCTools.ToolsConfig;
using static BRCTools.ToolsFunctions;
using static BRCTools.ToolsGame;

namespace BRCTools
{
    internal class ToolsPatcher : MonoBehaviour
    {
        private void Awake()
        {
            var harmony = new Harmony(Plugin.pluginGuid);
            harmony.PatchAll();
        }
    }

    // Block Inputs in InputFields //
    // --------------------------------------------------------------------------
    [HarmonyPatch(typeof(Player), "SetInputs")]
    public static class SetInputs_Patch
    {
        public static bool Prefix(Player __instance)
        {
            if (!ToolsBindings.acceptKeys)
                __instance.FlushInput();

            return ToolsBindings.acceptKeys;
        }
    }
    // --------------------------------------------------------------------------

    // Skip Intro + Load To Stage //
    // --------------------------------------------------------------------------

    // Launches game with unused code to load to a stage instead of menu, likely debug.
    [HarmonyPatch(typeof(Bootstrap), "LaunchGame")]
    public static class LaunchGame_Patch
    {
        public static bool Prefix(Bootstrap __instance)
        {
            __instance.LaunchGameIntoStage(Stage.Prelude);
            return false;
        }
    }

    // Interrupts code to get save file location and load that instead & try finds steam save file location.
    [HarmonyPatch(typeof(BaseModule), "StartGameFromCurrentSaveSlotToStage")]
    public static class StartGameToStage_Patch
    {
        public static bool Prefix(ref Stage stage, BaseModule __instance, SaveManager ___saveManager)
        {
            if (___saveManager.FindLastPlayedSaveSlot() != null)
            {
                Stage lastStage = ___saveManager.FindLastPlayedSaveSlot().CurrentStage;
                List<Stage> stages = new List<Stage> { Stage.Prelude, Stage.hideout, Stage.downhill, Stage.square, Stage.tower, Stage.Mall, Stage.pyramid, Stage.osaka };

                BaseModule baseMod = Game.GetBaseModule();
                if (!stages.Contains(lastStage) && baseMod != null)
                {
                    baseMod.LoadMainMenuScene();
                    return false;
                }

                stage = lastStage;
            }
            return true;
        }
    }
    // --------------------------------------------------------------------------

    // Fixes an error from quick reloading.
    [HarmonyPatch(typeof(BaseModule), "HandleStageFullyLoaded")]
    public static class HandleStageFullyLoaded_Patch
    {
        public static bool Prefix(StageManager ___stageManager, GameInput ___gameInput)
        {
            return ___stageManager != null && ___gameInput != null ? true : false;
        }
    }

    // Disable Saving //
    // --------------------------------------------------------------------------
    [HarmonyPatch(typeof(SaveManager), "SaveCurrentSaveSlotImmediate")]
    public static class SaveCurrentSaveSlotImmediate_Patch
    {
        public static bool Prefix()
        {
            bool forceSave = false;
            if (ToolsFunctions.loadingFile)
            {
                forceSave = true;
                ToolsFunctions.loadingFile = false;
            }

            return !ToolsFunctions.Toggles.saving || forceSave;
        }
    }

    [HarmonyPatch(typeof(SaveManager), "SaveCurrentSaveSlot")]
    public static class SaveCurrentSaveSlot_Patch
    {
        public static bool Prefix()
        {
            return !ToolsFunctions.Toggles.saving;
        }
    }

    [HarmonyPatch(typeof(SaveManager), "SaveCurrentSaveSlotBackup")]
    public static class SaveCurrentSaveSlotBackup_Patch
    {
        public static bool Prefix()
        {
            return !ToolsFunctions.Toggles.saving;
        }

    }
    // --------------------------------------------------------------------------

    // Make Crime Legal //
    // --------------------------------------------------------------------------
    [HarmonyPatch(typeof(WantedManager), "ProcessCrime")]
    public static class ProcessCrime_Patch
    {
        public static bool Prefix()
        {
            return !ToolsFunctions.Toggles.police;
        }

    }
    // --------------------------------------------------------------------------

    // No Damage //
    // --------------------------------------------------------------------------
    [HarmonyPatch(typeof(Player), "ChangeHP")]
    public static class ChangeHP_Patch
    {
        public static bool Prefix(ref int dmg, ref float ___HP, float ___maxHP)
        {
            if (ToolsFunctions.Toggles.invulnerable && ___HP < ___maxHP)
            {
                ___HP = ___maxHP;
                dmg = -((int)___maxHP);
                return true;
            }

            return !ToolsFunctions.Toggles.invulnerable;
        }
    }

    [HarmonyPatch(typeof(Player), "GetHit", new Type[] { typeof(int), typeof(Vector3), typeof(KnockbackAbility.KnockbackType) })]
    public static class GetHit_Patch
    {
        public static bool Prefix()
        {
            return !ToolsFunctions.Toggles.invulnerable;
        }
    }
    // --------------------------------------------------------------------------

    // Update Player //
    // --------------------------------------------------------------------------
    [HarmonyPatch(typeof(StageManager), "DoStagePostInitialization")]
    public static class StageInit_Patch
    {
        public static void Prefix()
        {
            ToolsFunctions.used_graffiti.Clear();

            Player          player          = Game.GetPlayer();
            WorldHandler    worldHandler    = Game.GetWorldHandler();
            SaveManager     saveManager     = Game.GetSaveManager();

            if (player != null && saveManager != null && saveManager.CurrentSaveSlot != null)
            {
                if (worldHandler != null)
                {
                    PlayerSpawner spawner = worldHandler.GetRespawnPoint(player.transform.position);
                    List<PlayerSpawner> spawners = worldHandler.SceneObjectsRegister.playerSpawners;
                    Attributes.spawnIndex = spawners.FindIndexOf(spawner);

                    if (worldHandler.SceneObjectsRegister.RetrieveDreamEncounter() != null)
                    {
                        var dreamSpawners = worldHandler.SceneObjectsRegister.RetrieveDreamEncounter().checkpoints;
                        Attributes.dreamSpawnIndex = dreamSpawners.ToList().IndexOf(dreamSpawners.Last());
                    }
                    else
                    {
                        Attributes.dreamSpawnIndex = 0;
                    }
                }

                StageProgress progress = saveManager.CurrentSaveSlot.GetCurrentStageProgress();
                ToolsFunctions.Attributes.levelSpawnPos = progress.respawnPos;
                ToolsFunctions.Attributes.levelSpawnRot = progress.respawnRot;
            }

            if (player != null && Game.TryGetValue(Game.fiPlayer, player, "character", out Characters character) && Game.TryGetValue(Game.fiPlayer, player, "moveStyleEquipped", out MoveStyle moveStyle))
            {
                ToolsFunctions.Attributes.charIndex = (int)character;
                ToolsFunctions.Attributes.styleIndex = (int)moveStyle;

                if (Core.Instance.SaveManager != null && Core.Instance.SaveManager.CurrentSaveSlot != null && Core.Instance.SaveManager.CurrentSaveSlot.GetCharacterProgress(character) != null)
                    ToolsFunctions.Attributes.outfitIndex = Core.Instance.SaveManager.CurrentSaveSlot.GetCharacterProgress(character).outfit;
            }
        }
    }
    // --------------------------------------------------------------------------

    // Handle Cars //
    // --------------------------------------------------------------------------
    [HarmonyPatch(typeof(CarsMoveHandler), "UpdateCars")]
    public static class UpdateCars_Patch
    {
        public static bool Prefix(ref CarsMoveHandler __instance)
        {
            __instance.gameObject.SetActive(!ToolsFunctions.Toggles.cars);

            return !ToolsFunctions.Toggles.cars;
        }
    }
    // --------------------------------------------------------------------------

    // Menu Stuff //
    // --------------------------------------------------------------------------
    [HarmonyPatch(typeof(AMenuController), "OnScreenSizeChanged")]
    public static class DetectScreenSizeChange_Patch
    {
        public static void Prefix()
        {
            ToolsGUI.Instance.UpdateMenuSize();
        }
    }

    [HarmonyPatch(typeof(GameInput), "SetCursorVisibility")]
    public static class SetCursorVisibility_Patch
    {
        public static bool Prefix()
        {
            return ToolsGUI.Instance.menuBlocked;
        }
    }
    // --------------------------------------------------------------------------

    // Auto Graffiti //
    // --------------------------------------------------------------------------
    [HarmonyPatch(typeof(GraffitiGame), "Init")]
    public static class Init_Patch
    {
        public static void Postfix(GraffitiGame __instance, ref List<int> ___targetsHitSequence, ref bool ___reloadedSpray, GraffitiSpot ___gSpot)
        {
            if (___gSpot.size == GraffitiSize.S || !ToolsFunctions.Toggles.autograf)
                return;

            object[] totalTargets = (object[])__instance.GetType().GetField("targets", Game.Instance.bindingFlags).GetValue(__instance);

            foreach (var item in totalTargets) { ___targetsHitSequence.Add(0); }

            Game.Instance.InvokeMethod(__instance, "SetState", new object[] { GraffitiGame.GraffitiGameState.COMPLETE_TARGETS });
        }
    }

    [HarmonyPatch(typeof(GraffitiArtInfo), "FindBySequence")]
    public static class FindBySequence_Patch
    {
        public static bool Prefix(GraffitiArtInfo __instance, List<int> graffitiGameHitSequence, ref GraffitiArt __result)
        {
            GraffitiSize size = graffitiGameHitSequence.Count() % 4 + GraffitiSize.M;
            if (size == GraffitiSize.S || !ToolsFunctions.Toggles.autograf)
                return true;

            List<GraffitiArt> list = __instance.FindBySize(size);
            List<GraffitiArt> possibleArt = ToolsConfig.Settings.shouldUseAllGraffitis ? list : list.Where((GraffitiArt x) => Core.Instance.Platform.User.GetUnlockableSaveDataFor(x.unlockable).IsUnlocked).ToList();

            GraffitiArt graffiti_to_use = null;
            foreach (var art in possibleArt)
            {
                if (!ToolsFunctions.used_graffiti.Contains(art))
                {
                    graffiti_to_use = art;
                    break;
                }
            }

            if (graffiti_to_use == null)
            {
                foreach (var art in possibleArt)
                    ToolsFunctions.used_graffiti.Remove(art);

                graffiti_to_use = possibleArt.Count() > 0 ? possibleArt[0] : list[0];
            }

            if (graffiti_to_use != null && !ToolsFunctions.used_graffiti.Contains(graffiti_to_use))
                ToolsFunctions.used_graffiti.Add(graffiti_to_use);

            __result = graffiti_to_use;
            return false;
        }
    }
    // --------------------------------------------------------------------------

    // Cutscene Skip - Old Ugly System But Works //
    // --------------------------------------------------------------------------
    [HarmonyPatch(typeof(SequenceHandler), "UpdateSequenceHandler")]
    public static class UpdateSequenceHandler_Patch
    {
        public static bool Prefix(SequenceHandler __instance)
        {
            if (!ToolsConfig.Settings.shouldAllowCutsceneSkip)
                return true;

            if (Game.CompatibilityFunctions.GetFieldValue(__instance, "sequence") != null)
            {
                if (Game.CompatibilityFunctions.GetFieldValue(__instance, "sequence").ToString().Split(' ')[0] != "ChangeOutfitSequence")
                {
                    if (Settings.speedUpCutsceneSkip)
                    {
                        Game.CompatibilityFunctions.SetFieldValue(__instance, "skipFadeDuration", 0f);
                        Game.CompatibilityFunctions.SetFieldValue(__instance, "skipStartTimer", 1.5f);
                    }

                    if (Game.CompatibilityFunctions.GetFieldValue(__instance, "skipTextActiveState").ToString() == "NOT_SKIPPABLE")
                        Game.CompatibilityFunctions.SetFieldValue(__instance, "skipTextActiveState", Game.CompatibilityFunctions.GetFieldValue(new SequenceHandler(), "skipTextActiveState"));
                }
            }
            return true;
        }
    }
    // --------------------------------------------------------------------------

    // Debug

    /*
    [HarmonyPatch(typeof(Player), "JumpIsAllowed")]
    public static class JumpIsAllowed_Patch
    {
        public static bool Prefix(ref bool __result)
        {
            __result = true;
            return false;
        }
    }
    */

    /*
    [HarmonyPatch(typeof(Player), "OnCollisionEnter")]
    public static class OnCollisionEnter_Patch
    {
        public static void Prefix(Collision other, bool ___isAI)
        {
            if (!___isAI)
            {
                Debug.Log($"Collider: {other} // Name: {other.gameObject.name} // Tag: {other.gameObject.tag} // Has: {other.gameObject.GetComponent<ToolsFunctions.TriggerRender>() != null}");
                Debug.Log("---------");
                foreach (var item in other.gameObject.GetComponents<Component>())
                {
                    Debug.Log(item);
                }
                Debug.Log("");
                try { Debug.Log(other.gameObject.GetComponent<MeshCollider>().enabled); } catch { }
                try { Debug.Log(other.gameObject.GetComponent<MeshRenderer>().material.name); } catch { }
                try { Debug.Log(other.gameObject.GetComponent<MeshRenderer>().material.color); } catch { }
                try { Debug.Log(other.gameObject.GetComponent<MeshRenderer>().material.shader); } catch { }
                try { Debug.Log(other.gameObject.GetHashCode().ToString()); } catch { }
            }
        }
    }
    */


    /*
    [HarmonyPatch(typeof(Player), "OnTriggerEnter")]
    public static class OnTriggerEnter_Patch
    {
        public static void Prefix(Collider other, bool ___isAI)
        {
            if (!___isAI)
            {
                Debug.Log("--------");
                Debug.Log(other);
                Debug.Log("--------");
                foreach (var item in other.GetComponents<Component>())
                {
                    Debug.Log(item);
                    Debug.Log(item.tag);
                    Debug.Log("");
                }
            }
        }

        public static void Postfix(ref Collider other)
        {
            Debug.Log(other.tag);
        }
    }
    */

}

