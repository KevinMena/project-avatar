using UnityEngine;

using AvatarBA.Patterns;

namespace AvatarBA.AI.States
{
    public class BaseState : IState
    {
        [SerializeField]
        protected string _stateName;

        protected bool _completed;

        protected StateMachine _owner;

        public string StateName => _stateName;
        public StateMachine Owner => _owner;
        public bool Completed => _completed;

        public virtual void Setup(GameObject owner) { }

        public virtual void OnEnter() { }

        public virtual void OnExit() { }

        public virtual void OnFixedUpdate() { }

        public virtual void OnUpdate() { }
    }
}
