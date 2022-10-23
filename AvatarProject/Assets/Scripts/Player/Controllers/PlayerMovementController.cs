using UnityEngine;

using AvatarBA.Common;
using AvatarBA.Managers;

namespace AvatarBA
{
    public class PlayerMovementController : CharacterMovementController
    {
        [SerializeField] private InputProvider _provider;

        [Header("Settings")]
        [SerializeField] private float _rotationSpeed = 0;

        [SerializeField] private LayerMask _terrainLayer;

        private Vector3 _movementDirection;
        private Vector3 _mousePosition;
        private Vector3 _desiredDirection;

        private Camera _gameplayCamera;

        private PlayerStatsController _statsController;

        private AnimationController _animationController;
        private readonly int MoveAnimation = Animator.StringToHash("Run");
        private const string MOVEMENT_STAT = "movementSpeed";

        protected override void Awake() 
        {
            base.Awake();
            _statsController = GetComponent<PlayerStatsController>();
            _animationController = GetComponent<AnimationController>();
            _gameplayCamera = Camera.main;
        }

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
        protected override void UpdateState()
        {
            InputState currentState = _provider.GetState();
            _desiredDirection = currentState.MovementDirection;
            _mousePosition = currentState.RotationDirection;
        }

        /// <summary>
        /// Calculates and moves the player.
        /// </summary>
        protected override void Move()
        {
            //Get the two axes from the camera and flatten them on the XZ plane
            Vector3 cameraForward = _gameplayCamera.transform.forward;
            cameraForward.y = 0f;
            Vector3 cameraRight = _gameplayCamera.transform.right;
            cameraRight.y = 0f;
            
            //Use the two axes, modulated by the corresponding inputs, and construct the final vector
            Vector3 adjustedMovement = cameraRight.normalized * _desiredDirection.x +
                cameraForward.normalized * _desiredDirection.z;

            _movementDirection = Vector3.ClampMagnitude(adjustedMovement, 1f);
            _movementDirection.y = 0f;

            if(_movementDirection != Vector3.zero)
                _animationController.PlayAnimation(MoveAnimation);
            else
                _animationController.PlayInitialAnimation();

            // Cache the current movement speed
            float movementSpeed = _statsController.GetStat(MOVEMENT_STAT) * Time.deltaTime;

            // Apply speed and calculate desire position
            Vector3 desiredVelocity = _movementDirection * movementSpeed;
            _characterController.Move(desiredVelocity);
        }
        
        /// <summary>
        /// Calculates and rotates the player
        /// </summary>
        protected override void Rotate()
        {
            // Get rotation from movement
            Vector3 targetDirection = _movementDirection;

            if(targetDirection == Vector3.zero)
            {
                Vector3 hitPoint = GetMousePosition();

                if(hitPoint == Vector3.zero)
                    return;
                
                targetDirection = hitPoint - transform.position;
            }
            
            // Calculate and apply new rotation
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            Quaternion desiredRotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
            transform.rotation = desiredRotation;
        }

        private Vector3 GetMousePosition()
        {
            RaycastHit hit;
            if(Physics.Raycast(_gameplayCamera.ScreenPointToRay(_mousePosition), out hit, 100, _terrainLayer))
            {
                return hit.point;
            }

            return Vector3.zero;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, transform.forward);
        }
    }
}

