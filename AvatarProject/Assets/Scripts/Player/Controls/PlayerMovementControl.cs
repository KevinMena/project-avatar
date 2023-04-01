using UnityEngine;

using AvatarBA.Managers;

namespace AvatarBA
{
    public class PlayerMovementControl : MovementControl
    {
        [Header("References")]
        [SerializeField]
        private InputProvider _provider;

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
            InputState currentState = _provider.GetState();
            _movementDirection = currentState.MovementDirection;
            _rotationDirection = currentState.RotationDirection;
            _speed = currentState.Speed;
        }
    }
}
