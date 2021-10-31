using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AvatarBA
{
    public class CharacterMovement : MonoBehaviour
    {
        [SerializeField] MovementInputProvider provider;

        [SerializeField] float runningSpeed = 5f;

        [SerializeField] Vector3 _inputMovDirection;

        private Transform cameraTransform;

        private Rigidbody rb;

        void Awake() 
        {
            rb = GetComponent<Rigidbody>();
            cameraTransform = Camera.main.transform;
        }

        void Update()
        {
            UpdateState();
        }

        void FixedUpdate() 
        {
            Move();
        }

        private void UpdateState()
        {
            InputState currentState = provider.GetState();
            _inputMovDirection.x = currentState.movementDirection.x;
            _inputMovDirection.z = currentState.movementDirection.y;
        }

        private void Move()
        {
            // Initial position
            Vector3 currentPosition = rb.position;

            // Calculate correct direction base on where the camera is looking
            Vector3 movementDirection = cameraTransform.forward * _inputMovDirection.z;
            movementDirection = movementDirection + cameraTransform.right *_inputMovDirection.x;
            movementDirection.Normalize();
            movementDirection.y = 0;

            // Apply speed and calculate desire position
            Vector3 velocity = movementDirection * runningSpeed;
            Vector3 newPosition = currentPosition + velocity * Time.deltaTime;
            rb.MovePosition(newPosition);
        }
    }
}
