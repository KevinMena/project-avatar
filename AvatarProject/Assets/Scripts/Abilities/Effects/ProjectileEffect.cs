using AvatarBA.Combat;
using UnityEngine;

namespace AvatarBA.Abilities.Effects
{
    [System.Serializable]
    public class ProjectileEffect : DamageEffect
    {
        [SerializeField]
        protected Projectile m_Prefab;

        [SerializeField]
        protected LayerMask m_Mask;

        public override void Execute(ExecutionContext context)
        {
            Core owner = context.Owner;

            Vector3 shootPosition = owner.ShootPosition.position;

            // Calculate rotation of the projectile so always lands where the user is looking towards
            Quaternion projectileRotation = Quaternion.LookRotation(owner.Movement.AimDirection);

            if (Physics.Raycast(owner.transform.position, owner.Movement.AimDirection, out RaycastHit hit, 500f, m_Mask))
            {
                Vector3 direction = (hit.point - shootPosition).normalized;
                projectileRotation = Quaternion.LookRotation(direction);
            }

            Projectile projectile = GameObject.Instantiate(m_Prefab, shootPosition, projectileRotation);

            // Setup projectile data
            float projectileDamage = CalculateDamage(owner);
            projectile.Setup(projectileDamage, m_Mask, owner.gameObject);

        }
    }
}
