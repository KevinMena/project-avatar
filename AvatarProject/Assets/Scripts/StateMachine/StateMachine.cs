namespace AvatarBA.Patterns
{
    public class StateMachine
    {
        protected State currentState;
        protected State initialState;

        public State CurrentState => currentState;

        public virtual void Start()
        {
            if(initialState != null)
                SetState(initialState);
        }

        public virtual void Update()
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