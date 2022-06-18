using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace AvatarBA
{
    [CreateAssetMenu(fileName = "Dash Ability", menuName ="Abilities/Dash")]
    public class DashAbility : Ability
    {
        [SerializeField] 
        private float _dashSpeed = 0;

        [SerializeField] 
        private float _dashTime = 0;

        public override void Initialize() { }

        public override IEnumerator Trigger(CharactersController owner, CharactersController target)
        {
            CharacterMovementController controller = owner.MovementController;

            // Calculate correct direction base on where the camera is looking
            Vector3 desiredDirection = owner.transform.forward;

            // Apply speed and calculate desire position
            Vector3 desiredVelocity = desiredDirection * _dashSpeed;

            controller.LoseControl(_dashTime);

            controller.AddForce(desiredVelocity, ForceMode.VelocityChange);

            yield return null;
        }
    }
}