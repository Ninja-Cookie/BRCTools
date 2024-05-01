using Reptile;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using static BRCTools.ToolsGame;
using static BRCTools.ToolsRef;

namespace BRCTools
{
    internal class ToolsFunctions : MonoBehaviour
    {
        public static ToolsFunctions Instance;

        public ToolsFunctions()
        {
            Instance = this;
        }

        public class Toggles : MonoBehaviour
        {
            private static bool _invulnerable = false;
            public static bool invulnerable { get { return _invulnerable; } set { _invulnerable = value; toolsGUI.UpdateLinks(); } }

            private static bool _police = false;
            public static bool police { get { return _police; } set { _police = value; toolsGUI.UpdateLinks(); } }

            private static bool _infBoost = false;
            public static bool infBoost { get { return _infBoost; } set { _infBoost = value; toolsGUI.UpdateLinks(); } }

            private static bool _tfps = false;
            public static bool tfps { get { return _tfps; } set { _tfps = value; toolsGUI.UpdateLinks(); } }

            private static bool _timescale = false;
            public static bool timescale { get { return _timescale; } set { _timescale = value; toolsGUI.UpdateLinks(); } }

            private static bool _noclip = false;
            public static bool noclip { get { return _noclip; } set { _noclip = value; toolsGUI.UpdateLinks(); } }

            private static bool _fly = false;
            public static bool fly { get { return _fly; } set { _fly = value; toolsGUI.UpdateLinks(); } }

            private static bool _triggers = false;
            public static bool triggers { get { return _triggers; } set { _triggers = value; toolsGUI.UpdateLinks(); } }

            private static bool _saving = false;
            public static bool saving { get { return _saving; } set { _saving = value; toolsGUI.UpdateLinks(); } }

            private static bool _cars = false;
            public static bool cars { get { return _cars; } set { _cars = value; toolsGUI.UpdateLinks(); } }

            private static bool _autograf = false;
            public static bool autograf { get { return _autograf; } set { _autograf = value; toolsGUI.UpdateLinks(); } }
        }

        public class Attributes : MonoBehaviour
        {
            private static Vector3 _savedPos = Vector3.zero;
            public static float savedPosX { get { return _savedPos.x; } set { _savedPos.x = value; toolsGUI.UpdateFields(nameof(savedPosX), value); } }
            public static float savedPosY { get { return _savedPos.y; } set { _savedPos.y = value; toolsGUI.UpdateFields(nameof(savedPosY), value); } }
            public static float savedPosZ { get { return _savedPos.z; } set { _savedPos.z = value; toolsGUI.UpdateFields(nameof(savedPosZ), value); } }
            public static Vector3 savedPos { get { return _savedPos; } set { savedPosX = value.x; savedPosY = value.y; savedPosZ = value.z; } }

            private static float _savedStorage = 0f;
            public static float savedStorage { get { return _savedStorage; } set { _savedStorage = value; toolsGUI.UpdateFields(nameof(savedStorage), value); } }

            private static float _savedBoost = 0f;
            public static float savedBoost { get { return _savedBoost; } set { _savedBoost = Mathf.Round(value * 100) / 100; toolsGUI.UpdateFields(nameof(savedBoost), _savedBoost); } }

            private static float _savedSpeed = 0f;
            public static float savedSpeed { get { return _savedSpeed; } set { _savedSpeed = value; toolsGUI.UpdateFields(nameof(savedSpeed), value); } }

            private static Ability _savedAbility = null;
            public static Ability savedAbility { get { return _savedAbility; } set { _savedAbility = value; } }

            private static bool _savedHasBoost = true;
            public static bool savedHasBoost { get { return _savedHasBoost; } set { _savedHasBoost = value; } }

            private static bool _savedHasDash = true;
            public static bool savedHasDash { get { return _savedHasDash; } set { _savedHasDash = value; } }

            private static int _savedAnim = -1;
            public static int savedAnim { get { return _savedAnim; } set { _savedAnim = value; } }

            private static Vector3 _savedSpeedVector = Vector3.zero;
            public static Vector3 savedSpeedVector { get { return _savedSpeedVector; } set { _savedSpeedVector = value; } }

            private static Vector3 _savedVelDir = Vector3.zero;
            public static Vector3 savedVelDir { get { return _savedVelDir; } set { _savedVelDir = value; } }

            private static Quaternion _savedRot = Quaternion.identity;
            public static Quaternion savedRot { get { return _savedRot; } set { _savedRot = value; } }

            private static MoveStyle _savedMovestyleCurrent = MoveStyle.ON_FOOT;
            public static MoveStyle savedMovestyleCurrent { get { return _savedMovestyleCurrent; } set { _savedMovestyleCurrent = value; } }

            private static MoveStyle _savedMovestyleEquip = MoveStyle.ON_FOOT;
            public static MoveStyle savedMovestyleEquip { get { return _savedMovestyleEquip; } set { _savedMovestyleEquip = value; } }

            private static bool _savedEquipmentUsing = false;
            public static bool savedEquipmentUsing { get { return _savedEquipmentUsing; } set { _savedEquipmentUsing = value; } }

            private static int _charIndex = (int)Characters.metalHead;
            public static int charIndex { get { return _charIndex; } set { _charIndex = value; toolsGUI.UpdateSelectors(toolsFunctions.FuncChangePlayer, toolsFunctions.CharToName(value)); } }

            private static int _styleIndex = (int)MoveStyle.SKATEBOARD;
            public static int styleIndex { get { return _styleIndex; } set { _styleIndex = value; toolsGUI.UpdateSelectors(toolsFunctions.FuncChangeStyle, toolsFunctions.StyleToName(value)); } }

            private static int _outfitIndex = 0;
            public static int outfitIndex { get { return _outfitIndex; } set { _outfitIndex = value; toolsGUI.UpdateSelectors(toolsFunctions.FuncChangeOutfit, toolsFunctions.OutfitToName(value)); } }

            private static int _levelIndex = 0;
            public static int levelIndex { get { return _levelIndex; } set { _levelIndex = value; toolsGUI.UpdateSelectors(toolsFunctions.FuncChangeLevel, toolsFunctions.LevelToName(value)); } }

            private static int _saveIndex = 0;
            public static int saveIndex { get { return _saveIndex; } set { _saveIndex = value; toolsGUI.UpdateSelectors(toolsFunctions.FuncChangeSave, toolsFunctions.SaveToName(value)); } }

            private static int _spawnIndex = 0;
            public static int spawnIndex { get { return _spawnIndex; } set { _spawnIndex = value; toolsGUI.UpdateSelectors(toolsFunctions.FuncChangeSpawn, toolsFunctions.SpawnToName(value)); } }

            private static int _dreamSpawnIndex = 0;
            public static int dreamSpawnIndex { get { return _dreamSpawnIndex; } set { _dreamSpawnIndex = value; toolsGUI.UpdateSelectors(toolsFunctions.FuncChangeDreamSpawn, toolsFunctions.DreamSpawnToName(value)); } }

