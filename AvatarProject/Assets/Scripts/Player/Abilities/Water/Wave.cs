using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using AvatarBA.Combat;

namespace AvatarBA.Abilities
{
    [CreateAssetMenu(fileName = "Ability_Wave", menuName = "Abilities/Water/Wave")]
    public class Wave : DamageAbility
    {
        [SerializeField]
        private float _pushBack;
        [SerializeField]
        private float _baseDistance = 0;
        [SerializeField]
        private float _attackTime = 0;

        public override void Initialize() { }

        public override IEnumerator Trigger(GameObject owner)
        {
            GameObject wave = Instantiate(Prefab, owner.transform.position, owner.transform.rotation);

            // Generate attack prefab
            if (wave.TryGetComponent(out PushBoxAreaAttack attack))
            {
                // Calculate damage for the attack
                float currentDamage = CalculateDamage(owner);
                attack.Setup(Name, new Vector3(1f, 0.5f, _baseDistance / 2), _baseDistance, currentDamage, _pushBack, _mask, owner);

                // Start attacking the area
                attack.StartAttack();
                yield return new WaitForSeconds(_attackTime);
                attack.StopAttack();
            }
            yield return null;
        }
    }
}