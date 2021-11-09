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

        public IEnumerator TriggerCO(GameObject caller, UnityAction<Vector3> OnMove, bool canMove)
        {
            canMove = false;
            
            // Calculate correct direction base on where the camera is looking
            Vector3 targetDirection = caller.gameObject.transform.forward;

            // Apply speed and calculate desire position
            Vector3 velocity = targetDirection * _dashSpeed;

            float finishedTime = Time.time + _dashTime;
            
            while(Time.time < finishedTime)
            {
                OnMove.Invoke(velocity * Time.deltaTime);
                yield return null;
            }

            canMove = true;
        }
    }
}