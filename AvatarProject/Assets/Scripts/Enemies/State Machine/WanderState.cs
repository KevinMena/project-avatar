using UnityEngine;

using AvatarBA.Managers;

namespace AvatarBA.AI.States
{
    public class WanderState : BaseState
    {
        private float _maxDistance = 0;
        private float _wanderDistance = 0;

        private Vector3 _destination;
        private InputState _movementState;

        private Core _ownerCore;

        public WanderState(float maxDistance, float wanderDistance)
        {
            _maxDistance = maxDistance;
            _wanderDistance = wanderDistance;
        }

        public override void Setup(GameObject owner)
        {
            _ownerCore = owner.GetComponentInParent<Core>();
        }

        public override void OnEnter()
        {
            _movementState = new InputState();
            _completed = false;

            do
            {
                _destination = RandomPoint(_ownerCore.transform.position);
            }
            while (Vector3.Distance(_ownerCore.SpawnPosition, _destination) > _maxDistance);
        }

        public override void OnUpdate()
        {
            Vector3 offset = _ownerCore.transform.position.TargetDirection(_destination);
            float cSquared = offset.Distance();

            if (cSquared <= 0.1f)
            {
                _completed = true;
                return;
            }

            _movementState.MovementDirection = offset.normalized;
            _movementState.RotationDirection = _movementState.MovementDirection;
            _movementState.Speed = -1;

            _ownerCore.Movement.UpdateState(_movementState);
        }

        public override void OnExit()
        {
            _movementState.MovementDirection = Vector3.zero;
            _movementState.RotationDirection = Vector3.zero;
            _movementState.Speed = 0;
            _ownerCore.Movement.UpdateState(_movementState);
            _completed = false;
        }

        private Vector3 RandomPoint(Vector3 agentPosition)
        {
            Vector2 targetPoint = Random.insideUnitCircle * _wanderDistance;
            return new Vector3(targetPoint.x + agentPosition.x,
                                        agentPosition.y,
                                        targetPoint.y + agentPosition.z);
        }
    }
}
