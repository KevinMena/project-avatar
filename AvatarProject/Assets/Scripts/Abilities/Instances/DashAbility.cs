using System.Collections;
using UnityEngine;

namespace AvatarBA.Abilities
{
    [CreateAssetMenu(fileName = "Ability_Dash_", menuName ="Abilities/Dash")]
    public class DashAbility : Ability
    {
        [SerializeField] 
        private float _dashSpeed = 0;

        [SerializeField] 
        private float _dashTime = 0;

        public override void Initialize() { }

        public override IEnumerator Trigger(GameObject owner)
        {
            if(owner.TryGetComponent(out CharacterMovementController movementController))
            {
                // Calculate correct direction base on where the camera is looking
                Vector3 desiredDirection = owner.transform.forward;

                // Apply speed and calculate desire position
                Vector3 desiredVelocity = desiredDirection * _dashSpeed * Time.deltaTime;

                movementController.DisableMovement();
                
                if(owner.TryGetComponent(out Character character))
                    character.BecomeInvulnerable();
                
                float dashTimer = Time.time + _dashTime;
                while(Time.time < dashTimer)
                {
                    movementController.AddMovement(desiredVelocity);
                    yield return null;
                }

                movementController.EnableMovement();
            }
        }
    }
}