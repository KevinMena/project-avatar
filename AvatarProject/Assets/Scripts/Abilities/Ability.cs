using System.Collections;
using UnityEngine;

using AvatarBA.Abilities.Effects;

namespace AvatarBA.Abilities
{
    public abstract class Ability : ScriptableObject
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

        [SerializeField]
        protected AbilityEffect[] effects;

        public string Id => _id;
        public Sprite Icon => icon;
        public string Name  => abilityName;
        public string Description => description;
        public float Cooldown => cooldown;
        public float ActiveTime => activeTime;
        public float Cost => cost;
        public AbilityType Type => type;
        public ref readonly AbilityEffect[] Effects => ref effects;

        public abstract void Initialize();
        public abstract IEnumerator Trigger(GameObject owner);
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
