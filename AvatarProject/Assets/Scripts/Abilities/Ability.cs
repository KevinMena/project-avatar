using System.Collections.Generic;
using UnityEngine;

using AvatarBA.Abilities.Effects;

namespace AvatarBA.Abilities
{
    [CreateAssetMenu(fileName = "Ability_", menuName = "Abilities/Ability")]
    public class Ability : ScriptableObject
    {
        [SerializeField]
        protected string _id;

        [SerializeField]
        protected Sprite icon;

        [SerializeField]
        protected string abilityName;

        [SerializeField, TextArea]
        protected string description;

        [SerializeField]
        protected float cooldown;

        [SerializeField]
        protected float activeTime;

        [SerializeField]
        protected float cost;

        [SerializeField]
        protected AbilityType type;

        [SerializeReference, SerializeReferenceDropdown]
        protected List<AbilityEffect> effects;

        public string Id => _id;
        public Sprite Icon => icon;
        public string Name => abilityName;
        public string Description => description;
        public float Cooldown => cooldown;
        public float ActiveTime => activeTime;
        public float Cost => cost;
        public AbilityType Type => type;
        public ref readonly List<AbilityEffect> Effects => ref effects;

        public void Cast(ExecutionContext context)
        {
            foreach (var effect in effects)
            {
                effect.Execute(context);
            }
        }
    }

    public enum AbilityState
    {
        Ready,
        Active,
        Cooldown
    }

    public enum AbilityType
    {
        Fire,
        Wind,
        Earth,
        Water,
        Neutral
    }
}