            private static int _framerate = 30;
            public static int framerate { get { return _framerate; } set { _framerate = Mathf.Max(value, 1); toolsGUI.UpdateFields(nameof(framerate), Mathf.RoundToInt(_framerate)); } }

            private static float _timescale = 0.1f;
            public static float timescale { get { return _timescale; } set { _timescale = Mathf.Max(value, 0.01f); toolsGUI.UpdateFields(nameof(timescale), _timescale); } }

            private static float _noclip = 50f;
            public static float noclip { get { return _noclip; } set { _noclip = Mathf.Max(value, 0); toolsGUI.UpdateFields(nameof(noclip), value); } }

            private static float _fly = 25f;
            public static float fly { get { return _fly; } set { _fly = Mathf.Max(value, 0); toolsGUI.UpdateFields(nameof(fly), value); } }

            private static Vector3 _levelSpawnPos = Vector3.zero;
            public static Vector3 levelSpawnPos { get { return _levelSpawnPos; } set { _levelSpawnPos = value; } }

            private static Vector3 _levelSpawnRot = Vector3.zero;
            public static Vector3 levelSpawnRot { get { return _levelSpawnRot; } set { _levelSpawnRot = value; } }
        }

        public void UpdateMenuInfo()
        {
            foreach (var prop in typeof(Attributes).GetProperties(BindingFlags.Static | BindingFlags.Public))
            {
                prop.SetValue(prop, prop.GetValue(prop));
            }
        }

        private delegate void ActiveFunctions();
        private ActiveFunctions activeFunctions;

        private delegate void ActiveFunctionsFixed();
        private ActiveFunctions activeFunctionsFixed;

        private delegate void ActiveFlyNoclipFixed();
        private ActiveFunctions activeFlyNoclipFixed;

        public Text errorMessage;

        private CancellationTokenSource tokenSrc;

        private void ShowError(string error) { tokenSrc?.Cancel(); Task.Run(() => ShowErrorTask(error)); }
        private async Task ShowErrorTask(string error)
        {
            tokenSrc = new CancellationTokenSource();

            if (errorMessage != null)
            {
                string msg = $"Error: {error}...";
                if (error == "Save Created")
                    msg = "Save Created";

                errorMessage.text = msg;

                errorMessage.gameObject.SetActive(true);
                await Task.Delay(TimeSpan.FromSeconds(3), tokenSrc.Token);
                errorMessage.gameObject.SetActive(false);
            }
        }

        private enum ErrorType
        {
            PLAYER_NOT_FOUND,
            BAD_CONDS,
            NO_ACTIVE_GAME,
            NO_SAVE,
            NO_SAVESLOT,
            SAVE_FILE_CORRUPT,
            FATEL,
            SAVE_CREATED
        }

        private void Error(ErrorType error, params string[] conds)
        {
            switch(error)
            {
                case ErrorType.PLAYER_NOT_FOUND:
                    ShowError("Player Not Found");
                break;

                case ErrorType.BAD_CONDS:
                    string cond = string.Join(", ", conds);
                    ShowError($"Coniditon(s) Not Met: {cond}");
                break;

                case ErrorType.NO_ACTIVE_GAME:
                    ShowError($"No Active Game Found");
                break;

                case ErrorType.NO_SAVE:
                    ShowError($"No Save Found");
                break;

                case ErrorType.NO_SAVESLOT:
                    ShowError("No Active Save Slot Found");
                break;

                case ErrorType.SAVE_FILE_CORRUPT:
                    ShowError("Save File Could Not Be Loaded");
                break;

                case ErrorType.FATEL:
                    ShowError("Something Went Wrong That Shouldn't");
                break;

                case ErrorType.SAVE_CREATED:
                    ShowError("Save Created");
                break;
            }
        }

        public void Update()
        {
            activeFunctions?.Invoke();
        }

        public void FixedUpdate()
        {
            activeFunctionsFixed?.Invoke();
            activeFlyNoclipFixed?.Invoke();
        }

        public void FuncRefill()
        {
            Player player = Game.GetPlayer();
            if (player != null && Game.TryGetValue(Game.fiPlayer, player, "maxBoostCharge", out float result))
            {
                player.AddBoostCharge(result);
            }
            else
            {
                if (player == null)
                    Error(ErrorType.PLAYER_NOT_FOUND);
                else
                    Error(ErrorType.BAD_CONDS, "Max Boost Not Found");
            }
        }

        public void FuncInfBoost() { ToggleInfBoost(); }
        private void ToggleInfBoost(bool forceOff = false) { if (forceOff ? Toggles.infBoost = false : Toggles.infBoost = !Toggles.infBoost) activeFunctions += UpdateInfBoost; else activeFunctions -= UpdateInfBoost; }
        private void UpdateInfBoost()
        {
            Player player = Game.GetPlayer();
            if (player != null && Game.TryGetValue(Game.fiPlayer, player, "maxBoostCharge", out float result))
                player.boostCharge = result;
        }

        public void FuncResetGraf()
        {
            WorldHandler worldHandler = Game.GetWorldHandler();
            SaveManager saveManager = Game.GetSaveManager();
            if (worldHandler != null && worldHandler.SceneObjectsRegister != null) {
                foreach (var graf in worldHandler.SceneObjectsRegister.grafSpots)
                {
                    if (graf.isOpen)
                    {
                        graf.ResetFirstTime();

                        if (graf.topCrew == Crew.PLAYERS)
                            graf.topCrew = Crew.NONE;

                        if (graf.bottomCrew == Crew.PLAYERS)
                            graf.bottomCrew = Crew.NONE;

                        graf.SetDefaultData();

                        graf.SetState(GraffitiState.NONE);

                        graf.WriteToData();
                    }
                }

                Player player = Game.GetPlayer();
                if (player != null)
                {
                    Game.TrySetValue<float>(Game.fiPlayer, player, "rep", 0f);
                    Game.invoke(player, "SetRepLabel", new object[] { 0f }, Game.miPlayer);

                    foreach (var rep in FindObjectsOfType<GameObject>().Where(x => x.GetComponent<DynamicRepPickup>()))
                        Destroy(rep);
                }

                if (saveManager != null && saveManager.CurrentSaveSlot != null && saveManager.CurrentSaveSlot.GetCurrentStageProgress() != null)
                {
                    saveManager.CurrentSaveSlot.GetCurrentStageProgress().reputation = 0;
                    saveManager.CurrentSaveSlot.SetCurrentStageProgress(saveManager.CurrentSaveSlot.CurrentStage);
                    saveManager.SaveCurrentSaveSlotImmediate();
                }
                else
                {
                    Error(ErrorType.FATEL);
                }
            }
            else
            {
                Error(ErrorType.NO_ACTIVE_GAME);
            }
        }

        public void FuncResLevel()
        {
            Player player = Game.GetPlayer();
            BaseModule baseMod = Game.GetBaseModule();
            SaveManager saveManager = Game.GetSaveManager();
            if (player != null && baseMod != null && saveManager != null && saveManager.CurrentSaveSlot != null)
            {
                StageProgress progress = saveManager.CurrentSaveSlot.GetCurrentStageProgress();
                progress.respawnPos = Attributes.levelSpawnPos;
                progress.respawnRot = Attributes.levelSpawnRot;
                saveManager.SaveCurrentSaveSlot();

                baseMod.UnPauseGame(PauseType.ForceOff);
                baseMod.StageManager.ExitCurrentStage(baseMod.CurrentStage, Stage.NONE);
            }
            else
            {
                Error(ErrorType.NO_ACTIVE_GAME);
            }
        }

