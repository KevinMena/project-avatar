using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using AvatarBA.Managers;

namespace AvatarBA.NPC
{
    public class NPCMovementController : CharacterMovementController
    {
        [Header("References")]
        [SerializeField] private InputProvider _provider;

        // TODO: Change this to use stats controller for NPC
        [SerializeField]
        private float _movementSpeed;
        [SerializeField]
        private float _rotationSpeed;

        private Vector3 _targetPosition;
        private Vector3 _targetRotation;

        private void Update()
        {
            UpdateState();
            if (_canMove)
            {
                Rotate();
                Move();
            }
        }

        protected override void Move()
        {
            Vector3 movementDirection = _targetPosition - transform.position;

            if (movementDirection.magnitude < 0.1f)
                return;

            movementDirection.Normalize();
            movementDirection.y = 0;

            float speed = _movementSpeed * Time.deltaTime;

            Vector3 desiredVelocity = movementDirection * speed;
            _characterController.Move(desiredVelocity);
        }

        protected override void Rotate()
        {
            // Get rotation from movement
            Vector3 rotationDirection = _targetRotation - transform.position;
            rotationDirection.y = 0;

            if (rotationDirection.sqrMagnitude < 0.1f)
                return;

            // Calculate and apply new rotation
            Quaternion targetRotation = Quaternion.LookRotation(rotationDirection);
            Quaternion desiredRotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
            transform.rotation = desiredRotation;
        }

        protected override void UpdateState()
        {
            InputState currentState = _provider.GetState();
            _targetPosition = currentState.MovementDirection;
            _targetRotation = currentState.RotationDirection;
        }
    }
}
