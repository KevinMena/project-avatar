using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AvatarBA
{
    public class Ability : ScriptableObject
    {
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
        protected List<AbilityEffect> effects = new List<AbilityEffect>();

        public Sprite Icon => icon;
        public string Name  => abilityName;
        public string Description => description;
        public float Cooldown => cooldown;
        public float ActiveTime => activeTime;
        public float Cost => cost;
        public AbilityType Type => type;
        public ref readonly List<AbilityEffect> Effects => ref effects;

        public virtual void Initialize() { }
        public virtual IEnumerator Trigger(GameObject owner) { yield return null; }
        public virtual bool PassRequirements() { return true; }
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
