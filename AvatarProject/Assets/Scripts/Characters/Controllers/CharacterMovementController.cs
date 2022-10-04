using System.Collections;
using UnityEngine;

namespace AvatarBA
{
    [RequireComponent(typeof(CharacterController))]
    public abstract class CharacterMovementController : MonoBehaviour
    {
        protected CharacterController _characterController;

        protected bool _canMove = true;

        protected virtual void Awake() 
        {
            _characterController = GetComponent<CharacterController>();
        }

        public virtual void DisableMovement() => _canMove = false;
            
        public virtual void EnableMovement() => _canMove = true;

        protected abstract void UpdateState();

        protected abstract void Move();

        protected abstract void Rotate();

        public void LoseControl(float loseTime)
        {
            StartCoroutine(LoseControlCO(loseTime));
        }

        private IEnumerator LoseControlCO(float loseTime)
        {
            float finishedTime = Time.time + loseTime;

            DisableMovement();

            while(Time.time < finishedTime)
            {
                // Lose control effects
                yield return null;
            }

            EnableMovement();
        }

        public void AddMovement(Vector3 direction)
        {
            _characterController.Move(direction);
        }
    }
}
