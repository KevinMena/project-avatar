using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace AvatarBA
{
    [CreateAssetMenu(fileName = "Dash Ability", menuName ="Abilities/Dash")]
    public class DashAbility : Ability
    {
        public override void Initialize() { }

        public override void Trigger()
        {
            if(state == AbilityState.cooldown) return;

            state = AbilityState.cooldown;
        }

        public IEnumerator TriggerCO(GameObject caller, UnityAction<Vector3> OnMove, bool canMove, float dashSpeed, float dashTime)
        {
            canMove = false;
            
            // Calculate correct direction base on where the camera is looking
            Vector3 targetDirection = caller.gameObject.transform.forward;

            // Apply speed and calculate desire position
            Vector3 velocity = targetDirection * dashSpeed;

            float finishedTime = Time.time + dashTime;
            
            while(Time.time < finishedTime)
            {
                OnMove.Invoke(velocity * Time.deltaTime);
                yield return null;
            }

            canMove = true;
        }
    }
}