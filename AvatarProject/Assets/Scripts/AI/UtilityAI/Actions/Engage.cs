using UnityEngine;

using AvatarBA.AI.Considerations;
using AvatarBA.Managers;

namespace AvatarBA.AI.Actions
{
    public class Engage : Action
    {
        [SerializeField]
        private float m_stopDistance = 1;

        private InputState m_movementState;
        private Sensors m_ownerSensors;
        private Core m_ownerCore;

        private const float STOP_DISTANCE = 1.5f;

        protected override void Start()
        {
            _considerations = new Consideration[1] { new TargetInRange() };
            m_ownerSensors = GetComponentInParent<Sensors>();
            m_ownerCore = GetComponentInParent<Core>();
        }

        public override void OnUpdate()
        {
            if(!m_ownerSensors.TargetInRange)
            {
                _completed = true;
                return;
            }

            Vector3 offset = m_ownerCore.transform.position.TargetDirection(m_ownerSensors.TargetPosition);
            float cSquared = offset.Distance();

            if (cSquared <= m_stopDistance)
            {
                _completed = true;
                return;
            }

            m_movementState.MovementDirection = offset.normalized;
            m_movementState.RotationDirection = m_movementState.MovementDirection;
            m_movementState.Speed = -1;

            m_ownerCore.Movement.UpdateState(m_movementState);
        }

        public override void OnExit()
        {
            m_movementState.MovementDirection = Vector3.zero;
            m_movementState.RotationDirection = Vector3.zero;
            m_movementState.Speed = 0;
            m_ownerCore.Movement.UpdateState(m_movementState);
            _completed = false;
        }
    }
}
