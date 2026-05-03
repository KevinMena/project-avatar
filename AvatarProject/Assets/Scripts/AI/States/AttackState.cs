using System.Collections.Generic;
using AvatarBA.Combat;
using AvatarBA.Interfaces;
using AvatarBA.Patterns;
using AvatarBA.Stats;
using UnityEngine;

namespace AvatarBA.AI
{
    [System.Serializable]
    public class AttackState : BaseState
    {
        [SerializeField]
        private string m_NextStateId;
        [SerializeField]
        private CombatStateData m_Data;

        [SerializeField]
        private LayerMask m_HittableLayer;

        [SerializeField]
        private StatDefinition m_AttackStat;

        [SerializeField]
        private float m_MeleeRange;

        private ConeHitbox m_Hitbox;

        private readonly int m_AnimationHash;
        private float m_AttackDuration = 0;
        private float m_AttackDamage = 0;

        private float m_StartupDuration = 0;
        private float m_ActiveDuration = 0;

        private List<Collider> m_AlreadyHit;

        private float m_Timer = 0;

        public override void Setup(Core owner, Sensors ownerSensors)
        {
            base.Setup(owner, ownerSensors);
            m_AlreadyHit = new List<Collider>();
            m_Hitbox = new ConeHitbox(m_Data.AttackRange, m_Data.AttackAngle, m_HittableLayer);
            m_Hitbox.OnCollision += CollisionedWith;
        }

        public override void OnEnter()
        {
            m_Timer = 0;
            m_AttackDuration = 1f;
            m_StartupDuration = m_AttackDuration * 0.1f;
            m_ActiveDuration = m_AttackDuration * 0.8f;
            m_AttackDamage = m_Owner.Stats.GetStat(m_AttackStat.Id);

            m_Hitbox.StartCheckCollision();
        }

        public override void OnUpdate()
        {
            m_Timer += Time.deltaTime;

            // If the attack is just starting, then do nothing
            if (m_Timer < m_StartupDuration)
                return;

            // Window of attack activated
            if (m_Timer < m_ActiveDuration)
                Attack();
        }

        public override void OnExit()
        {
            m_Hitbox.StopCheckCollision();
            m_AlreadyHit.Clear();
        }

        public override string ShouldTransition()
        {
            // If the attack is done then we transition to the transition state.
            if (m_Timer > m_AttackDuration)
            {
                return m_NextStateId;
            }

            return base.ShouldTransition();
        }

        public void Attack()
        {
            m_Hitbox.Position = m_Owner.HitPoint.position;
            m_Hitbox.Forward = m_Owner.HitPoint.forward;
            m_Hitbox.Right = m_Owner.HitPoint.right;
            m_Hitbox.CheckCollision();
        }

        public void CollisionedWith(Collider hit)
        {
            if (m_AlreadyHit.Contains(hit))
                return;

            m_AlreadyHit.Add(hit);

            if (hit.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(m_AttackDamage);
            }
        }
    }
}
