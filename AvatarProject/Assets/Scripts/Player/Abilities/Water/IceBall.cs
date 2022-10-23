using System.Collections;
using UnityEngine;

using AvatarBA.Combat;

namespace AvatarBA.Abilities
{
    [CreateAssetMenu(fileName = "Ability_IceBall", menuName = "Abilities/Water/Ice Ball")]
    public class IceBall : DamageAbility
    {
        public override void Initialize() { }

        public override IEnumerator Trigger(GameObject owner)
        {
            Vector3 shootPosition = owner.transform.position;

            if(owner.TryGetComponent(out Core ownerCore))
            {
                shootPosition = ownerCore.ShootPosition.position;
            }

            // Calculate rotation of the projectile so always lands where the user is looking towards
            Quaternion projectileRotation = owner.transform.rotation;

            if(Physics.Raycast(owner.transform.position, owner.transform.forward, out RaycastHit hit, 500f, Mask))
            {
                Vector3 direction = (hit.point - shootPosition).normalized;
                projectileRotation = Quaternion.LookRotation(direction);
            }
            
            GameObject iceball = Instantiate(Prefab, shootPosition, projectileRotation);
            // Setup projectile data
            if(iceball.TryGetComponent(out Projectile projectile))
            {
                // Calculate damage for the projectile
                float projectileDamage = CalculateDamage(owner);
                projectile.Setup(projectileDamage, Mask, owner);
            }

            yield return null;
        }
    }
}