        public void FuncRefillHP()
        {
            Player player = Game.GetPlayer();
            if (player != null)
            {
                if (Game.TryGetValue(Game.fiPlayer, player, "maxHP", out float maxHP))
                    Game.TrySetValue(Game.fiPlayer, player, "HP", maxHP);
            }
            else
            {
                Error(ErrorType.PLAYER_NOT_FOUND);
            }
        }

        public void FuncInvulnerable() { ToggleInvulnerable(); }
        public void ToggleInvulnerable(bool forceOff = false) { if (forceOff ? Toggles.invulnerable = false : Toggles.invulnerable = !Toggles.invulnerable) activeFunctions += UpdateInvulnerable; else activeFunctions -= UpdateInvulnerable; }
        private void UpdateInvulnerable()
        {
            Player player = Game.GetPlayer();
            if (player != null)
            {
                if (Game.TryGetValue(Game.fiPlayer, player, "maxHP", out float maxHP))
                    Game.TrySetValue(Game.fiPlayer, player, "HP", maxHP);
            }
        }

        public void FuncPolice() { TogglePolice(); }
        private void TogglePolice(bool forceOff = false) { if (forceOff ? Toggles.police = false : Toggles.police = !Toggles.police) activeFunctions += UpdatePolice; else activeFunctions -= UpdatePolice; }
        private void UpdatePolice()
        {
            if (WantedManager.instance != null)
                WantedManager.instance.StopPlayerWantedStatus(false);
        }

        public void FuncDisCars() { ToggleDisCars(); }
        private void ToggleDisCars(bool forceOff = false) { if (forceOff ? Toggles.cars = false : Toggles.cars = !Toggles.cars) activeFunctions += UpdateDisCars; else activeFunctions -= UpdateDisCars; }
        private void UpdateDisCars()
        {
            // NOT NEEDED
        }

        private static int prevVSync= QualitySettings.vSyncCount;
        private static int prevFPS = -1;
        public void FuncLockFPS() { prevVSync = QualitySettings.vSyncCount; ToggleLockFPS(); }
        private void ToggleLockFPS(bool forceOff = false) { if (forceOff ? Toggles.tfps = false : Toggles.tfps = !Toggles.tfps) { activeFunctions += UpdateLockFPS; } else { QualitySettings.vSyncCount = prevVSync; Application.targetFrameRate = prevFPS; activeFunctions -= UpdateLockFPS; } }
        private void UpdateLockFPS()
        {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = Attributes.framerate;
        }

        public void FuncSetTimescale() { ToggleSetTimescale(); }
        private void ToggleSetTimescale(bool forceOff = false) { if (forceOff ? Toggles.timescale = false : Toggles.timescale = !Toggles.timescale) { activeFunctions += UpdateSetTimescale; } else { Time.timeScale = 1f; activeFunctions -= UpdateSetTimescale; } }
        private void UpdateSetTimescale()
        {
            Time.timeScale = Attributes.timescale;
        }

        public void FuncNoclip() { ToggleNoclip(); }
        private void ToggleNoclip(bool forceOff = false)
        {
            Player player = Game.GetPlayer();

            if (player != null)
            {
                if (forceOff ? Toggles.noclip = false : Toggles.noclip = !Toggles.noclip)
                {
                    if (Toggles.fly)
                        Toggles.fly = false;

                    activeFlyNoclipFixed = UpdateFlyNoclip;
                }
                else if (!Toggles.fly)
                {
                    EndFlyNoclip();
                }
            }
            else
            {
                EndFlyNoclip();
            }
        }

        public void FuncFly() { ToggleFly(); }
        private void ToggleFly(bool forceOff = false)
        {
            Player player = Game.GetPlayer();

            if (player != null)
            {
                if (forceOff ? Toggles.fly = false : Toggles.fly = !Toggles.fly)
                {
                    if (Toggles.noclip)
                        Toggles.noclip = false;

                    activeFlyNoclipFixed = UpdateFlyNoclip;
                }
                else if (!Toggles.noclip)
                {
                    EndFlyNoclip();
                }
            }
            else
            {
                EndFlyNoclip();
            }
        }
        
        private void UpdateFlyNoclip()
        {
            Player      player      = Game.GetPlayer();
            GameInput   gameInput   = Game.GetGameInput();

            if ((Toggles.noclip || Toggles.fly) && player != null && gameInput != null)
            {
                if (Toggles.noclip)
                {
                    player.GetComponent<Collider>().enabled = false;
                    player.interactionCollider.enabled      = false;
                }
                else
                {
                    player.GetComponent<Collider>().enabled = true;
                    player.interactionCollider.enabled      = true;
                }

                if (Camera.main != null) { Camera.main.farClipPlane = 20000f; }

                Game.TrySetValue(Game.fiPlayer, player, "userInputEnabled", false);

                if (Game.TryGetValue(Game.fiWorldHandler, Game.GetWorldHandler(), "currentCameraTransform", out Transform cameraMode))
                {
                    Vector3 velocity = Vector3.zero;

                    float targetSpeed = Toggles.fly ? Attributes.fly : Attributes.noclip;
                    float finalFlySpeedForward = targetSpeed;
                    float finalFlySpeedRight = targetSpeed;

                    player.CompletelyStop();

                    if (gameInput != null)
                    {

                        Vector3 hAxis = gameInput.GetAxis(5, 0) * cameraMode.right * targetSpeed;
                        Vector3 vAxis = gameInput.GetAxis(6, 0) * Vector3.Normalize(new Vector3(cameraMode.forward.x, 0f, cameraMode.forward.z)) * targetSpeed;
                        Vector3 axis = hAxis + vAxis;
                        axis.y = 0f;

                        if (axis.magnitude > targetSpeed)
                            axis = axis.normalized * targetSpeed;

                        velocity = axis;

                        if (velocity != Vector3.zero) { player.motor.rotation = Quaternion.Euler(new Vector3(0f, cameraMode.eulerAngles.y, cameraMode.eulerAngles.z)); }
                    }

                    if (Input.GetKey(KeyCode.Space) || (gameInput != null && gameInput.GetButtonHeld(7, 0)))
                    {
                        if (player.IsGrounded())
                        {
                            player.motor.ForcedUnground();
                            player.Jump();
                        }
                        velocity.y = 20;
                    }
                    else if (Input.GetKey(KeyCode.LeftControl) || (gameInput != null && gameInput.GetButtonHeld(65, 0)))
                    {
                        velocity.y = -20;
                    }
                    else
                    {
                        velocity.y = 0.00f;
                    }

                    player.SetVelocity(velocity);
                }
            }
            else
            {
                EndFlyNoclip();
            }
        }

