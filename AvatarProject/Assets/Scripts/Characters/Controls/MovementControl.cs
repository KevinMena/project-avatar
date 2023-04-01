using AvatarBA.Managers;
using System.Collections;
using UnityEngine;

namespace AvatarBA
{
    public class MovementControl : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField]
        protected float _rotationSpeed = 0;

        protected Core _core;
        protected CharacterController _controller;

        protected Vector3 _movementDirection;
        protected Vector3 _rotationDirection;
        protected float _speed;
        protected bool _canMove = true;

        private bool _inMovement = false;

        protected readonly int MoveAnimation = Animator.StringToHash("Run");
        protected const string MOVEMENT_SPEED_STAT = "movementSpeed";

        public void DisableMovement() => _canMove = false;
        public void EnableMovement() => _canMove = true;

        protected void Awake()
        {
            _core = GetComponent<Core>();
            _controller = GetComponent<CharacterController>();
        }

        protected virtual void Update()
        {
            if (_canMove)
            {
                Move();
                Rotate();
            }
        }

        /// <summary>
        /// Updates the current movement state
        /// </summary>
        public void UpdateState(InputState newState) 
        {
            _movementDirection = newState.MovementDirection;
            _rotationDirection = newState.RotationDirection;
            _speed = newState.Speed;
        }

        /// <summary>
        /// Calculates and moves the player.
        /// </summary>
        protected void Move()
        {
            // If not velocity just not move at all
            if (_inMovement && _movementDirection == Vector3.zero)
            {
                _core.Animation.PlayInitialAnimation(MoveAnimation);
                _inMovement = false;
                return;
            }

            if (_movementDirection == Vector3.zero)
                return;

            //Set animation
            _core.Animation.PlayAnimation(MoveAnimation);

            // Cache the current movement speed
            if (_speed == -1)
                _speed = _core.Stats.GetStat(MOVEMENT_SPEED_STAT);

            // Apply speed and calculate desire position
            Vector3 desiredVelocity = _speed * Time.deltaTime * _movementDirection;
            desiredVelocity.y = 0;
            _controller.Move(desiredVelocity);
            _inMovement = true;
        }

        /// <summary>
        /// Calculates and rotates the player
        /// </summary>
        protected void Rotate()
        {
            if (_rotationDirection == Vector3.zero)
                return;

            // Remove the Y axis from the vector we are looking at
            _rotationDirection.y = 0;

            // Calculate and apply new rotation
            Quaternion targetRotation = Quaternion.LookRotation(_rotationDirection);
            Quaternion desiredRotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
            transform.rotation = desiredRotation;
        }

        public void LoseControl(float loseTime)
        {
            StartCoroutine(LoseControlCO(loseTime));
        }

        protected IEnumerator LoseControlCO(float loseTime)
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