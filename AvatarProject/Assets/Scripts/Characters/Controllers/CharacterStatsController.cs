using UnityEngine;

using AvatarBA.Stats;

namespace AvatarBA
{    
    public class CharacterStatsController : MonoBehaviour, IStatContainer
    {
        [Header("References")]
        [SerializeField] 
        protected CharacterData _characterData;

        private Stat _health;
        private Stat _attackPower;
        private Stat _attackSpeed;
        private Stat _defense;
        private Stat _movementSpeed;
        private Stat _spiritPower;

        public float Health => _health != null ? _health.Value: 0;
        public float AttackPower => _attackPower != null ? _attackPower.Value : 0;
        public float AttackSpeed => _attackSpeed != null ? _attackSpeed.Value : 0;
        public float Defense => _defense != null ? _defense.Value : 0;
        public float MovementSpeed => _movementSpeed != null ? _movementSpeed.Value : 0;
        public float SpiritPower => _spiritPower != null ? _spiritPower.Value : 0;

        public float BaseHealth => _health != null ? _health.BaseValue : 0;
        public float BaseAttackPower => _attackPower != null ? _attackPower.BaseValue : 0;
        public float BaseAttackSpeed => _attackSpeed != null ? _attackSpeed.BaseValue : 0;
        public float BaseDefense => _defense != null ? _defense.BaseValue : 0;
        public float BaseMovementSpeed => _movementSpeed != null ? _movementSpeed.BaseValue : 0;
        public float BaseSpiritPower => _spiritPower != null ? _spiritPower.BaseValue : 0;

        protected virtual void Start()
        {
            CreateRuntimeValues();
        }

        public void CreateRuntimeValues()
        {
            _health = new Stat(_characterData.BaseHealth);
            _attackPower = new Stat(_characterData.BaseAttackPower);
            _attackSpeed = new Stat(_characterData.BaseAttackSpeed);
            _defense = new Stat(_characterData.BaseDefense);
            _movementSpeed = new Stat(_characterData.BaseMovementSpeed);
            _spiritPower = new Stat(_characterData.BaseSpiritPower);
        }

        public void ApplyChangeToStat(Stat stat, float value, StatModifierType modifierType)
        {
            StatModifier modifier = new StatModifier(value, modifierType);
            stat.AddModifier(modifier);
        }

        public void RemoveChangeToStat(Stat stat, float value, StatModifierType modifierType)
        {
            StatModifier modifier = new StatModifier(value, modifierType);
            stat.RemoveModifier(modifier);
        }

        public void RemoveChangeToStatFromSource(Stat stat, object source)
        {
            stat.RemoveModifiersFromSource(source);
        }
    }
}
