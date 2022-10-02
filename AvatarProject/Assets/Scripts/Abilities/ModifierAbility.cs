using System.Collections;
using UnityEngine;

using AvatarBA.Stats;

namespace AvatarBA.Abilities
{
    public abstract class ModifierAbility : Ability
    {
        [SerializeField]
        protected float _baseModifier = 0;

        [SerializeField]
        protected float _duration = 0;

        public float BaseModifier => _baseModifier;
        public float Duration => _duration;

        protected IEnumerator ApplyChange(GameObject owner, string statToChange, StatModifierType type)
        {
            if (owner.TryGetComponent(out CharacterStatsController statsController))
            {
                statsController.ApplyChangeToStat(statToChange, Id, BaseModifier, type);
                yield return new WaitForSeconds(_duration);
                statsController.RemoveChangeToStat(statToChange, Id);
            }
        }
    }
}