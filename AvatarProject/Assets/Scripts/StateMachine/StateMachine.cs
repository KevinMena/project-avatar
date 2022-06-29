using UnityEngine;

namespace AvatarBA.Patterns
{
    public class StateMachine : MonoBehaviour
    {
        protected State currentState;
        protected State initialState;

        public State InitialState
        {
            get 
            {
                return initialState;
            }
        }

        protected virtual void Start()
        {
            if(initialState != null)
                SetState(initialState);
        }

        protected void Update()
        {
            currentState?.OnUpdate();
            Transition triggeredTransition = currentState?.CheckTransitions();

            if(triggeredTransition != null)
                SetState(triggeredTransition.TargetState);
        }

        public void SetState(State nextState)
        {
            currentState?.OnExit();
            currentState = nextState;
            currentState.OnEnter();
        }

        public void SetStateToInitial()
        {
            SetState(initialState);
        }
    }
}