using System.Collections;
using AvatarBA.Combat;
using UnityEngine;

namespace AvatarBA.Abilities.Effects
{
    [System.Serializable]
    public class AreaAttackEffect : DamageEffect
    {
        [SerializeField]
        protected AreaAttack m_Prefab;
        [SerializeField]
        protected LayerMask m_Mask;
        [SerializeField]
        private float m_BaseDistance = 0;
        [SerializeField]
        private float m_AttackTime = 0;

        public override void Execute(ExecutionContext context)
        {
            Core owner = context.Owner;

            owner.StartCoroutine(Trigger(owner));
        }

        private IEnumerator Trigger(Core owner)
        {
            // Get position and rotation to spawn
            Vector3 spawnPos = owner.transform.position;
            Quaternion spawnRot = Quaternion.LookRotation(owner.Movement.AimDirection);

            // Create VFX
            // Instantiate area of attack
            AreaAttack attackArea = GameObject.Instantiate(m_Prefab, spawnPos, spawnRot);

            // Generate attack prefab
            // Calculate damage for the attack
            float currentDamage = CalculateDamage(owner);
            attackArea.Setup(Id, new Vector3(0.5f, 0.5f, m_BaseDistance / 2), m_BaseDistance, currentDamage, m_Mask, owner.gameObject);

            // Start attacking the area
            attackArea.StartAttack();
            yield return new WaitForSeconds(m_AttackTime);
            attackArea.StopAttack();

            yield return null;
        }
    }
}

