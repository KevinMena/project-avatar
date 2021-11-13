using System.Collections;
using UnityEngine;

namespace AvatarBA
{
    public class PlayerMovementController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private MovementInputProvider _provider;

        [SerializeField] private DashAbility _dashAbility;

        [Header("Settings")]
        [SerializeField] private Vector3 _movementDirection;

        [SerializeField] private Vector3 _mousePosition;

        [SerializeField] private float _rotationSpeed = 0;

        [SerializeField] private LayerMask _terrainLayer;

        private Vector3 _desiredDirection;

        private Camera _gameplayCamera;

        private Rigidbody _rigidbody;

        private PlayerStatsController _statsController;
        
        private bool _canMove = true;

        public bool CanMove
        {
            get => _canMove;
            set => _canMove = value;
        }

        private void Awake() 
        {
            _rigidbody = GetComponent<Rigidbody>();
            _statsController = GetComponent<PlayerStatsController>();
            _gameplayCamera = Camera.main;
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
        }

        private void FixedUpdate() 
        {
            if(_canMove)
            {
                Move();
                Rotate();
            }
        }

        /// <summary>
        /// Ask the _provider to gather the current information about the inputs and 
        /// updates the corresponding information.
        /// </summary>
        private void UpdateState()
        {
            InputState currentState = _provider.GetState();
            _desiredDirection.x = currentState.movementDirection.x;
            _desiredDirection.z = currentState.movementDirection.y;
            _mousePosition = currentState.mousePosition;
        }

        /// <summary>
        /// Calculates and moves the player.
        /// </summary>
        private void Move()
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

            // Cache the current movement speed
            float movementSpeed = _statsController.GetStatValue("movementSpeed");

            // Cache velocity last frame
            Vector3 previousVelocity = _rigidbody.velocity;

            // Apply speed and calculate desire position
            Vector3 desiredVelocity = _movementDirection * movementSpeed;
            Vector3 velocityChange = desiredVelocity - previousVelocity;
            _rigidbody.AddForce(velocityChange, ForceMode.VelocityChange);
        }
        
        /// <summary>
        /// Calculates and rotates the player
        /// </summary>
        private void Rotate()
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
            Quaternion desiredRotation = Quaternion.Slerp(_rigidbody.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
            _rigidbody.rotation = desiredRotation;
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

        private void OnDash()
        {
            if(_dashAbility.state == AbilityState.cooldown) return;

            _dashAbility.Trigger();
            StartCoroutine(_dashAbility.TriggerCO(_rigidbody, 
                                                t => StartCoroutine(LoseControl(t))
                                                ));
            StartCoroutine(_dashAbility.CooldownCountdown());
        }

        private IEnumerator LoseControl(float loseTime)
        {
            float finishedTime = Time.time + loseTime;

            CanMove = false;

            while(Time.time < finishedTime)
            {
                // Lose control effects
                yield return null;
            }

            CanMove = true;
        }
    }
}

