using UnityEngine;

using AvatarBA.Managers;

namespace AvatarBA
{
    public class PlayerMovementControl : MovementControl
    {
        [Header("References")]
        [SerializeField]
        private InputProvider m_provider;

        protected override void Update()
        {
            UpdateState();
            base.Update();
        }

        /// <summary>
        /// Ask the Provider to gather the current information about the inputs and 
        /// updates the corresponding information.
        /// </summary>
        private void UpdateState()
        {
            InputState currentState = m_provider.GetState();
            m_movementDirection = currentState.MovementDirection;
            m_aimDirection = currentState.AimDirection;
            m_speed = currentState.Speed;
        }
    }
}
