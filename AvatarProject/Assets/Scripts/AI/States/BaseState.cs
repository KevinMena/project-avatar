using AvatarBA.AI;
using UnityEngine;

namespace AvatarBA.Patterns
{
    [System.Serializable]
    public class BaseState : IState
    {
        [SerializeField]
        protected string m_Id;

        public string Id => m_Id;

        protected Core m_Owner;
        protected Sensors m_OwnerSensors;

        public virtual void Setup(Core owner, Sensors ownerSensors)
        {
            m_Owner = owner;
            m_OwnerSensors = ownerSensors;
        }

        public virtual string ShouldTransition()
        {
            return "";
        }

        public virtual void OnEnter()
        {
        }

        public virtual void OnExit()
        {
        }

        public virtual void OnFixedUpdate()
        {
        }

        public virtual void OnUpdate()
        {
        }
    }
}
