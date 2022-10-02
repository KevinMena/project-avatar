using System.Collections;
using UnityEngine;

using AvatarBA.Stats;

namespace AvatarBA.Abilities
{
    [CreateAssetMenu(fileName = "Ability_IceArmor", menuName = "Abilities/Water/Ice Armor")]
    public class IceArmor : ModifierAbility
    {
        private const string DEFENSE_STAT = "defense";

        public override void Initialize() { }

        public override IEnumerator Trigger(GameObject owner)
        {
            // Spawn VFX
            // Create modifier for the stat and apply it
            yield return ApplyChange(owner, DEFENSE_STAT, StatModifierType.PercentAdditive);
            // VFX for after
            yield return null;
        }
    }
}