using System.Collections;
using UnityEngine;

using AvatarBA.AI.States;
using AvatarBA.Patterns;

namespace AvatarBA.AI
{
    public class Action : MonoBehaviour
    {
        [SerializeField]
        protected string _name;

        protected Consideration[] _considerations;
        protected BaseState _actionState;
        
        public string Name => _name;
        public Consideration[] Considerations => _considerations;

        protected virtual void Start()
        {
            _actionState.Setup(gameObject);
        }

        public void Execute(StateMachine owner) 
        { 
            owner.SetState(_actionState);
        }
    }
}