        private void EndFlyNoclip()
        {
            if (Toggles.fly)
                Toggles.fly = false;

            if (Toggles.noclip)
                Toggles.noclip = false;

            activeFlyNoclipFixed = null;

            Player player = Game.GetPlayer();
            if (player != null)
            {
                if (Camera.main != null) { Camera.main.farClipPlane = 1000f; }
                Game.TrySetValue(Game.fiPlayer, player, "userInputEnabled", true);

                player.interactionCollider.enabled = true;
                player.GetComponent<Collider>().enabled = true;
            }
        }

        public void FuncDisSaving() { ToggleDisSaving(); }
        private void ToggleDisSaving(bool forceOff = false) { if (forceOff ? Toggles.saving = false : Toggles.saving = !Toggles.saving) activeFunctions += UpdateDisSaving; else activeFunctions -= UpdateDisSaving; }
        private void UpdateDisSaving()
        {
            // NOT REQUIRED -- FOR NOW
        }

        public void FuncEndWanted()
        {
            if (WantedManager.instance != null)
                WantedManager.instance.StopPlayerWantedStatus(false);
            else
                Error(ErrorType.NO_ACTIVE_GAME);
        }

        public void FuncSetStorage()
        {
            SetStorageSpeed();
        }
        private void SetStorageSpeed()
        {
            WallrunLineAbility wallrunLine = Game.GetWallrunLineAbility();
            if (wallrunLine == null || !Game.TrySetValue(Game.fiWallrun, wallrunLine, "lastSpeed", savedData.storageSpeed)) { Error(wallrunLine == null ? ErrorType.PLAYER_NOT_FOUND : ErrorType.FATEL); }
        }

        public float GetStorageSpeed()
        {
            WallrunLineAbility wallrunLine = Game.GetWallrunLineAbility();
            if (wallrunLine != null && Game.TryGetValue(Game.fiWallrun, wallrunLine, "lastSpeed", out float speed))
            {
                return speed;
            }
            return 0f;
        }

        public string FuncChangePlayer(int i)
        {
            int newCharIndex        = Attributes.charIndex + i;
            Attributes.charIndex    = newCharIndex < (int)Characters.girl1 ? (int)Characters.MAX - 1 : newCharIndex >= (int)Characters.MAX ? (int)Characters.girl1 : newCharIndex;

            Player player = Game.GetPlayer();
            if (player != null && Game.TryGetValue(Game.fiPlayer, player, "usingEquippedMovestyle", out bool usingEquippedMovestyle))
            {
                player.SetCharacter((Characters)Attributes.charIndex, Attributes.outfitIndex);
                RefreshPlayer();
                player.SwitchToEquippedMovestyle(usingEquippedMovestyle, showEffect: false);

                if (Core.Instance.SaveManager != null && Core.Instance.SaveManager.CurrentSaveSlot != null && Core.Instance.SaveManager.CurrentSaveSlot.GetCharacterProgress((Characters)Attributes.charIndex) != null)
                    Core.Instance.SaveManager.CurrentSaveSlot.GetCharacterProgress((Characters)Attributes.charIndex).character = (Characters)Attributes.charIndex;
            }

            return CharToName(Attributes.charIndex);
        }
        public string CharToName(int i)
        {
            string charName = ((Characters)i).ToString();

            if (toolsConfig.charToName.TryGetValue(charName, out string result))
                return result;
            return charName;
        }

        public string FuncChangeStyle(int i)
        {
            int newStyleIndex = Attributes.styleIndex + i;
            int max = ToolsConfig.Settings.shouldHaveSpecial ? (int)MoveStyle.MAX : (int)MoveStyle.SPECIAL_SKATEBOARD;
            Attributes.styleIndex = newStyleIndex < (int)MoveStyle.BMX ? max - 1 : newStyleIndex >= max ? (int)MoveStyle.BMX : newStyleIndex;

            Player player = Game.GetPlayer();
            if (player != null && Game.TryGetValue(Game.fiPlayer, player, "usingEquippedMovestyle", out bool usingEquippedMovestyle))
            {
                player.SetCurrentMoveStyleEquipped((MoveStyle)Attributes.styleIndex);
                RefreshPlayer();
                player.SwitchToEquippedMovestyle(usingEquippedMovestyle, showEffect: false);

                if (Core.Instance.SaveManager != null && Core.Instance.SaveManager.CurrentSaveSlot != null && Core.Instance.SaveManager.CurrentSaveSlot.GetCharacterProgress((Characters)Attributes.charIndex) != null)
                    Core.Instance.SaveManager.CurrentSaveSlot.GetCharacterProgress((Characters)Attributes.charIndex).moveStyle = (MoveStyle)Attributes.styleIndex;
            }

            return StyleToName(Attributes.styleIndex);
        }
        public string StyleToName(int i)
        {
            string styleName = ((MoveStyle)i).ToString();

            if (toolsConfig.styleToName.TryGetValue(styleName, out string result))
                return result;
            return styleName;
        }

        public string FuncChangeOutfit(int i)
        {
            int newOutfitIndex = Attributes.outfitIndex + i;
            int max = toolsConfig.outfitToName.Keys.Count - 1;
            Attributes.outfitIndex = newOutfitIndex < 0 ? max : newOutfitIndex > max ? 0 : newOutfitIndex;

            Player player = Game.GetPlayer();
            if (player != null)
                player.SetOutfit(Attributes.outfitIndex);

            return OutfitToName(Attributes.outfitIndex);
        }
        public string OutfitToName(int i)
        {
            if (toolsConfig.outfitToName.TryGetValue(i, out string result))
                return result;
            return i.ToString();
        }

        public string FuncChangeLevel(int i)
        {
            int newLevelIndex = Attributes.levelIndex + i;
            int max = toolsConfig.levelToName.Keys.Count - 1;
            Attributes.levelIndex = newLevelIndex < 0 ? max : newLevelIndex > max ? 0 : newLevelIndex;
            return LevelToName(Attributes.levelIndex);
        }
        public string LevelToName(int i)
        {
            if (toolsConfig.levelToName.TryGetValue(toolsConfig.levelToName.Keys.ToArray()[i], out string result))
                return result;
            return i.ToString();
        }

        private void UpdateSaveFiles()
        {
            if (Directory.Exists(ToolsConfig.folder_saves) && File.Exists(ToolsConfig.file_desc))
            {
                string[] files = Directory.GetFiles(ToolsConfig.folder_saves).Where(x => x.EndsWith(ToolsConfig.ext_saves)).ToArray();
                if ((files.Count() != ToolsConfig.files_saves.Keys.Count()) || !files.All(x => ToolsConfig.files_saves.Keys.Contains(x)) || !File.ReadAllLines(ToolsConfig.file_desc).ToHashSet<string>().SequenceEqual(ToolsConfig.desc_contents))
                {
                    ToolsConfig.Instance.UpdateSaveFiles();
                }
            }
            else
            {
                ToolsConfig.files_saves.Clear();
            }
        }

