using System.Collections;
using UnityEngine;

using AvatarBA.Abilities.Effects;

namespace AvatarBA.Abilities
{
    [CreateAssetMenu(fileName = "Ability_", menuName = "Abilities/New Ability")]
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
        protected AbilityEffect[] featureEffects;

        [SerializeField]
        protected AbilityEffect[] effects;

        public Sprite Icon => icon;
        public string Name  => abilityName;
        public string Description => description;
        public float Cooldown => cooldown;
        public float ActiveTime => activeTime;
        public float Cost => cost;
        public AbilityType Type => type;
        public ref readonly AbilityEffect[] Features => ref featureEffects;
        public ref readonly AbilityEffect[] Effects => ref effects;

        public virtual void Initialize() { }

        public virtual IEnumerator Trigger(GameObject owner) 
        {
            foreach (var feature in Features)
            {
                if(owner.TryGetComponent(out Character character))
                {
                    character.StartCoroutine(feature.Cast(owner));
                }
            }

            foreach (var effect in Effects)
            {
                if (owner.TryGetComponent(out Character character))
                {
                    character.StartCoroutine(effect.Cast(owner));
                }
            }

            yield return null; 
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
