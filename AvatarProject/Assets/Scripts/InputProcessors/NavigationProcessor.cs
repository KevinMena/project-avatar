using System.Collections;
using UnityEngine;

namespace AvatarBA.Managers
{
    public class NavigationProcessor : Processor
    {
        private Vector3 _movementDirection;
        private Vector3 _rotationDirection;
        private float _movementSpeed;

        private void Awake()
        {
            _provider?.Subscribe(this, _priority);
        }

        private void OnDisable()
        {
            _provider?.UnSubscribe(this, _priority);
        }

        public void MoveToPoint(Vector3 targetPosition, float movementSpeed)
        {
            StartCoroutine(MoveToPointCoroutine(targetPosition, movementSpeed));
        }

        private IEnumerator MoveToPointCoroutine(Vector3 targetPosition, float movementSpeed)
        {
            Vector3 offset = transform.position.TargetDirection(targetPosition);
            offset.y = 0;
            float cSquared = offset.Distance();

            while (cSquared > 0.1f)
            {
                _movementDirection = offset.normalized;
                _rotationDirection = _movementDirection;
                _movementSpeed = movementSpeed;

                yield return null;
                offset = transform.position.TargetDirection(targetPosition);
                offset.y = 0;
                cSquared = offset.Distance();
            }

            Reset();
        }

        private void Reset()
        {
            _movementDirection = Vector3.zero;
            _rotationDirection = Vector3.zero;
            _movementSpeed = 0;
        }

        public override void Process(ref InputState currentState)
        {
            if (_movementDirection == Vector3.zero)
                return;

            currentState.MovementDirection = _movementDirection;
            currentState.RotationDirection = _rotationDirection;
            currentState.Speed = _movementSpeed;
        }
    }
}