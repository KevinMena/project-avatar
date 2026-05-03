using System;

namespace AvatarBA.Patterns
{
    public class StateMachine
    {
        private IState m_CurrentState;
        private IState m_InitialState;

        public Action<string> OnStateChanged;
        public IState CurrentState => m_CurrentState;

        public StateMachine(IState initialState, Action<string> stateChanged = null)
        {
            m_InitialState = initialState;
            OnStateChanged = stateChanged;
            SetState(m_InitialState);
        }

        public void Update()
        {
            m_CurrentState?.OnUpdate();
        }

        public void FixedUpdate()
        {
            m_CurrentState?.OnFixedUpdate();
        }

        public void SetInitialState()
        {
            SetState(m_InitialState);
        }

        public void SetState(IState nextState)
        {
            m_CurrentState?.OnExit();
            m_CurrentState = nextState;
            m_CurrentState.OnEnter();
            OnStateChanged?.Invoke(m_CurrentState.GetType().Name);
        }
    }
}
