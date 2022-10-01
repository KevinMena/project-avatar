using System.Collections;
using UnityEngine;

using AvatarBA.Combat;

namespace AvatarBA.Abilities
{
    [CreateAssetMenu(fileName = "Ability_IceBall", menuName = "Abilities/Water/Ice Ball")]
    public class IceBall : Ability
    {
        [SerializeField]
        private GameObject _prefab;

        [SerializeField]
        private float _baseDamage;

        [SerializeField]
        private LayerMask _mask;

        private const string ATTACK_STAT = "attackPower";

        public override IEnumerator Trigger(GameObject owner)
        {
            Vector3 shootPosition = owner.transform.position;

            if(owner.TryGetComponent(out Character character))
            {
                shootPosition = character.ShootPosition.position;
            }

            // Calculate rotation of the projectile so always lands where the user is looking towards
            Quaternion projectileRotation = owner.transform.rotation;
            RaycastHit hit;
            if(Physics.Raycast(owner.transform.position, owner.transform.forward, out hit, 500f, _mask))
            {
                Vector3 direction = (hit.point - shootPosition).normalized;
                projectileRotation = Quaternion.LookRotation(direction);
            }
            
            GameObject iceball = Instantiate(_prefab, shootPosition, projectileRotation);
            // Setup projectile data
            if(iceball.TryGetComponent(out Projectile projectile))
            {
                // Calculate damage for the projectile
                float projectileDamage = _baseDamage;
                if (owner.TryGetComponent(out CharacterStatsController statsController))
                    projectileDamage += statsController.GetStat(ATTACK_STAT);
                projectile.Setup(projectileDamage, _mask, owner);
            }

            return base.Trigger(owner);
        }
    }
}