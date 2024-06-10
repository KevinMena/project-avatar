using UnityEngine;

namespace AvatarBA.Managers
{
    public class InputProcessor : Processor
    {
        [Header("Input References")]
        [SerializeField]
        private InputManager m_inputManager = default;

        [Header("Values")]
        [SerializeField] 
        private LayerMask m_terrainLayer;

        // Movement variables
        private Vector3 m_movementDirection;
        private Vector3 m_aimDirection;

        private Camera m_gameplayCamera;

        private bool m_canReceiveInput = true;

        public void DisableMovementInput() => m_canReceiveInput = false;
        public void EnableMovementInput() => m_canReceiveInput = true;

        private void Awake() 
        {
            m_inputManager.MovementEvent += OnMovement;
            m_inputManager.MousePositionEvent += OnMouseMovement;

            _provider?.Subscribe(this, _priority);

            m_gameplayCamera = Camera.main;
        }

        private void OnDisable() 
        {
            m_inputManager.MovementEvent -= OnMovement;
            m_inputManager.MousePositionEvent -= OnMouseMovement;
            _provider?.UnSubscribe(this, _priority);
        }

        public void OnMovement(Vector2 movementInput)
        {
            if(movementInput == Vector2.zero)
            {
                m_movementDirection = Vector3.zero;
                return;
            }

            //Get the two axes from the camera and flatten them on the XZ plane
            Vector3 cameraForward = m_gameplayCamera.transform.forward;
            cameraForward.y = 0f;
            Vector3 cameraRight = m_gameplayCamera.transform.right;
            cameraRight.y = 0f;

            //Use the two axes, modulated by the corresponding inputs, and construct the final vector
            Vector3 adjustedMovement = cameraRight.normalized * movementInput.x +
                cameraForward.normalized * movementInput.y;

            m_movementDirection = Vector3.ClampMagnitude(adjustedMovement, 1f);
            m_movementDirection.y = 0;
        }

        public void OnMouseMovement(Vector2 mousePosition)
        {
            Vector3 hitPoint = GetMousePosition(mousePosition);

            if (hitPoint == Vector3.zero)
            {
                m_aimDirection = Vector3.zero;
                return;
            }

            m_aimDirection = hitPoint - transform.position;
            m_aimDirection.y = 0;
        }

        private Vector3 GetMousePosition(Vector2 mousePosition)
        {
            if (Physics.Raycast(m_gameplayCamera.ScreenPointToRay(mousePosition), out RaycastHit hit, 100, m_terrainLayer))
            {
                return hit.point;
            }

            return Vector3.zero;
        }

        public override void Process(ref InputState currentState)
        {
            currentState.MovementDirection = m_canReceiveInput ? m_movementDirection : Vector3.zero;
            currentState.AimDirection = m_canReceiveInput ? m_aimDirection : Vector3.zero;
            currentState.Speed = -1;
        }
    }
}
