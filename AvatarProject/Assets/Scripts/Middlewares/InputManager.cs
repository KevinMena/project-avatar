using UnityEngine;
using UnityEngine.InputSystem;
using System;

namespace AvatarBA
{    
    [CreateAssetMenu(fileName = "Input Manager", menuName ="Middleware/Input Middleware")]
    public class InputManager : Middleware
    {
        // Logic variables
        [SerializeField] 
        private MovementInputProvider provider = default;

        // Movement variables
        private Vector2 _movementInput;

        private Vector2 _mousePosition;

        private bool _canDash;
        
        // Input variables
        private PlayerInputActions _playerInput = null;

        // FOR NOW
        public event Action OnCoreAbility1;

        public event Action OnCoreAbility2;

        public event Action OnCoreAbility3;

        public event Action OnCoreAbility4;


        private void OnEnable() 
        {
            if (_playerInput == null)
            {
                _playerInput = new PlayerInputActions();
                SetCallbacks();
            }

            EnableInput();
            provider?.Subscribe(this);
        }

        private void OnDisable()
        {
            RemoveCallbacks();
            DisableInput();
        }

        private void OnMousePosition(InputAction.CallbackContext context)
        {
            _mousePosition = context.ReadValue<Vector2>();
        }

        private void OnMove(InputAction.CallbackContext context)
        {
            _movementInput = context.ReadValue<Vector2>();
        }

        private void OnMoveFinished(InputAction.CallbackContext context)
        {
            _movementInput = Vector2.zero;
        }

        private void OnDash(InputAction.CallbackContext context)
        {
            provider.Dash();
        }

        public override void Process(ref InputState currentState)
        {
            currentState.movementDirection = _movementInput;
            currentState.canDash = true;
            currentState.mousePosition = _mousePosition;
        }

        private void SetCallbacks()
        {
            _playerInput.Gameplay.Mouse.performed += OnMousePosition;
            _playerInput.Gameplay.Movement.performed += OnMove;
            _playerInput.Gameplay.Movement.canceled += OnMoveFinished;
            _playerInput.Gameplay.Dash.performed += OnDash;
        }

        private void RemoveCallbacks()
        {
            _playerInput.Gameplay.Mouse.performed -= OnMousePosition;
            _playerInput.Gameplay.Movement.performed -= OnMove;
            _playerInput.Gameplay.Movement.canceled -= OnMoveFinished;
            _playerInput.Gameplay.Dash.performed -= OnDash;
            
        }

        public void EnableInput()
        {
            _playerInput.Gameplay.Enable();
        }

        private void DisableInput()
        {
            _playerInput.Gameplay.Disable();
        }

    }
}
