using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AvatarBA
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] 
        private PlayerInformation _playerInformation;

        [SerializeField] 
        private MovementInputProvider _provider;

        [SerializeField] 
        private Vector3 _movementDirection;

        private Vector3 _previousMoveDirection;

        private Transform _gameplayCamera;

        private CharacterController _characterController;
        
        private bool _canMove = true;

        private float _dashCooldown = 0.7f;

        private bool _dashOnCooldown = false;

        private float _dashStartTime;

        private float _dashCooldownTime;

        private void Awake() 
        {
            _characterController = GetComponent<CharacterController>();
            _gameplayCamera = Camera.main.transform;
        }

        private void Start() 
        {
            _provider.OnDash += OnDash;
        }

        private void OnDisable() 
        {
            _provider.OnDash -= OnDash;
        }

        private void Update()
        {
            UpdateState();
            if(_canMove)
            {
                Move();
                Rotate();
            }

            if(_dashOnCooldown)
            {
                if(Time.time > _dashCooldownTime)
                    _dashOnCooldown = false;
            }
        }

        /// <summary>
        /// Ask the _provider to gather the current information about the inputs and 
        /// updates the corresponding information.
        /// </summary>
        private void UpdateState()
        {
            InputState currentState = _provider.GetState();
            _previousMoveDirection.x = currentState.movementDirection.x;
            _previousMoveDirection.z = currentState.movementDirection.y;
        }

        /// <summary>
        /// Calculates and moves the player.
        /// </summary>
        private void Move()
        {
            //Get the two axes from the camera and flatten them on the XZ plane
            Vector3 cameraForward = _gameplayCamera.forward;
            cameraForward.y = 0f;
            Vector3 cameraRight = _gameplayCamera.right;
            cameraRight.y = 0f;

            //Use the two axes, modulated by the corresponding inputs, and construct the final vector
            Vector3 adjustedMovement = cameraRight.normalized * _previousMoveDirection.x +
                cameraForward.normalized * _previousMoveDirection.z;

            _movementDirection = Vector3.ClampMagnitude(adjustedMovement, 1f);

            // Apply speed and calculate desire position
            Vector3 velocity = _movementDirection * _playerInformation.runningSpeed;
            _characterController.Move(velocity * Time.deltaTime);
        }
        
        /// <summary>
        /// Calculates and rotates the player
        /// </summary>
        private void Rotate()
        {
            // Get rotation from movement
            Vector3 targetDirection = _movementDirection;

            if(targetDirection == Vector3.zero)
                targetDirection = transform.forward;
            
            // Calculate and apply new rotation
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            Quaternion targetRotationDif = Quaternion.Slerp(transform.rotation, targetRotation, _playerInformation.rotationSpeed * Time.deltaTime);
            transform.rotation = targetRotationDif;
        }

        private void OnDash()
        {
            if(_dashOnCooldown) return;

            _dashOnCooldown = true;
            _dashStartTime = Time.time;
            _dashCooldownTime = _dashStartTime + _dashCooldown;
            StartCoroutine(Dash());
        }

        private IEnumerator Dash()
        {
            _canMove = false;
            // Calculate correct direction base on where the camera is looking
            Vector3 targetDirection = transform.forward;

            // Apply speed and calculate desire position
            Vector3 velocity = targetDirection * _playerInformation.dashingSpeed;

            float finishedTime = Time.time + _playerInformation.dashTime;
            
            while(Time.time < finishedTime)
            {
                _characterController.Move(velocity * Time.deltaTime);
                yield return null;
            }
            _canMove = true;
        }
    }
}

