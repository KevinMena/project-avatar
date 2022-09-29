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

        private const string ATTACK_STAT = "attackPower";

        public override IEnumerator Trigger(GameObject owner)
        {
            Vector3 shootPosition = owner.transform.position;

            if(owner.TryGetComponent(out Character character))
            {
                shootPosition = character.ShootPosition.position;
            }
            
            GameObject iceball = Instantiate(_prefab, shootPosition, owner.transform.rotation);
            // Setup projectile data
            if(iceball.TryGetComponent(out Projectile projectile))
            {
                // Calculate damage for the projectile
                float projectileDamage = _baseDamage;
                if (owner.TryGetComponent(out CharacterStatsController statsController))
                    projectileDamage += statsController.GetStat(ATTACK_STAT);
                projectile.Setup(projectileDamage, owner);
            }

            return base.Trigger(owner);
        }
    }
}