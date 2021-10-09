using Enums;
using ModQuickEquip.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;

namespace ModQuickEquip
{
    /// <summary>
    /// ModQuickEquip is a mod for Green Hell that adds a quick equip slot
    /// for the 5th weapon - knive slot attached to the related backpack pocket
    /// and keybound to Alpha5 or the key configurable in ModAPI
    /// </summary>
    class ModQuickEquip : MonoBehaviour
    {
        private static readonly string ModName = nameof(ModQuickEquip);
        private static ModQuickEquip Instance;
        private static Player LocalPlayer;
        private static HUDManager LocalHUDManager;
        private static InventoryBackpack LocalInventoryBackpack;

        private bool IsModQuickEquipActive { get; set; } = false;
        public static Item CurrentWeapon { get; private set; }
        public static ItemSlot Quick1Slot { get; private set; }
        public static ItemSlot Quick2Slot { get; private set; }
        public static ItemSlot Quick3Slot { get; private set; }
        public static ItemSlot Quick4Slot { get; private set; }
        public static ItemSlot Quick5Slot { get; private set; }

        public bool IsModActiveForMultiplayer { get; private set; }
        public bool IsModActiveForSingleplayer => ReplTools.AmIMaster();

        private void HandleException(Exception exc, string methodName)
        {
            string info = $"[{ModName}:{methodName}] throws exception:\n{exc.Message}";
            ModAPI.Log.Write(info);
            ShowHUDBigInfo(HUDBigInfoMessage(info, MessageType.Error, Color.red));
        }

        public static string PermissionChangedMessage(string permission, string reason)
           => $"Permission to use mods and cheats in multiplayer was {permission} because {reason}.";
        public static string HUDBigInfoMessage(string message, MessageType messageType, Color? headcolor = null)
            => $"<color=#{ (headcolor != null ? ColorUtility.ToHtmlStringRGBA(headcolor.Value) : ColorUtility.ToHtmlStringRGBA(Color.red))  }>{messageType}</color>\n{message}";

        public void ShowHUDBigInfo(string text)
        {
            string header = $"{ModName} Info";
            string textureName = HUDInfoLogTextureType.Count.ToString();
            HUDBigInfo hudBigInfo = (HUDBigInfo)LocalHUDManager.GetHUD(typeof(HUDBigInfo));
            HUDBigInfoData.s_Duration = 2f;
            HUDBigInfoData hudBigInfoData = new HUDBigInfoData
            {
                m_Header = header,
                m_Text = text,
                m_TextureName = textureName,
                m_ShowTime = Time.time
            };
            hudBigInfo.AddInfo(hudBigInfoData);
            hudBigInfo.Show(true);
        }

        public void ShowHUDInfoLog(string itemID, string localizedTextKey)
        {
            var localization = GreenHellGame.Instance.GetLocalization();
            HUDMessages hUDMessages = (HUDMessages)LocalHUDManager.GetHUD(typeof(HUDMessages));
            hUDMessages.AddMessage(
                $"{localization.Get(localizedTextKey)}  {localization.Get(itemID)}"
                );
        }

        private static readonly string RuntimeConfigurationFile = Path.Combine(Application.dataPath.Replace("GH_Data", "Mods"), "RuntimeConfiguration.xml");
        private static KeyCode ModKeybindingId { get; set; } = KeyCode.Alpha5;
        private KeyCode GetConfigurableKey(string buttonId)
        {
            KeyCode configuredKeyCode = default;
            string configuredKeybinding = string.Empty;

            try
            {
                if (File.Exists(RuntimeConfigurationFile))
                {
                    using (var xmlReader = XmlReader.Create(new StreamReader(RuntimeConfigurationFile)))
                    {
                        while (xmlReader.Read())
                        {
                            if (xmlReader["ID"] == ModName)
                            {
                                if (xmlReader.ReadToFollowing(nameof(Button)) && xmlReader["ID"] == buttonId)
                                {
                                    configuredKeybinding = xmlReader.ReadElementContentAsString();
                                }
                            }
                        }
                    }
                }

                configuredKeybinding = configuredKeybinding?.Replace("NumPad", "Keypad").Replace("Oem", "");

                configuredKeyCode = (KeyCode)(!string.IsNullOrEmpty(configuredKeybinding)
                                                            ? Enum.Parse(typeof(KeyCode), configuredKeybinding)
                                                            : GetType()?.GetProperty(buttonId)?.GetValue(this));
                return configuredKeyCode;
            }
            catch (Exception exc)
            {
                HandleException(exc, nameof(GetConfigurableKey));
                configuredKeyCode = (KeyCode)(GetType()?.GetProperty(buttonId)?.GetValue(this));
                return configuredKeyCode;
            }
        }

        public ModQuickEquip()
        {
            Instance = this;
        }

        public static ModQuickEquip Get()
        {
            return Instance;
        }

        public void Start()
        {
            ModManager.ModManager.onPermissionValueChanged += ModManager_onPermissionValueChanged;
            ModKeybindingId = GetConfigurableKey(nameof(ModKeybindingId));
        }

        private void ModManager_onPermissionValueChanged(bool optionValue)
        {
            string reason = optionValue ? "the game host allowed usage" : "the game host did not allow usage";
            IsModActiveForMultiplayer = optionValue;

            ShowHUDBigInfo(
                          optionValue ?
                            HUDBigInfoMessage(PermissionChangedMessage($"granted", $"{reason}"), MessageType.Info, Color.green)
                            : HUDBigInfoMessage(PermissionChangedMessage($"revoked", $"{reason}"), MessageType.Info, Color.yellow)
                            );
        }

        private void Update()
        {
            InitData();
            InitWeaponSlots();
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                EquipWeapon(Quick1Slot);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                EquipWeapon(Quick2Slot);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                EquipWeapon(Quick3Slot);
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                EquipWeapon(Quick4Slot);
            }
            if (Input.GetKeyDown(ModKeybindingId))
            {
                EquipWeapon(Quick5Slot);
            }
        }

        private void InitData()
        {
            LocalPlayer = Player.Get();
            LocalInventoryBackpack = InventoryBackpack.Get();
            LocalHUDManager = HUDManager.Get();
        }

        private void InitWeaponSlots()
        {
            Quick1Slot = LocalInventoryBackpack.GetSlotByIndex(0, BackpackPocket.Left);
            Quick2Slot = LocalInventoryBackpack.GetSlotByIndex(1, BackpackPocket.Left);
            Quick3Slot = LocalInventoryBackpack.GetSlotByIndex(2, BackpackPocket.Left);
            Quick4Slot = LocalInventoryBackpack.GetSlotByIndex(3, BackpackPocket.Left);
            Quick5Slot = LocalInventoryBackpack.GetSlotByIndex(4, BackpackPocket.Left);
        }

        private void EquipWeapon(ItemSlot slot)
        {
            if (IsModActiveForSingleplayer || IsModActiveForMultiplayer)
            {
                LocalPlayer.Equip(slot);
            }
        }
    }
}
