using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using AvatarBA.Stats;

namespace AvatarBA.Abilities
{
    [CreateAssetMenu(fileName = "Ability_IceArmor", menuName = "Abilities/Water/Ice Armor")]
    public class IceArmor : Ability
    {
        [SerializeField]
        private float _baseAugment = 0;

        [SerializeField]
        private float _duration = 0;

        private const string DEFENSE_STAT = "defense";

        public override IEnumerator Trigger(GameObject owner)
        {
            // Spawn VFX
            // Create modifier for the stat and apply it
            if(owner.TryGetComponent(out CharacterStatsController statsController))
            {
                statsController.ApplyChangeToStat(DEFENSE_STAT, Id, _baseAugment, StatModifierType.Flat);
                yield return new WaitForSeconds(_duration);
                statsController.RemoveChangeToStat(DEFENSE_STAT, Id);
            }
            // VFX for after
            yield return base.Trigger(owner);
        }
    }
}