using System.Collections;
using UnityEngine;

using AvatarBA.Combat;

namespace AvatarBA.Abilities
{
    [CreateAssetMenu(fileName = "Ability_WaterWhip", menuName = "Abilities/Water/Water Whip")]
    public class WaterWhip : Ability
    {
        [SerializeField]
        private GameObject _prefab;
        [SerializeField]
        private float _baseDamage = 0;
        [SerializeField]
        private float _baseDistance = 0;
        [SerializeField]
        private float _attackTime = 0;
        [SerializeField]
        private LayerMask _mask;

        private const string ATTACK_STAT = "attackPower";

        public override IEnumerator Trigger(GameObject owner)
        {
            // Create VFX
            // Instantiate area of attack
            GameObject waterWhip = Instantiate(_prefab, owner.transform.position, owner.transform.rotation);

            // Generate attack prefab
            if (waterWhip.TryGetComponent(out BoxAreaAttack attack))
            {
                // Calculate damage for the attack
                float currentDamage = _baseDamage;
                if (owner.TryGetComponent(out CharacterStatsController statsController))
                    currentDamage += statsController.GetStat(ATTACK_STAT);
                attack.Setup(Name, new Vector3(0.5f, 0.5f, _baseDistance / 2), _baseDistance, currentDamage, _mask, owner);

                // Start attacking the area
                attack.StartAttack();
                yield return new WaitForSeconds(_attackTime);
                attack.StopAttack();
            }

            yield return base.Trigger(owner);
        }
    }
}