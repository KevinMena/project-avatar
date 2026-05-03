using AvatarBA.Managers;
using AvatarBA.Patterns;
using UnityEngine;

namespace AvatarBA.AI
{
    [System.Serializable]
    public class EngageState : BaseState
    {
        [SerializeField]
        private string m_NextStateId;
        [SerializeField]
        private string m_StopStateId;
        [SerializeField]
        private float m_StopDistance = 1;

        private bool m_DistanceReached = false;

        private InputState m_movementState;

        public override void Setup(Core owner, Sensors ownerSensors)
        {
            base.Setup(owner, ownerSensors);
            m_OwnerSensors = ownerSensors;
        }

        public override void OnUpdate()
        {
            Vector3 offset = m_Owner.transform.position.TargetDirection(m_OwnerSensors.TargetPosition);
            float cSquared = offset.Distance();

            if (cSquared <= m_StopDistance)
            {
                m_DistanceReached = true;
                return;
            }

            m_movementState.MovementDirection = offset.normalized;
            m_movementState.AimDirection = m_movementState.MovementDirection;
            m_movementState.Speed = -1;

            m_Owner.Movement.UpdateState(m_movementState);
        }

        public override void OnExit()
        {
            m_movementState.MovementDirection = Vector3.zero;
            m_movementState.AimDirection = Vector3.zero;
            m_movementState.Speed = 0;
            m_Owner.Movement.UpdateState(m_movementState);
            m_DistanceReached = false;
        }

        public override string ShouldTransition()
        {
            if (!m_OwnerSensors.TargetInRange)
            {
                return m_StopStateId;
            }

            if (m_DistanceReached)
            {
                return m_NextStateId;
            }

            return base.ShouldTransition();
        }
    }
}
