using Enums;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ModQuickEquip
{
    class ModQuickEquip : MonoBehaviour
    {
        private static ModQuickEquip s_Instance;
        private static Player player;
        private static InventoryBackpack inventoryBackpack;

        public static Dictionary<int, ItemSlot> WeaponSlots { get; private set; } = new Dictionary<int, ItemSlot>();
        public static ItemSlot EquippedWeaponSlot { get; private set; }
        public static Item EquippedWeapon { get; private set; }
        public static ItemSlot BladeSlot { get; private set; } = inventoryBackpack.GetSlotByIndex(4, BackpackPocket.Left);
        public static int WeaponSlotIndex { get; private set; }

        private bool IsModQuickEquipActive { get; set; } = false;

        public ModQuickEquip()
        {
            IsModQuickEquipActive = true;
            s_Instance = this;
        }

        public static ModQuickEquip Get()
        {
            return s_Instance;
        }

        private void Update()
        {
            try
            {
                if (QuickEquipSlotKeyPressed())
                {
                    InitData();
                    ToggleEquippedWeapon();
                }
            }
            catch (Exception exc)
            {
                ModAPI.Log.Write($"[{nameof(ModQuickEquip)}.{nameof(ModQuickEquip)}:{nameof(Update)}] throws exception: {exc.Message}");
            }
        }

        private bool QuickEquipSlotKeyPressed()
        {
            return Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Alpha5);
        }

        private void InitData()
        {
            WeaponSlotIndex = GetQuickEquipKeyPressed();
            player = Player.Get();
            inventoryBackpack = InventoryBackpack.Get();
            if (inventoryBackpack != null)
            {
                for (int i = 0; i < 5; i++)
                {
                    if (!WeaponSlots.ContainsKey(i))
                    {
                        WeaponSlots.Add(i, inventoryBackpack.GetSlotByIndex(i, BackpackPocket.Left));
                    }
                }
                EquippedWeaponSlot = inventoryBackpack.m_EquippedItemSlot;
                EquippedWeapon = inventoryBackpack.m_EquippedItem;
            }

        }

        private int GetQuickEquipKeyPressed()
        {
            int key = 0;
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                key = 0;
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                key = 1;
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                key = 2;
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                key = 3;
            }
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                key = 4;
            }
            return key;
        }

        public void ToggleEquippedWeapon()
        {
            if (CanEquipItem)
            {
                if (EquippedWeapon != null)
                {
                    player.HideWeapon();
                }
                else
                {
                    player.Equip(WeaponSlots.GetValueOrDefault(WeaponSlotIndex));
                }
            }
        }

        /// <summary>
        /// Verify if player can equip item at this frame
        /// </summary>
        public static bool CanEquipItem
        {
            get
            {
                if (HarvestingAnimalController.Get().IsActive())
                {
                    return false;
                }
                if (MudMixerController.Get().IsActive())
                {
                    return false;
                }
                if (HarvestingSmallAnimalController.Get().IsActive())
                {
                    return false;
                }
                if (FishingController.Get().IsActive() && !FishingController.Get().CanHideRod())
                {
                    return false;
                }
                if (player.m_Animator.GetBool(player.m_CleanUpHash))
                {
                    return false;
                }
                if (ScenarioManager.Get().IsBoolVariableTrue("PlayerMechGameEnding"))
                {
                    return false;
                }
                return true;
            }
        }
    }
}