        public string FuncChangeSave(int i)
        {
            UpdateSaveFiles();

            int newSaveIndex = Attributes.saveIndex + i;
            int max = ToolsConfig.files_saves.Keys.Count - 1;
            Attributes.saveIndex = newSaveIndex < 0 ? Mathf.Max(max, 0) : newSaveIndex > max ? 0 : newSaveIndex;
            return SaveToName(Attributes.saveIndex);
        }
        public string SaveToName(int i)
        {
            if (ToolsConfig.files_saves.Count > 0 && ToolsConfig.files_saves.TryGetValue(ToolsConfig.files_saves.Keys.ToArray()[i], out string result))
                return result;
            return i.ToString();
        }

        public static bool loadingFile = false;
        public void FuncLoadSaveFile()
        {
            if (ToolsConfig.files_saves.Count > 0)
            {
                BaseModule      baseMod         = Game.GetBaseModule();
                SaveManager     saveManager     = Game.GetSaveManager();
                SaveSlotHandler saveSlotHandler = Game.GetSaveSlotHandler();
                string          path            = Path.Combine(ToolsConfig.folder_saves, ToolsConfig.files_saves.Keys.ToArray()[Attributes.saveIndex]);

                if (File.Exists(path) && baseMod != null && saveManager != null && saveManager.CurrentSaveSlot != null && Game.GetPlayer() != null && saveSlotHandler != null)
                {
                    try
                    {
                        using (BinaryReader dataRead = new BinaryReader(File.Open(path, FileMode.Open, FileAccess.Read, FileShare.None)))
                        {
                            SaveSlotData saveSlotData = new SaveSlotData();
                            saveSlotData.Read(dataRead);

                            saveSlotData.saveSlotId = saveManager.CurrentSaveSlot.saveSlotId;

                            if (Game.TrySetValue(Game.fiSaveSlotHandler, saveSlotHandler, "currentSaveSlot", saveSlotData))
                            {
                                loadingFile = true; saveManager.SaveCurrentSaveSlotImmediate();
                                baseMod.UnPauseGame(PauseType.ForceOff);
                                baseMod.UnloadCurrentStage();
                                Core.Instance.Platform.User.LogOut();

                                if (Game.TryGetValue(Game.fiBaseMod, baseMod, "currentSceneSetupInstructions", out List<ASceneSetupInstruction> currentSceneSetupInstructions))
                                {
                                    for (int i = currentSceneSetupInstructions.Count - 1; i >= 0; i--)
                                    {
                                        Game.invoke(baseMod, "StopLoadInstruction", new object[] { currentSceneSetupInstructions[i] }, Game.miBaseMod);
                                    }
                                }

                                baseMod.StartGameToStage(Stage.Prelude);
                            }
                        }
                    }
                    catch (Exception e) { Debug.LogError(e); Error(ErrorType.SAVE_FILE_CORRUPT); }
                }
                else
                {
                    if (!File.Exists(path))
                        Error(ErrorType.NO_SAVE);
                    else if (Game.GetPlayer() == null)
                        Error(ErrorType.NO_SAVESLOT);
                    else
                        Error(ErrorType.FATEL);
                }
            }
            else
            {
                Error(ErrorType.NO_SAVE);
            }
        }

        private string GetCurrentFileName(SaveSlotHandler saveSlotHandler, int i)
        {
            if (saveSlotHandler != null && saveSlotHandler.CurrentSaveSlot != null && Game.GetPlayer() != null)
                return $"{((int)Story.GetCurrentObjectiveInfo().chapter).ToString("D3")}-{i.ToString("D4")}-{saveSlotHandler.CurrentSaveSlot.CurrentStoryObjective}";
            return $"no_file_name-{i}";
        }

        public void FuncCreateSaveFile()
        {
            SaveSlotHandler saveSlotHandler = Game.GetSaveSlotHandler();

            if (saveSlotHandler != null && saveSlotHandler.CurrentSaveSlot != null && Game.GetPlayer() != null)
            {
                int i = ToolsConfig.files_saves.Count();
                string fileName = GetCurrentFileName(saveSlotHandler, i);
                string path = Path.Combine(ToolsConfig.folder_saves, fileName);

                bool pass = true;
                if (File.Exists($"{path}{ToolsConfig.ext_saves}"))
                {
                    string originalFileName = fileName;
                    while (File.Exists(Path.Combine(ToolsConfig.folder_saves, $"{fileName}{ToolsConfig.ext_saves}")))
                    {
                        i++;

                        if (i > 9999)
                        {
                            pass = false;
                            break;
                        }

                        fileName = GetCurrentFileName(saveSlotHandler, i);
                    }
                    path = Path.Combine(ToolsConfig.folder_saves, fileName);
                }

                if (pass)
                {
                    string savePath = $"{path}{ToolsConfig.ext_saves}";

                    using (BinaryWriter binData = new BinaryWriter(new FileStream(savePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None)))
                    {
                        saveSlotHandler.CurrentSaveSlot.Write(binData);
                    }

                    if (File.Exists(ToolsConfig.file_desc))
                    {
                        File.AppendAllText(ToolsConfig.file_desc, $"\n{fileName}{ToolsConfig.ext_saves}: {fileName}");
                        ToolsConfig.files_saves.Add(savePath, fileName);
                        ToolsConfig.files_saves = ToolsConfig.files_saves.OrderBy(x => x.Key, StringComparer.OrdinalIgnoreCase).ToDictionary(x => x.Key, y => y.Value);
                        Attributes.saveIndex = ToolsConfig.files_saves.Keys.ToList().IndexOf(savePath);
                    }

                    Error(ErrorType.SAVE_CREATED);
                }
            }
            else
            {
                if (Game.GetPlayer() == null)
                    Error(ErrorType.NO_SAVESLOT);
                else
                    Error(ErrorType.FATEL);
            }
        }

        public void FuncResGame()
        {
            BaseModule baseMod = Game.GetBaseModule();
            Player player = Game.GetPlayer();
            if (baseMod != null && player != null && Core.Instance.UIManager != null && !Core.Instance.UIManager.Overlay.IsShowingLoadingScreen)
            {
                baseMod.UnPauseGame(PauseType.ForceOff);
                baseMod.UnloadCurrentStage();
                Core.Instance.Platform.User.LogOut();

                if (Game.TryGetValue(Game.fiBaseMod, baseMod, "currentSceneSetupInstructions", out List<ASceneSetupInstruction> currentSceneSetupInstructions))
                {
                    for (int i = currentSceneSetupInstructions.Count - 1; i >= 0; i--)
                    {
                        Game.invoke(baseMod, "StopLoadInstruction", new object[] { currentSceneSetupInstructions[i] }, Game.miBaseMod);
                    }
                }

                baseMod.StartGameToStage(Stage.Prelude);
            }
            else
            {
                Error(ErrorType.NO_ACTIVE_GAME);
            }
        }

