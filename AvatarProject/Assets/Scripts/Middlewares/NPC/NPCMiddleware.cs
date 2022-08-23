using UnityEngine;

namespace AvatarBA.NPC
{
    public class NPCMiddleware : Middleware
    {
        [SerializeField]
        private MovementInputProvider _provider = default;

        private Vector3 _movementPosition;
        private Vector3 _lastPosition;

        private void Awake()
        {
            _provider?.Subscribe(this);
        }

        private void OnDisable()
        {
            _provider?.UnSubscribe(this);
        }

        public void SetDestination(Vector3 destination)
        {
            if(destination == _movementPosition)
                return;

            _movementPosition = destination;
        }

        public override void Process(ref InputState currentState)
        {
            currentState.movementDirection = _movementPosition;
            currentState.targetPosition = _movementPosition;
        }
    }
}