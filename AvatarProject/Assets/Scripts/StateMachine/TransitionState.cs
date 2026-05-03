namespace AvatarBA.Patterns
{
    public class TransitionState : IState
    {
        protected StateMachineMonoBehaviour owner;
        protected IState targetState;

        public TransitionState(StateMachineMonoBehaviour owner, IState state)
        {
            this.owner = owner;
            targetState = state;
        }

        public virtual void OnEnter() { }

        public virtual void OnExit() { }

        public virtual void OnUpdate() { }

        public virtual void OnFixedUpdate() { }

        protected virtual void TransitionToTarget()
        {
            owner.SetState(targetState);
        }
    }
}