        private static SavedData savedData = new SavedData();
        private class SavedData
        {
            public Vector3      position        { get { return Attributes.savedPos;                 } set { Attributes.savedPos             = value; } }
            public Vector3      velocityDir     { get { return Attributes.savedVelDir;              } set { Attributes.savedVelDir          = value; } }
            public float        velocity        { get { return Attributes.savedSpeed;               } set { Attributes.savedSpeed           = value; } }
            public Vector3      velocityVector  { get { return Attributes.savedSpeedVector;         } set { Attributes.savedSpeedVector     = value; } }
            public Quaternion   rotation        { get { return Attributes.savedRot;                 } set { Attributes.savedRot             = value; } }
            public float        boost           { get { return Attributes.savedBoost;               } set { Attributes.savedBoost           = value; } }
            public MoveStyle    moveStyleCurrent{ get { return Attributes.savedMovestyleCurrent;    } set { Attributes.savedMovestyleCurrent= value; } }
            public MoveStyle    moveStyleEquip  { get { return Attributes.savedMovestyleEquip;      } set { Attributes.savedMovestyleEquip  = value; } }
            public bool         usingEquipment  { get { return Attributes.savedEquipmentUsing;      } set { Attributes.savedEquipmentUsing  = value; } }
            public float        storageSpeed    { get { return Attributes.savedStorage;             } set { Attributes.savedStorage         = value; } }
            public Ability      ability         { get { return Attributes.savedAbility;             } set { Attributes.savedAbility         = value; } }
            public int          animation       { get { return Attributes.savedAnim;                } set { Attributes.savedAnim            = value; } }
            public bool         hasBoost        { get { return Attributes.savedHasBoost;            } set { Attributes.savedHasBoost        = value; } }
            public bool         hasDash         { get { return Attributes.savedHasDash;             } set { Attributes.savedHasDash         = value; } }
        }

        public void FuncSave()
        {
            Player player = Game.GetPlayer();
            WallrunLineAbility wallrunLine = Game.GetWallrunLineAbility();
            if
            (
                player != null && wallrunLine != null &&
                Game.TryGetValue(Game.fiPlayer, player,         "boostCharge",              out float           boostCharge)            &&
                Game.TryGetValue(Game.fiPlayer, player,         "moveStyle",                out MoveStyle       moveStyle)              &&
                Game.TryGetValue(Game.fiPlayer, player,         "moveStyleEquipped",        out MoveStyle       moveStyleEquipped)      &&
                Game.TryGetValue(Game.fiPlayer, player,         "usingEquippedMovestyle",   out bool            usingEquippedMovestyle) &&
                Game.TryGetValue(Game.fiWallrun,wallrunLine,    "lastSpeed",                out float           lastSpeed)              &&
                Game.TryGetValue(Game.fiPlayer, player,         "ability",                  out Ability         curAbility)             &&
                Game.TryGetValue(Game.fiPlayer, player,         "curAnim",                  out int             curAnim)                &&
                Game.TryGetValue(Game.fiPlayer, player,         "boostAbility",             out BoostAbility    boostAbility)           &&
                Game.TryGetValue(Game.fiPlayer, player,         "airDashAbility",           out AirDashAbility  airDashAbility)
            )
            {
                savedData = new SavedData()
                {
                    position            = player.tf.position,
                    velocityDir         = player.motor.velocity.normalized,
                    velocity            = player.motor.velocity.magnitude,
                    velocityVector      = player.motor.velocity,
                    rotation            = player.motor.rotation,
                    boost               = boostCharge,
                    moveStyleCurrent    = moveStyle,
                    moveStyleEquip      = moveStyleEquipped,
                    usingEquipment      = usingEquippedMovestyle,
                    storageSpeed        = lastSpeed,
                    ability             = curAbility,
                    animation           = curAnim,
                    hasBoost            = boostAbility.haveAirStartBoost,
                    hasDash             = airDashAbility.haveAirDash
                };
            }
            else
            {
                Error(player == null ? ErrorType.PLAYER_NOT_FOUND : ErrorType.FATEL);
            }
        }

        private void RefreshPlayer()
        {
            Player player = Game.GetPlayer();
            if (player != null)
            {
                Game.invoke(player, "InitHitboxes", null, Game.miPlayer);
                Game.invoke(player, "InitCuffs", null, Game.miPlayer);
            }
        }

        public void FuncLoad()
        {
            Player player = Game.GetPlayer();
            if (player != null)
            {
                // Refresh player
                Game.invoke(player, "InitCuffs", null, Game.miPlayer);

                // Load Player Position and Rotation
                Game.GetWorldHandler()?.PlaceCurrentPlayerAt(savedData.position, savedData.rotation, true);

                // Load Velocity (Horizontal Independent)
                Vector3 horVel = savedData.velocityDir * savedData.velocity; player.SetVelocity(new Vector3(horVel.x, savedData.velocityVector.y, horVel.z));

                // Set Boost Value
                player.AddBoostCharge(-player.boostCharge); player.AddBoostCharge(savedData.boost);

                // Set Movestyle
                if (savedData.moveStyleEquip != MoveStyle.ON_FOOT)
                {
                    player.SetCurrentMoveStyleEquipped(savedData.moveStyleEquip, changeAnim: false); player.SwitchToEquippedMovestyle(savedData.usingEquipment, showEffect: false);
                    Attributes.styleIndex = (int)savedData.moveStyleEquip;
                }

                if (Core.Instance.SaveManager != null && Core.Instance.SaveManager.CurrentSaveSlot != null && Core.Instance.SaveManager.CurrentSaveSlot.GetCharacterProgress(Core.Instance.SaveManager.CurrentSaveSlot.currentCharacter) != null)
                    Core.Instance.SaveManager.CurrentSaveSlot.GetCharacterProgress(Core.Instance.SaveManager.CurrentSaveSlot.currentCharacter).moveStyle = (MoveStyle)Attributes.styleIndex;

                // Set Zip Storage Speed
                SetStorageSpeed();
                
                if (savedData.animation != -1)
                    player.PlayAnim(savedData.animation, true, true, 0f);

                if (Game.TryGetValue(Game.fiPlayer, player, "boostAbility", out BoostAbility boostAbility) && Game.TryGetValue(Game.fiPlayer, player, "airDashAbility", out AirDashAbility airDashAbility))
                {
                    boostAbility.haveAirStartBoost = savedData.hasBoost;
                    airDashAbility.haveAirDash = savedData.hasDash;
                }

                // Update Ability
                if (savedData.ability != null)
                {
                    Type abiltiyType = savedData.ability.GetType();
                    if      (abiltiyType == typeof(GrindAbility))       { GrindAbility          a = savedData.ability as GrindAbility;          a.cooldown      = 0f; }
                    else if (abiltiyType == typeof(WallrunLineAbility)) { WallrunLineAbility    a = savedData.ability as WallrunLineAbility;    a.cooldownTimer = 0f; }
                    else if (abiltiyType == typeof(HandplantAbility))   { HandplantAbility      a = savedData.ability as HandplantAbility;      a.cooldownTimer = 0f; }
                }
            }
            else
            {
                Error(ErrorType.PLAYER_NOT_FOUND);
            }
        }

        public void FuncGoToLevel()
        {
            BaseModule baseMod = Game.GetBaseModule();
            if (baseMod != null && Game.GetPlayer() != null)
            {
                baseMod.UnPauseGame(PauseType.ForceOff);
                baseMod.StageManager.ExitCurrentStage((Stage)toolsConfig.levelToName.Keys.ToArray()[Attributes.levelIndex], Stage.NONE);
            }
            else
            {
                Error(ErrorType.NO_ACTIVE_GAME);
            }
        }

