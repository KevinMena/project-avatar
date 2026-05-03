using AvatarBA.Managers;
using AvatarBA.Patterns;
using UnityEngine;

namespace AvatarBA.AI
{
    [System.Serializable]
    public class PatrolState : BaseState
    {
        [SerializeField]
        private string m_NextStateId;
        [SerializeField]
        private string m_StopStateId;
        [SerializeField]
        private float m_MaxDistance = 0;

        [SerializeField]
        private float m_WanderDistance = 0;

        private bool m_DestinationReached = false;

        private Vector3 m_Destination;
        private InputState m_MovementState;

        public override void Setup(Core owner, Sensors ownerSensors)
        {
            base.Setup(owner, ownerSensors);
            m_MovementState = new InputState();
        }

        public override void OnEnter()
        {
            m_DestinationReached = false;
            GenerateDestination();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            Vector3 offset = m_Owner.transform.position.TargetDirection(m_Destination);
            float cSquared = offset.Distance();

            if (cSquared <= 0.1f)
            {
                m_DestinationReached = true;
                return;
            }

            m_MovementState.MovementDirection = offset.normalized;
            m_MovementState.AimDirection = m_MovementState.MovementDirection;
            m_MovementState.Speed = -1;

            m_Owner.Movement.UpdateState(m_MovementState);
        }

        public override void OnExit()
        {
            m_MovementState.MovementDirection = Vector3.zero;
            m_MovementState.AimDirection = Vector3.zero;
            m_MovementState.Speed = 0;
            m_Owner.Movement.UpdateState(m_MovementState);
        }

        public override string ShouldTransition()
        {
            if (m_OwnerSensors.TargetInRange)
            {
                return m_NextStateId;
            }

            if (m_DestinationReached)
            {
                return m_StopStateId;
            }

            return base.ShouldTransition();
        }

        private void GenerateDestination()
        {
            do
            {
                m_Destination = RandomPoint(m_Owner.transform.position);
            }
            while (Vector3.Distance(m_Owner.SpawnPosition, m_Destination) > m_MaxDistance);
        }

        private Vector3 RandomPoint(Vector3 agentPosition)
        {
            Vector2 targetPoint = Random.insideUnitCircle * m_WanderDistance;
            return new Vector3(targetPoint.x + agentPosition.x,
                                        agentPosition.y,
                                        targetPoint.y + agentPosition.z);
        }
    }
}
