using UnityEngine;

namespace AvatarBA.Patterns
{
    public class StateMachine : MonoBehaviour
    {
        protected State currentState;
        protected State initialState;

        public State CurrentState => currentState;

        protected virtual void Start()
        {
            if(initialState != null)
                SetState(initialState);
        }

        protected virtual void Update()
        {
            currentState?.OnUpdate();
        }

        protected void SetInitialState()
        {
            currentState = initialState;
            currentState.OnEnter();
        }

        public virtual void SetState(State nextState)
        {
            currentState?.OnExit();
            currentState = nextState;
            currentState.OnEnter();
        }

        public virtual void SetStateToInitial()
        {
            SetState(initialState);
        }
    }
}