        private float timeUpdate = Time.time;
        private const float updateRate = 0.25f;
        public void FuncTriggers() { ToggleTriggers(); HandleTriggers(); }
        private void ToggleTriggers(bool forceOff = false) { if (forceOff ? Toggles.triggers = false : Toggles.triggers = !Toggles.triggers) { timeUpdate = Time.time; activeFunctionsFixed += UpdateTriggers; } else { activeFunctionsFixed -= UpdateTriggers; } }
        private void UpdateTriggers()
        {
            if (Toggles.triggers && Time.time >= (timeUpdate + updateRate))
            {
                timeUpdate = Time.time;
                HandleTriggers();
            }
        }

        private int step = 0;
        public void HandleTriggers()
        {
            WorldHandler worldHandler = Game.GetWorldHandler();
            Player player = Game.GetPlayer();
            bool worldHandlerExists = worldHandler != null && worldHandler.CurrentCamera != null;

            if (Toggles.triggers && player != null && worldHandlerExists)
            {
                // Get Material
                if (triggerMat == null)
                    triggerMat = new Material(Shader.Find("TextMeshPro/Sprite"));

                // Give All Masks To Camera (Thank Reference of Original TriggerTools)
                worldHandler.CurrentCamera.cullingMask = ~0;

                // Get Colliders Around Player, Filtering Out Exceptions
                List<Collider> allColliders = Physics.OverlapSphere(player.transform.position, 150f).Where(x =>
                    x.gameObject.activeInHierarchy &&
                    !x.gameObject.TryGetComponent(out TriggerRender tr) && !x.gameObject.TryGetComponent(out GrindLine gl) && !x.gameObject.TryGetComponent(out StreetLife sl) &&
                    x.tag != "walkZone"
                ).ToList();

                if (allColliders.Count > 0)
                {
                    List<Collider> colliders = new List<Collider>();

                    // Filter Colliders On Next Step Each New Update (saves on thread calls)
                    switch(step)
                    {
                        case 0:
                            colliders = allColliders.Where(filter => filter.name == "PyramidWoodsCollider" || filter.name == "CollBox" || filter.isTrigger || filter.tag != "Untagged" || (filter.material == null && filter.enabled == true)).ToList();
                            step++;
                        break;

                        case 1:
                            colliders = allColliders.Where(filter => filter.gameObject.TryGetComponent(out MeshRenderer meshRenderer) && (meshRenderer.material.name.Contains("ProCollisionMat") || meshRenderer.material.name.Contains("ProBuilderDefault"))).ToList();
                            step++;
                        break;

                        // Edgecases
                        case 2:
                            colliders = allColliders.Where(filter => filter.name == "DH_BRect_EdgeBISBIS (45)" || (filter.name == "DH_MarketShield" && filter.TryGetComponent(out BoxCollider col) && col.bounds.center.ToString() == "(-2815.61, 135.45, -90.82)") || filter.name == "Tower_OfficeRow (1)").ToList();
                            step = 0;
                        break;
                    }

                    // Add Trigger Render Class to Filter
                    foreach (var obj in colliders)
                        obj.gameObject.AddComponent<TriggerRender>();
                }
            }
            else if (worldHandlerExists)
            {
                worldHandler.CurrentCamera.cullingMask = WorldHandler.GetGameplayCameraCullingMask();
            }
        }

        public static Mesh cubeMesh;
        public static Material triggerMat;
        public class TriggerRender : MonoBehaviour
        {
            GameObject targetObject;

            void Awake()
            {
                targetObject = gameObject;

                HandleEdgeCases();
                SetUpMesh();
                SetUpFilter();
                SetUpRenderer();
            }

            void HandleEdgeCases()
            {
                if (targetObject.name == "DH_MarketShield" && targetObject.TryGetComponent(out BoxCollider box))
                {
                    GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

                    cube.transform.SetPositionAndRotation(transform.TransformPoint(box.center), box.transform.rotation);
                    cube.transform.localScale = new Vector3(box.size.x + 0.5f, transform.TransformVector(box.size).y, transform.TransformVector(box.size).x);
                    Destroy(cube.GetComponent<Collider>());

                    cube.GetComponent<Renderer>().enabled = false;

                    targetObject = cube;
                }
                else if (targetObject.name == "DH_BRect_EdgeBISBIS (45)" && targetObject.TryGetComponent(out BoxCollider box2))
                {
                    GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

                    cube.transform.SetPositionAndRotation(transform.TransformPoint(box2.center), box2.transform.rotation);
                    cube.transform.localScale = new Vector3(transform.TransformVector(box2.size).x + 1f, transform.TransformVector(box2.size).y, transform.TransformVector(box2.size).z - 3.4f);
                    Destroy(cube.GetComponent<Collider>());

                    cube.GetComponent<Renderer>().enabled = false;

                    targetObject = cube;
                }
                else if (targetObject.name == "Tower_OfficeRow (1)" && targetObject.TryGetComponent(out BoxCollider box3))
                {
                    GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

                    cube.transform.SetPositionAndRotation(transform.TransformPoint(box3.center), box3.transform.rotation);
                    cube.transform.Rotate(0, 90, 0);
                    cube.transform.localScale = new Vector3(transform.TransformVector(box3.size).x, transform.TransformVector(box3.size).y, transform.TransformVector(box3.size).z);
                    Destroy(cube.GetComponent<Collider>());

                    cube.GetComponent<Renderer>().enabled = false;

                    targetObject = cube;
                }
            }

            // Set a Cube Mesh (Thank Reference of Original TriggerTools)
            void SetUpMesh()
            {
                if (cubeMesh == null)
                {
                    GameObject defaultCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    cubeMesh = defaultCube.GetComponent<MeshFilter>().mesh;
                    Destroy(defaultCube);
                }
                mesh = Instantiate(cubeMesh);
            }

            Mesh        mesh;
            MeshFilter  filter;

            void SetUpFilter()
            {
                if (!targetObject.TryGetComponent(out filter))
                {
                    filter = targetObject.AddComponent<MeshFilter>();

                    filter.mesh = mesh;
                    filter.mesh.RecalculateNormals();
                    filter.mesh.RecalculateBounds();
                    filter.mesh.Optimize();
                }
                else
                {
                    filter  = targetObject.GetComponent<MeshFilter>();
                    mesh    = filter.mesh;
                }
            }

            MeshRenderer    renderer;
            Material        originalMaterial;
            Material        triggerMaterial;
            bool            wasEnabled = false;
            float           alpha = 0.1f;

            void SetUpRenderer()
            {
                triggerMaterial         = Instantiate(triggerMat);
                triggerMaterial.color   = GetMaterialColor();

                if (!targetObject.TryGetComponent(out renderer))
                {
                    renderer = targetObject.AddComponent<MeshRenderer>();
                    wasEnabled = false;
                }
                else
                {
                    if (renderer.material.HasProperty("_Color"))
                        triggerMaterial.color = new Color(renderer.material.color.r, renderer.material.color.g, renderer.material.color.b, alpha);

                    wasEnabled = renderer.enabled;
                }

                originalMaterial    = renderer.material;
                renderer.material   = triggerMaterial;
            }

