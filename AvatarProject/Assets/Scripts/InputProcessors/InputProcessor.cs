using UnityEngine;

namespace AvatarBA.Managers
{
    public class InputProcessor : Processor
    {
        [Header("Input References")]
        [SerializeField]
        private InputManager _inputManager = default;

        [Header("Values")]
        [SerializeField] 
        private LayerMask _terrainLayer;

        // Movement variables
        private Vector3 _movementDirection;
        private Vector3 _rotationDirection;

        private Camera _gameplayCamera;

        private bool _canReceiveInput = true;

        public void DisableMovementInput() => _canReceiveInput = false;
        public void EnableMovementInput() => _canReceiveInput = true;

        private void Awake() 
        {
            _inputManager.MovementEvent += OnMovement;
            _inputManager.MousePositionEvent += OnMouseMovement;

            _provider?.Subscribe(this, _priority);

            _gameplayCamera = Camera.main;
        }

        private void OnDisable() 
        {
            _inputManager.MovementEvent -= OnMovement;
            _inputManager.MousePositionEvent -= OnMouseMovement;
            _provider?.UnSubscribe(this, _priority);
        }

        public void OnMovement(Vector2 movementInput)
        {
            if(movementInput == Vector2.zero)
            {
                _movementDirection = Vector3.zero;
                return;
            }

            //Get the two axes from the camera and flatten them on the XZ plane
            Vector3 cameraForward = _gameplayCamera.transform.forward;
            cameraForward.y = 0f;
            Vector3 cameraRight = _gameplayCamera.transform.right;
            cameraRight.y = 0f;

            //Use the two axes, modulated by the corresponding inputs, and construct the final vector
            Vector3 adjustedMovement = cameraRight.normalized * movementInput.x +
                cameraForward.normalized * movementInput.y;

            _movementDirection = Vector3.ClampMagnitude(adjustedMovement, 1f);
            _movementDirection.y = 0;
        }

        public void OnMouseMovement(Vector2 mousePosition)
        {
            Vector3 hitPoint = GetMousePosition(mousePosition);

            if (hitPoint == Vector3.zero)
            {
                _rotationDirection = Vector3.zero;
                return;
            }

            _rotationDirection = hitPoint - transform.position;
            _rotationDirection.y = 0;
        }

        private Vector3 GetMousePosition(Vector2 mousePosition)
        {
            if (Physics.Raycast(_gameplayCamera.ScreenPointToRay(mousePosition), out RaycastHit hit, 100, _terrainLayer))
            {
                return hit.point;
            }

            return Vector3.zero;
        }

        public override void Process(ref InputState currentState)
        {
            Vector3 currentRotationDirection = _movementDirection != Vector3.zero ? _movementDirection : _rotationDirection;

            currentState.MovementDirection = _canReceiveInput ? _movementDirection : Vector3.zero;
            currentState.RotationDirection = _canReceiveInput ? currentRotationDirection : Vector3.zero;
            currentState.Speed = -1;
        }
    }
}
