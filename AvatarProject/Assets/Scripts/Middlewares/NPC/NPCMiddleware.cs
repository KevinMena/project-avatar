using UnityEngine;

using AvatarBA.Managers;

namespace AvatarBA.NPC
{
    public class NPCMiddleware : Middleware
    {
        private Vector3 _movementPosition;

        private void Awake()
        {
            _provider?.Subscribe(this, _priority);
        }

        private void OnDestroy()
        {
            _provider?.UnSubscribe(this, _priority);
        }

        private void Start()
        {
            _movementPosition = transform.position;
        }

        public void SetDestination(Vector3 destination)
        {
            _movementPosition = destination;
        }

        public override void Process(ref InputState currentState)
        {
            currentState.MovementDirection = _movementPosition;
            currentState.RotationDirection = _movementPosition;
        }
    }
}