            Color GetMaterialColor()
            {
                string toCheck = targetObject.tag.ToLower() != "untagged" ? targetObject.tag.ToLower() : targetObject.name.ToLower();

                switch (toCheck)
                {
                    case "spawner":
                        return new Color(0f, 1f, 0f, alpha);

                    case "npc":
                        return new Color(0.961f, 0.671f, 0.251f, alpha);

                    case "trigger":
                        return new Color(0.961f, 0.671f, 0.251f, alpha);

                    case "progressobject":
                        return new Color(0.961f, 0.671f, 0.251f, alpha);

                    case "machine":
                        return new Color(1f, 1f, 0.351f, alpha);

                    case "boostchargeparented":
                        return new Color(0f, 0f, 1f, alpha);

                    case "camerazone":
                        return new Color(0.5f, 0.5f, 1f, alpha);

                    case "dh_marketshield":
                        return new Color(0.71f, 0.15f, 0.8f, alpha);

                    case "dh_brect_edgebisbis (45)":
                        return new Color(0.71f, 0.15f, 0.8f, alpha);

                    case "tower_officerow (1)":
                        return new Color(0.71f, 0.15f, 0.8f, alpha);

                    case "collbox":
                        return new Color(0.71f, 0.15f, 0.8f, alpha);
                }

                if (targetObject.name.Contains("trigger"))
                    return new Color(0.961f, 0.671f, 0.251f, alpha);

                return new Color(0f, 1f, 1f, alpha);
            }

            void Update()
            {
                renderer.material = Toggles.triggers ? triggerMaterial : originalMaterial;
                renderer.enabled = Toggles.triggers ? true : wasEnabled;
            }
        }

        public static List<GraffitiArt> used_graffiti = new List<GraffitiArt>();
        public void FuncAutoGraf() { ToggleAutoGraf(); }
        public void ToggleAutoGraf(bool forceOff = false) { if (forceOff ? Toggles.autograf = false : Toggles.autograf = !Toggles.autograf) activeFunctions += UpdateAutoGraf; else activeFunctions -= UpdateAutoGraf; }
        private void UpdateAutoGraf()
        {
            // NOT NEEDED
        }

        public string FuncChangeSpawn(int i)
        {
            Player player = Game.GetPlayer();
            WorldHandler worldHandler = Game.GetWorldHandler();
            if (player != null && worldHandler != null)
            {
                List<PlayerSpawner> playerSpawners = worldHandler.SceneObjectsRegister.playerSpawners;

                int newSpawnIndex = Attributes.spawnIndex + i;
                int max = playerSpawners.Count() - 1;
                Attributes.spawnIndex = newSpawnIndex < 0 ? max : newSpawnIndex > max ? 0 : newSpawnIndex;

                worldHandler.PlaceCurrentPlayerAt(playerSpawners[Attributes.spawnIndex].transform.position, playerSpawners[Attributes.spawnIndex].transform.rotation, true);
            }
            return SpawnToName(Attributes.spawnIndex);
        }
        public string SpawnToName(int i)
        {
            return $"{i}";
        }

        public string FuncChangeDreamSpawn(int i)
        {
            Player player = Game.GetPlayer();
            WorldHandler worldHandler = Game.GetWorldHandler();
            if (player != null && worldHandler != null && worldHandler.SceneObjectsRegister.RetrieveDreamEncounter() != null)
            {
                var dreamSpawners = worldHandler.SceneObjectsRegister.RetrieveDreamEncounter().checkpoints;

                if (dreamSpawners.Count() > 0)
                {
                    int newDreamIndex = Attributes.dreamSpawnIndex + i;
                    int max = dreamSpawners.Count() - 1;
                    Attributes.dreamSpawnIndex = newDreamIndex < 0 ? max : newDreamIndex > max ? 0 : newDreamIndex;

                    worldHandler.PlaceCurrentPlayerAt(dreamSpawners[Attributes.dreamSpawnIndex].spawnLocation.position, dreamSpawners[Attributes.dreamSpawnIndex].spawnLocation.rotation, true);
                }
            }
            return DreamSpawnToName(Attributes.dreamSpawnIndex);
        }
        public string DreamSpawnToName(int i)
        {
            return $"{i}";
        }

        public void FuncUnlock()
        {
            Player player = Game.GetPlayer();
            SaveManager saveManager = Game.GetSaveManager();
            if (player != null && saveManager != null && saveManager.CurrentSaveSlot != null)
            {
                player.LockCameraApp(false);
                player.LockBoostAbility(false);
                player.LockBoostpack(false);
                player.LockCharacterSelect(false);
                for (int i = 0; i < (int)Dances.MAX; i++) { saveManager.CurrentSaveSlot.dancesUnlocked[i] = true; }
                player.LockFortuneApp(false);
                player.LockPhone(false);
                player.LockSpraycan(false);
                player.LockSwitchToEquippedMoveStyle(false);
                saveManager.CurrentSaveSlot.taxiLocked = false;
            }
            else
            {
                Error(player == null ? ErrorType.PLAYER_NOT_FOUND : ErrorType.NO_SAVE);
            }
        }

        public void FuncUnlockChars()
        {
            Player player = Game.GetPlayer();
            SaveManager saveManager = Game.GetSaveManager();
            if (player != null && saveManager != null && saveManager.CurrentSaveSlot != null)
            {
                for (int i = 0; i < (int)Characters.MAX; i++) { saveManager.CurrentSaveSlot.UnlockCharacter((Characters)i); }
            }
            else
            {
                Error(player == null ? ErrorType.PLAYER_NOT_FOUND : ErrorType.NO_SAVE);
            }
        }

        public void FuncForcePolice()
        {
            Player          player          = Game.GetPlayer();
            WantedManager   wantedManager   = WantedManager.instance;
            SaveManager     saveManager     = Game.GetSaveManager();
            if (player != null && wantedManager != null && saveManager != null && saveManager.CurrentSaveSlot != null)
            {
                float max = saveManager.CurrentSaveSlot.maxWantedStars;
                saveManager.CurrentSaveSlot.maxWantedStars = 6f;
                Game.Instance.InvokeMethod(wantedManager, "ProcessCrime",       new object[] { WantedManager.Crime.VANDALISM_XLARGE });
                Game.Instance.InvokeMethod(wantedManager, "MakePlayerWanted",   new object[] { WantedManager.Crime.VANDALISM_XLARGE });
                saveManager.CurrentSaveSlot.maxWantedStars = max;
                saveManager.SaveCurrentSaveSlotImmediate();
            }
            else
            {
                if (player == null)
                    Error(ErrorType.PLAYER_NOT_FOUND);
                else if (wantedManager == null)
                    Error(ErrorType.BAD_CONDS, "No Wanted Manager");
                else
                    Error(ErrorType.NO_SAVE);
            }
        }
    }
}
