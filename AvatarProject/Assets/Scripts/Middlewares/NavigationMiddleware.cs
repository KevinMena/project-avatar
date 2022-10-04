using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AvatarBA.Managers
{
    public class NavigationMiddleware : Middleware
    {
        private Vector3 _movementDirection;
        private Vector3 _rotationDirection;
        private float _movementSpeed;

        private void Awake()
        {
            _provider?.Subscribe(this);
        }

        private void OnDestroy()
        {
            _provider?.UnSubscribe(this);
        }

        public void SetMovement(Vector3 movementDirection, Vector3 rotationDirection, float speed)
        {
            _movementDirection = movementDirection;
            _rotationDirection = rotationDirection;
            _movementSpeed = speed;
        }

        public override void Process(ref InputState currentState)
        {
            
        }
    }
}