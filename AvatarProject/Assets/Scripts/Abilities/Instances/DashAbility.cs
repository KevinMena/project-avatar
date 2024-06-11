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
        private float _dashDistance = 0;

        public override void Initialize() { }

        public override IEnumerator Trigger(GameObject owner)
        {
            if(owner.TryGetComponent(out Core ownerCore))
            {
                // Calculate correct direction base on where the owner is looking
                Vector3 targetPosition = owner.transform.position + (ownerCore.Movement.AimDirection * _dashDistance);

                if (owner.TryGetComponent(out Health health))
                    health.BecomeInvulnerable();

                ownerCore.Movement.DisableMovement();

                ownerCore.Movement.Impulse(ownerCore.Movement.AimDirection, _dashSpeed);

                float cSquared;

                do
                {
                    Vector3 offset = targetPosition - owner.transform.position;
                    offset.y = 0;
                    cSquared = offset.Distance();
                    yield return null;
                }while (cSquared > 0.1f);

                ownerCore.Movement.EnableMovement();
            }
            yield return null;
        }
    }
}