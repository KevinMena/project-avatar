using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AvatarBA.NPC
{
    public class NPCMovementController : CharacterMovementController
    {
        [Header("References")]
        [SerializeField] private MovementInputProvider _provider;

        // TODO: Change this to use stats controller for NPC
        [SerializeField]
        private float _movementSpeed;
        [SerializeField]
        private float _rotationSpeed;

        private Vector3 _desiredPosition;

        private void Update()
        {
            UpdateState();
        }

        private void FixedUpdate()
        {
            if (_canMove)
            {
                Rotate();
                Move();
            }
        }

        protected override void Move()
        {
            Vector3 movementDirection = _desiredPosition - transform.position;

            if (movementDirection.sqrMagnitude < 0.1f)
            {
                _rigidbody.velocity = Vector3.zero;
                return;
            }

            movementDirection.Normalize();

            // Cache velocity last frame
            Vector3 previousVelocity = _rigidbody.velocity;

            // Apply speed and calculate desire position
            Vector3 desiredVelocity = movementDirection * _movementSpeed;
            Vector3 velocityChange = desiredVelocity - previousVelocity;
            _rigidbody.AddForce(velocityChange, ForceMode.VelocityChange);
        }

        protected override void Rotate()
        {
            if (Vector3.Distance(transform.position, _desiredPosition) < 0.1f)
                return;

            // Get rotation from movement
            Vector3 targetDirection = _desiredPosition - transform.position;

            if (targetDirection.sqrMagnitude < 0.1f)
                return;

            // Calculate and apply new rotation
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            Quaternion desiredRotation = Quaternion.Slerp(_rigidbody.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
            _rigidbody.rotation = desiredRotation;
        }

        protected override void UpdateState()
        {
            InputState currentState = _provider.GetState();
            _desiredPosition = currentState.movementDirection;
        }
    }
}
