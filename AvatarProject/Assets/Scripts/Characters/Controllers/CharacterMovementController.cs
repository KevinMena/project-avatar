using System.Collections;
using UnityEngine;

namespace AvatarBA
{
    [RequireComponent(typeof(Rigidbody))]
    public abstract class CharacterMovementController : MonoBehaviour
    {
        protected Rigidbody _rigidbody;

        protected bool _canMove = true;

        protected virtual void Awake() 
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        public void DisableMovement() => _canMove = false;
        public void EnableMovement() => _canMove = true;

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

        public void AddForce(Vector3 velocity, ForceMode mode)
        {
            _rigidbody.AddForce(velocity, mode);
        }
    }
}
