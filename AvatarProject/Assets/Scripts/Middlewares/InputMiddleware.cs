using UnityEngine;

namespace AvatarBA
{
    public class InputMiddleware : Middleware
    {
        // Logic variables
        [SerializeField]
        private InputManager _inputManager = default;

        [SerializeField] 
        private MovementInputProvider _provider = default;

        // Movement variables
        private Vector2 _movementPosition;

        private Vector2 _mousePosition;

        private void Awake() 
        {
            _inputManager.MovementEvent += OnMovement;
            _inputManager.MousePositionEvent += OnMouseMovement;

            _provider?.Subscribe(this);
        }

        private void OnDisable() 
        {
            _inputManager.MovementEvent -= OnMovement;
            _inputManager.MousePositionEvent -= OnMouseMovement;
            _provider?.UnSubscribe(this);
        }

        public void OnMovement(Vector2 movementInput)
        {
            _movementPosition = movementInput;
        }

        public void OnMouseMovement(Vector2 mousePosition)
        {
            _mousePosition = mousePosition;
        }

        public override void Process(ref InputState currentState)
        {
            currentState.movementDirection = new Vector3(_movementPosition.x, 0, _movementPosition.y);
            currentState.targetPosition = _mousePosition;
        }
    }
}
