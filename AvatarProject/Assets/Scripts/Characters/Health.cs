using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using AvatarBA.Stats;
using AvatarBA.Interfaces;
using AvatarBA.Managers;

namespace AvatarBA
{
    public class Health : MonoBehaviour, IDamageable
    {
        [Header("References")]
        [SerializeField]
        private HealthMiddleware _displayMiddleware = default;

        [Header("Data")]
        [SerializeField]
        private float _invulnerableDuration = 0.5f;

        [SerializeField]
        private float _invulnerableDelta = 0.15f;

        private Core _core;
        private Stat _health;
        private Stat _maxHealth;
        private bool _isInvulnerable = false;

        private const string DEFENSE_STAT = "defense";

        public float Current
        {
            get
            {
                if (_health.Value < 0)
                    return 0;
                else if (_health.Value > _maxHealth.Value)
                    return Maximum;
                return _health.Value;
            }
        }

        public float Maximum => _maxHealth.Value;

        private void Awake()
        {
            _core = GetComponent<Core>();
        }

        private void Start()
        {
            _health = new Stat("Health", _core.Data.BaseHealth);
            _maxHealth = new Stat("Max Health", _core.Data.BaseHealth);
            _displayMiddleware.Setup(_core.Data.BaseHealth);
        }

        public void TakeDamage(float damage)
        {
            // Damage formula
            float appliedDamage = damage / _core.Stats.GetStat(DEFENSE_STAT);
            StatModifier damageModifier = new StatModifier("damage", -appliedDamage, StatModifierType.Flat);
            _health.AddModifier(damageModifier);
            //Update UI
            _displayMiddleware.UpdateHealth(Current);
        }

        public void TakeHit(float damage, Vector2 hitDirection)
        {
            throw new System.NotImplementedException();
        }

        public void BecomeInvulnerable()
        {
            if (!_isInvulnerable)
                StartCoroutine(Invulnerable());
        }

        private IEnumerator Invulnerable()
        {
            _isInvulnerable = true;

            for (float i = 0; i < _invulnerableDuration; i += _invulnerableDelta)
            {
                yield return new WaitForSeconds(_invulnerableDelta);
            }

            _isInvulnerable = false;
        }
    }
}

