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

        public static Dictionary<int, ItemSlot> WeaponSlots { get; private set; }
        public static ItemSlot EquippedWeaponSlot { get; private set; }
        public static Item EquippedWeapon { get; private set; }
        public static ItemSlot BladeSlot { get; private set; }
        public static int WeaponSlotIndex { get; private set; }

        private bool IsModQuickEquipActive { get; set; } = false;

        public ModQuickEquip()
        {
            IsModQuickEquipActive = true;
            WeaponSlots = new Dictionary<int, ItemSlot>();
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
                UpdateQuickEquip();
            }
            catch (Exception exc)
            {
                ModAPI.Log.Write($"[{nameof(ModQuickEquip)}.{nameof(ModQuickEquip)}:{nameof(Update)}] throws exception: {exc.Message}");
            }
        }

        private void UpdateQuickEquip()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                WeaponSlotIndex = 0;
                InitData();
                InitWeaponSlots();
                ToggleEquippedWeapon();
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                WeaponSlotIndex = 1;
                InitData();
                InitWeaponSlots();
                ToggleEquippedWeapon();
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                WeaponSlotIndex = 2;
                InitData();
                InitWeaponSlots();
                ToggleEquippedWeapon();
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                WeaponSlotIndex = 3;
                InitData();
                InitWeaponSlots();
                ToggleEquippedWeapon();
            }
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                WeaponSlotIndex = 4;
                InitData();
                InitWeaponSlots();
                ToggleEquippedWeapon();
            }
        }

        private void InitData()
        {
            player = Player.Get();
            inventoryBackpack = InventoryBackpack.Get();
            EquippedWeaponSlot = inventoryBackpack.m_EquippedItemSlot;
            EquippedWeapon = inventoryBackpack.m_EquippedItem;
        }

        private void InitWeaponSlots()
        {
            WeaponSlots.Clear();
            for (int i = 0; i < 5; i++)
            {
                WeaponSlots.Add(i, inventoryBackpack.GetSlotByIndex(i, BackpackPocket.Left));
            }
            BladeSlot = WeaponSlots.GetValueOrDefault(4);
        }

        public void ToggleEquippedWeapon()
        {
            if (CanEquipItem)
            {
                ItemSlot slot = inventoryBackpack.GetSlotByIndex(WeaponSlotIndex, BackpackPocket.Left);
                if (EquippedWeaponSlot == slot )
                {
                    player.HideWeapon();
                }
                else
                {
                    player.Equip(slot);
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
                if (Player.Get().m_Animator.GetBool(Player.Get().m_CleanUpHash))
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
