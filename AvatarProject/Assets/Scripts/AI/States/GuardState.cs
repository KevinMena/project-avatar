using AvatarBA.Common;
using AvatarBA.Patterns;
using UnityEngine;

namespace AvatarBA.AI
{
    [System.Serializable]
    public class GuardState : BaseState
    {
        [SerializeField]
        private string m_NextStateId;
        [SerializeField]
        private string m_BackStateId;
        [SerializeField]
        private float m_GuardTime;

        private Timer m_Timer;

        private bool m_GuardOver = false;

        public override void Setup(Core owner, Sensors ownerSensors)
        {
            base.Setup(owner, ownerSensors);
            m_Timer = new Timer(m_GuardTime);
        }

        public override void OnEnter()
        {
            m_GuardOver = false;
            m_Timer.Start();
        }

        public override void OnUpdate()
        {
            if (m_Timer.IsComplete)
            {
                m_GuardOver = true;
                return;
            }

            m_Timer.Update(Time.deltaTime);
        }

        public override string ShouldTransition()
        {
            if (m_OwnerSensors.TargetInRange)
            {
                return m_NextStateId;
            }

            if (m_GuardOver)
            {
                return m_BackStateId;
            }

            return base.ShouldTransition();
        }
    }
}
