using Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ModQuickEquip
{
    class ModQuickEquip : MonoBehaviour
    {
        private static ModQuickEquip s_Instance;
        private static Player player;

        public bool IsModQuickEquipActive = false;

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
                if (CanEquipItem && Input.GetKeyDown(KeyCode.Alpha5))
                {
                    player = Player.Get();
                    player.Equip(InventoryBackpack.Get().GetSlotByIndex(4, BackpackPocket.Left));
                }
            }
            catch (Exception exc)
            {
                ModAPI.Log.Write($"[{nameof(ModQuickEquip)}.{nameof(ModQuickEquip)}:{nameof(Update)}] throws exception: {exc.Message}");
            }
        }

        private bool CanEquipItem
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
                if (ScenarioManager.Get().IsBoolVariableTrue("PlayerMechGameEnding"))
                {
                    return false;
                }
                return true;
            }
        }
    }
}
