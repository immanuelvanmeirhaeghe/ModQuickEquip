using Enums;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ModQuickEquip
{
    class ModQuickEquip : MonoBehaviour
    {
        private static readonly string ModName = nameof(ModQuickEquip);
        private static ModQuickEquip Instance;
        private static Player LocalPlayer;
        private static InventoryBackpack LocalInventoryBackpack;

        private bool IsModQuickEquipActive { get; set; } = false;
        public static Item CurrentWeapon { get; private set; }
        public static ItemSlot Quick1Slot { get; private set; }
        public static ItemSlot Quick2Slot { get; private set; }
        public static ItemSlot Quick3Slot { get; private set; }
        public static ItemSlot Quick4Slot { get; private set; }
        public static ItemSlot Quick5Slot { get; private set; }

        public ModQuickEquip()
        {
            IsModQuickEquipActive = true;
            Instance = this;
        }

        public static ModQuickEquip Get()
        {
            return Instance;
        }

        private void Update()
        {
            try
            {
                UpdateQuickEquip();
            }
            catch (Exception exc)
            {
                ModAPI.Log.Write($"[{ModName}:{nameof(Update)}] throws exception:\n{exc.Message}");
            }
        }

        private void UpdateQuickEquip()
        {
            if (CanEquipItem)
            {
                InitData();
                InitWeaponSlots();

                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    ToggleEquippedWeapon(0);
                }
                if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    ToggleEquippedWeapon(1);
                }
                if (Input.GetKeyDown(KeyCode.Alpha3))
                {
                    ToggleEquippedWeapon(2);
                }
                if (Input.GetKeyDown(KeyCode.Alpha4))
                {
                    ToggleEquippedWeapon(3);
                }
                if (Input.GetKeyDown(KeyCode.Alpha5))
                {
                    ToggleEquippedWeapon(4);
                }
            }
        }

        private void InitData()
        {
            LocalPlayer = Player.Get();
            LocalInventoryBackpack = InventoryBackpack.Get();
        }

        private void InitWeaponSlots()
        {
            Quick1Slot = LocalInventoryBackpack.GetSlotByIndex(0, BackpackPocket.Left);
            Quick2Slot = LocalInventoryBackpack.GetSlotByIndex(1, BackpackPocket.Left);
            Quick3Slot = LocalInventoryBackpack.GetSlotByIndex(2, BackpackPocket.Left);
            Quick4Slot = LocalInventoryBackpack.GetSlotByIndex(3, BackpackPocket.Left);
            Quick5Slot = LocalInventoryBackpack.GetSlotByIndex(4, BackpackPocket.Left);
        }

        public void ToggleEquippedWeapon(int idx)
        {
            CurrentWeapon = LocalInventoryBackpack.m_EquippedItem;
            switch (idx)
            {
                case 0:
                    if (Quick1Slot?.m_Item?.GetName() == CurrentWeapon?.GetName())
                    {
                        UnequipWeapon();
                    }
                    else
                    {
                        EquipWeapon(Quick1Slot);
                    }
                    break;
                case 1:
                    if (Quick2Slot?.m_Item?.GetName() == CurrentWeapon?.GetName())
                    {
                        UnequipWeapon();
                    }
                    else
                    {
                        EquipWeapon(Quick2Slot);
                    }
                    break;
                case 2:
                    if (Quick3Slot?.m_Item?.GetName() == CurrentWeapon?.GetName())
                    {
                        UnequipWeapon();
                    }
                    else
                    {
                        EquipWeapon(Quick3Slot);
                    }
                    break;
                case 3:
                    if (Quick4Slot?.m_Item?.GetName() == CurrentWeapon?.GetName())
                    {
                        UnequipWeapon();
                    }
                    else
                    {
                        EquipWeapon(Quick4Slot);
                    }
                    break;
                case 4:
                    if (Quick5Slot?.m_Item?.GetName() == CurrentWeapon?.GetName())
                    {
                        UnequipWeapon();
                    }
                    else
                    {
                        EquipWeapon(Quick5Slot);
                    }
                    break;
                default:
                    break;
            }
        }

        private static void EquipWeapon(ItemSlot slot)
        {
            LocalPlayer.Equip(slot);
            LocalPlayer.ShowWeapon();
        }

        private static void UnequipWeapon()
        {
            LocalPlayer.HideWeapon();
            LocalPlayer.ItemsFromHandsPutToInventory();
            LocalPlayer.UpdateHands();
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
