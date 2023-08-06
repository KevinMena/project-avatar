using UnityEngine;

namespace AvatarBA.Patterns
{
    public class StateMachine : MonoBehaviour
    {
        [SerializeField]
        protected IState currentState;
        protected IState initialState;

        public IState CurrentState => currentState;

        protected virtual void Start()
        {
            if(initialState != null)
                SetState(initialState);
        }

        protected virtual void Update()
        {
            currentState?.OnUpdate();
        }

        protected virtual void FixedUpdate()
        {
            currentState?.OnFixedUpdate();
        }

        protected void SetInitialState()
        {
            currentState = initialState;
            currentState.OnEnter();
        }

        public virtual void SetState(IState nextState)
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