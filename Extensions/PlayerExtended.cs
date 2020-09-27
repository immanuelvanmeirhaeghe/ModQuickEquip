using UnityEngine;

namespace ModQuickEquip.Extensions
{
    class PlayerExtended : Player
    {
        protected override void Start()
        {
            base.Start();
            new GameObject($"__{nameof(ModQuickEquip)}__").AddComponent<ModQuickEquip>();
        }
    }
}
