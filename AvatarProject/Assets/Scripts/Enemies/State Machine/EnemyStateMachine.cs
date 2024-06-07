using AvatarBA.Patterns;
using UnityEngine.Events;

namespace AvatarBA.AI.States
{
    public class EnemyStateMachine : StateMachine
    {
        public UnityAction OnStateFinish;
        public bool StateComplete => currentState == null ? false : (currentState as Action).Completed;

        protected override void Start() { }

        protected override void Update() 
        {
            base.Update();

            if(StateComplete)
                FinishState();
        }

        public override void SetState(IState nextState)
        {
            currentState = nextState;
            currentState.OnEnter();
        }

        private void FinishState()
        {
            currentState?.OnExit();
            OnStateFinish?.Invoke();
            currentState = null;
        }
    }
}
