using AvatarBA.Managers;
using System.Collections;
using UnityEngine;
using static UnityEngine.GridBrushBase;

namespace AvatarBA.Abilities
{
    [CreateAssetMenu(fileName = "Ability_Dash_", menuName ="Abilities/Dash")]
    public class DashAbility : Ability
    {
        [SerializeField] 
        private float _dashSpeed = 0;

        [SerializeField] 
        private float _dashTime = 0;

        [SerializeField]
        private float _dashDistance = 0;

        public override void Initialize() { }

        public override IEnumerator Trigger(GameObject owner)
        {
            if(owner.TryGetComponent(out Core ownerCore))
            {
                // Calculate correct direction base on where the owner is looking
                Vector3 targetPosition = owner.transform.position + (owner.transform.forward * _dashDistance);

                if (owner.TryGetComponent(out Health health))
                    health.BecomeInvulnerable();

                ownerCore.Movement.DisableMovement();

                Vector3 offset = targetPosition - owner.transform.position;
                offset.y = 0;

                float cSquared = offset.x * offset.x + offset.z * offset.z;

                while (cSquared > 0.1f)
                {
                    ownerCore.Movement.Impulse(offset.normalized, _dashSpeed);
                    yield return null;
                    offset = targetPosition - owner.transform.position;
                    offset.y = 0;
                    cSquared = offset.x * offset.x + offset.z * offset.z;
                }

                ownerCore.Movement.EnableMovement();
            }
            yield return null;
        }
    }
}