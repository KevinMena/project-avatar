using System.Collections;
using UnityEngine;

namespace AvatarBA.Abilities.Effects
{
    [CreateAssetMenu(fileName = "Effect_Dash", menuName = "Abilities/Effects/Dash Effect")]
    public class DashEffect : AbilityEffect
    {
        [SerializeField]
        private float _dashSpeed = 0;

        [SerializeField]
        private float _dashTime = 0;

        public override IEnumerator Cast(GameObject owner)
        {
            if (owner.TryGetComponent(out CharacterMovementController movementController))
            {
                // Calculate correct direction base on where the camera is looking
                Vector3 desiredDirection = owner.transform.forward;

                // Apply speed and calculate desire position
                Vector3 desiredVelocity = desiredDirection * _dashSpeed;

                //While dashing cannot move
                movementController.LoseControl(_dashTime);

                // Becomes invulnerable for X amount of frames
                if (owner.TryGetComponent(out Character character))
                    character.BecomeInvulnerable();

                movementController.AddForce(desiredVelocity, ForceMode.VelocityChange);
            }

            yield return null;
        }
    }
}
