using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AvatarBA
{
    public class CharacterMovementController : MonoBehaviour
    {
        // Not sure tho
        private CharactersController _controller;

        protected Rigidbody _rigidbody;

        protected bool _canMove = true;

        public CharactersController Controller => _controller;

        protected virtual void Awake() 
        {
            _rigidbody = GetComponent<Rigidbody>();
            _controller = GetComponent<CharactersController>();
        }

        public void DisableMovement() => _canMove = false;
        public void EnableMovement() => _canMove = true;

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
