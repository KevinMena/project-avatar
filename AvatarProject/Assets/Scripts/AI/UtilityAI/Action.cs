using UnityEngine;

using AvatarBA.Patterns;

namespace AvatarBA.AI
{
    public class Action : MonoBehaviour, IState
    {
        [SerializeField]
        protected string _name;

        protected bool _completed;

        protected Consideration[] _considerations;
        
        public string Name => _name;
        public Consideration[] Considerations => _considerations;
        public bool Completed => _completed;

        protected virtual void Start() { }

        public virtual void OnEnter() { }

        public virtual void OnExit() { }

        public virtual void OnFixedUpdate() { }

        public virtual void OnUpdate() { }
    }
}

