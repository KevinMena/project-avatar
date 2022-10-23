using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using AvatarBA.Managers;

namespace AvatarBA
{
    public class MovementControl : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] 
        private InputProvider _provider;

        [Header("Data")]
        [SerializeField] 
        private float _rotationSpeed = 0;

        protected Core _core;
        protected CharacterController _controller;

        private Vector3 _movementDirection;
        private Vector3 _rotationDirection;
        private float _speed;
        protected bool _canMove = true;

        private readonly int MoveAnimation = Animator.StringToHash("Run");
        private const string MOVEMENT_SPEED_STAT = "movementSpeed";

        protected virtual void Awake()
        {
            _core = GetComponent<Core>();
            _controller = GetComponent<CharacterController>();
        }

        public virtual void DisableMovement() => _canMove = false;

        public virtual void EnableMovement() => _canMove = true;

        private void Update()
        {
            UpdateState();
            if (_canMove)
            {
                Move();
                Rotate();
            }
        }

        /// <summary>
        /// Ask the _provider to gather the current information about the inputs and 
        /// updates the corresponding information.
        /// </summary>
        protected void UpdateState()
        {
            InputState currentState = _provider.GetState();
            _movementDirection = currentState.MovementDirection;
            _rotationDirection = currentState.RotationDirection;
            _speed = currentState.Speed;
        }

        /// <summary>
        /// Calculates and moves the player.
        /// </summary>
        protected void Move()
        {
            // If not velocity just not move at all
            if (_movementDirection == Vector3.zero)
            {
                _core.Animation.PlayInitialAnimation(MoveAnimation);
                return;
            }

            //Set animation
            _core.Animation.PlayAnimation(MoveAnimation);

            // Cache the current movement speed
            if (_speed == 0)
                _speed = _core.Stats.GetStat(MOVEMENT_SPEED_STAT);

            // Apply speed and calculate desire position
            Vector3 desiredVelocity = _movementDirection * _speed * Time.deltaTime;
            _controller.Move(desiredVelocity);
            _speed = 0;
        }

        /// <summary>
        /// Calculates and rotates the player
        /// </summary>
        protected void Rotate()
        {
            if (_rotationDirection == Vector3.zero)
                return;

            // Calculate and apply new rotation
            Quaternion targetRotation = Quaternion.LookRotation(_rotationDirection);
            Quaternion desiredRotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
            transform.rotation = desiredRotation;
        }

        public void LoseControl(float loseTime)
        {
            StartCoroutine(LoseControlCO(loseTime));
        }

        private IEnumerator LoseControlCO(float loseTime)
        {
            float finishedTime = Time.time + loseTime;

            DisableMovement();

            while (Time.time < finishedTime)
            {
                // Lose control effects
                yield return null;
            }

            EnableMovement();
        }

        public void Impulse(Vector3 direction, float speed)
        {
            Vector3 desiredVelocity = direction * speed * Time.deltaTime;
            _controller.Move(desiredVelocity);
        }
    }
}