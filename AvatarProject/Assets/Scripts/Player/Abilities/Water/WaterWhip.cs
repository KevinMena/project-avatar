using System.Collections;
using UnityEngine;

using AvatarBA.Combat;

namespace AvatarBA.Abilities
{
    [CreateAssetMenu(fileName = "Ability_WaterWhip", menuName = "Abilities/Water/Water Whip")]
    public class WaterWhip : DamageAbility
    {
        [SerializeField]
        private float _baseDistance = 0;
        [SerializeField]
        private float _attackTime = 0;

        public override void Initialize() { }

        public override IEnumerator Trigger(GameObject owner)
        {
            // Get position and rotation to spawn
            Vector3 spawnPos = owner.transform.position;
            Quaternion spawnRot = owner.transform.rotation;

            if(owner.TryGetComponent(out Core ownerCore))
            {
                spawnRot = Quaternion.LookRotation(ownerCore.Movement.AimDirection);
            }

            // Create VFX
            // Instantiate area of attack
            GameObject waterWhip = Instantiate(Prefab, spawnPos, spawnRot);

            // Generate attack prefab
            if (waterWhip.TryGetComponent(out BoxAreaAttack attack))
            {
                // Calculate damage for the attack
                float currentDamage = CalculateDamage(owner);
                attack.Setup(Name, new Vector3(0.5f, 0.5f, _baseDistance / 2), _baseDistance, currentDamage, _mask, owner);

                // Start attacking the area
                attack.StartAttack();
                yield return new WaitForSeconds(_attackTime);
                attack.StopAttack();
            }

            yield return null;
        }
    }
}