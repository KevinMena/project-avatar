using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using System;

namespace AvatarBA
{    
    [CreateAssetMenu(fileName = "InputManager", menuName ="Managers/Input Manager")]
    public class InputManager : ScriptableObject
    {
        public event UnityAction<Vector2> MovementEvent;
        public event UnityAction<Vector2> MousePositionEvent;
        public event UnityAction DashEvent;
        public event UnityAction MeleeAttackEvent;

        private PlayerInputActions _playerInput = null;

        private void OnEnable() 
        {
            if (_playerInput == null)
            {
                _playerInput = new PlayerInputActions();
                SetCallbacks();
            }

            //EnableInput();
        }

        private void OnDisable()
        {
            RemoveCallbacks();
            DisableInput();
        }

        private void OnMousePosition(InputAction.CallbackContext context)
        {
            MousePositionEvent.Invoke(context.ReadValue<Vector2>());
        }

        private void OnMove(InputAction.CallbackContext context)
        {
            MovementEvent.Invoke(context.ReadValue<Vector2>());
        }

        private void OnDash(InputAction.CallbackContext context)
        {
            DashEvent.Invoke();
        }

        private void OnMeleeAtack(InputAction.CallbackContext context)
        {
            MeleeAttackEvent.Invoke();
        }

        private void SetCallbacks()
        {
            _playerInput.Gameplay.Mouse.performed += OnMousePosition;
            _playerInput.Gameplay.Movement.performed += OnMove;
            _playerInput.Gameplay.Movement.canceled += OnMove;
            _playerInput.Gameplay.Dash.performed += OnDash;
            _playerInput.Gameplay.MeleeAttack.performed += OnMeleeAtack;
        }

        private void RemoveCallbacks()
        {
            _playerInput.Gameplay.Mouse.performed -= OnMousePosition;
            _playerInput.Gameplay.Movement.performed -= OnMove;
            _playerInput.Gameplay.Movement.canceled -= OnMove;
            _playerInput.Gameplay.Dash.performed -= OnDash;
            _playerInput.Gameplay.MeleeAttack.performed -= OnMeleeAtack;
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
