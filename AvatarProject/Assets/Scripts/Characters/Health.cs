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
        private HealthMiddleware m_displayMiddleware = null;

        [Header("Data")]
        [SerializeField]
        private float m_invulnerableDuration = 0.5f;

        [SerializeField]
        private float m_invulnerableDelta = 0.15f;

        private Core m_core;
        private Stat m_health;
        private Stat m_maxHealth;
        private bool m_isInvulnerable = false;

        private const string DEFENSE_STAT = "defense";

        public float Current
        {
            get
            {
                if (m_health.Value < 0)
                    return 0;
                else if (m_health.Value > m_maxHealth.Value)
                    return Maximum;
                return m_health.Value;
            }
        }

        public float DefenseValue
        {
            get
            {
                if(m_core.Stats == null)
                    return 0;
                float value = m_core.Stats.GetStat(DEFENSE_STAT);

                if(value < 0)
                    return 0;
                return value;
            }
        }

        public float Maximum => m_maxHealth.Value;

        private void Awake()
        {
            m_core = GetComponent<Core>();
        }

        private void Start()
        {
            m_health = new Stat("Health", m_core.Data.BaseHealth);
            m_maxHealth = new Stat("Max Health", m_core.Data.BaseHealth);
            m_displayMiddleware?.Setup(m_core.Data.BaseHealth);
        }

        public void TakeDamage(float damage)
        {
            // Damage formula
            float appliedDamage = damage / DefenseValue;
            StatModifier damageModifier = new StatModifier("damage", -appliedDamage, StatModifierType.Flat);
            m_health.AddModifier(damageModifier);
            //Update UI
            m_displayMiddleware?.UpdateHealth(Current);
            // Check if character is dead
            CheckDeath();
        }

        public void TakeHit(float damage, Vector2 hitDirection)
        {
            TakeDamage(damage);
            // Throw entity back
        }

        private void CheckDeath()
        {
            if(Current == 0)
            {
                m_core.gameObject.SetActive(false);
            }
        }

        public void BecomeInvulnerable()
        {
            if (!m_isInvulnerable)
                StartCoroutine(Invulnerable());
        }

        private IEnumerator Invulnerable()
        {
            m_isInvulnerable = true;

            for (float i = 0; i < m_invulnerableDuration; i += m_invulnerableDelta)
            {
                yield return new WaitForSeconds(m_invulnerableDelta);
            }

            m_isInvulnerable = false;
        }
    }
}

