using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace AvatarBA
{
    public class Ability : ScriptableObject
    {
        [SerializeField]
        protected Sprite _icon;

        [SerializeField]

        protected string _name;

        [SerializeField, TextArea]
        protected string _description;

        [SerializeField]
        protected float _cooldown;
        
        [SerializeField]
        protected float _activeTime;

        [SerializeField]
        protected float _cost;

        [SerializeField]
        protected AbilityType _type;

        [SerializeField]
        protected List<AbilityEffect> _effects = new List<AbilityEffect>();

        public Sprite Icon => _icon;
        public string Name  => _name;
        public string Description => _description;
        public float Cooldown => _cooldown;
        public float ActiveTime => _activeTime;
        public float Cost => _cost;
        public AbilityType Type => _type;
        public ref readonly List<AbilityEffect> Effects => ref _effects;

        public virtual void Initialize() { }
        public virtual IEnumerator Trigger(GameObject owner) { yield return null; }
        public virtual bool PassRequirements() { return true; }
    }

    public enum AbilityState
    {
        ready,
        active,
        cooldown   
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
