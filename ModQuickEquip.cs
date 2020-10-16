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
            player = Player.Get();
            inventoryBackpack = InventoryBackpack.Get();
        }

        private void InitWeaponSlots()
        {
            Quick1Slot = inventoryBackpack.GetSlotByIndex(0, BackpackPocket.Left);
            Quick2Slot = inventoryBackpack.GetSlotByIndex(1, BackpackPocket.Left);
            Quick3Slot = inventoryBackpack.GetSlotByIndex(2, BackpackPocket.Left);
            Quick4Slot = inventoryBackpack.GetSlotByIndex(3, BackpackPocket.Left);
            Quick5Slot = inventoryBackpack.GetSlotByIndex(4, BackpackPocket.Left);
        }

        public void ToggleEquippedWeapon(int idx)
        {
            switch (idx)
            {
                case 0:
                    if (player.HasItemEquiped(Quick1Slot?.m_Item?.GetName()))
                    {
                        player.HideWeapon();
                        player.ItemsFromHandsPutToInventory();
                        player.UpdateHands();
                    }
                    else
                    {
                        player.Equip(Quick1Slot);
                        player.ShowWeapon();
                    }
                    break;
                case 1:
                    if (player.HasItemEquiped(Quick2Slot?.m_Item?.GetName()))
                    {
                        player.HideWeapon();
                        player.ItemsFromHandsPutToInventory();
                        player.UpdateHands();
                    }
                    else
                    {
                        player.Equip(Quick2Slot);
                        player.ShowWeapon();
                    }
                    break;
                case 2:
                    if (player.HasItemEquiped(Quick3Slot?.m_Item?.GetName()))
                    {
                        player.HideWeapon();
                        player.ItemsFromHandsPutToInventory();
                        player.UpdateHands();
                    }
                    else
                    {
                        player.Equip(Quick3Slot);
                        player.ShowWeapon();
                    }
                    break;
                case 3:
                    if (player.HasItemEquiped(Quick4Slot?.m_Item?.GetName()))
                    {
                        player.HideWeapon();
                        player.ItemsFromHandsPutToInventory();
                        player.UpdateHands();
                    }
                    else
                    {
                        player.Equip(Quick4Slot);
                        player.ShowWeapon();
                    }
                    break;
                case 4:
                    if (player.HasItemEquiped(Quick5Slot?.m_Item?.GetName()))
                    {
                        player.HideWeapon();
                        player.ItemsFromHandsPutToInventory();
                        player.UpdateHands();
                    }
                    else
                    {
                        player.Equip(Quick5Slot);
                        player.ShowWeapon();
                    }
                    break;
                default:
                    break;
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
