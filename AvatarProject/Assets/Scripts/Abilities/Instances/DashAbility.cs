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

        [SerializeField]
        private float _dashDistance = 0;

        public override void Initialize() { }

        public override IEnumerator Trigger(GameObject owner)
        {
            if(owner.TryGetComponent(out CharacterMovementController movementController))
            {
                // Calculate correct direction base on where the owner is looking
                Vector3 targetPosition = owner.transform.position + (owner.transform.forward * _dashDistance);

                // Calculate the direction where the movement is going to be
                Vector3 targetDirection = targetPosition - owner.transform.position;

                movementController.DisableMovement();
                
                if(owner.TryGetComponent(out Character character))
                    character.BecomeInvulnerable();

                while(targetDirection.magnitude > 0.1f)
                {
                    Vector3 movementDirection = _dashSpeed * Time.deltaTime * targetDirection.normalized;
                    movementController.AddMovement(movementDirection);
                    targetDirection = targetPosition - owner.transform.position;
                    yield return null;
                }

                movementController.EnableMovement();
            }
        }
    }
}