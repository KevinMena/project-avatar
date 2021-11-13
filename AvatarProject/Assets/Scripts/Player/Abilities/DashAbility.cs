using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace AvatarBA
{
    [CreateAssetMenu(fileName = "Dash Ability", menuName ="Abilities/Dash")]
    public class DashAbility : Ability
    {
        [SerializeField] private float _dashSpeed = 0;

        [SerializeField] private float _dashTime = 0;

        public override void Initialize() { }

        public override void Trigger()
        {
            if(state == AbilityState.cooldown) return;

            state = AbilityState.cooldown;
        }

        public IEnumerator TriggerCO(Rigidbody caller, UnityAction<float> OnLoseControl)
        {
            // Calculate correct direction base on where the camera is looking
            Vector3 desiredDirection = caller.transform.forward;

            // Apply speed and calculate desire position
            Vector3 desiredVelocity = desiredDirection * _dashSpeed;

            OnLoseControl.Invoke(_dashTime);

            caller.AddForce(desiredVelocity, ForceMode.VelocityChange);

            yield return null;
        }
    }
}