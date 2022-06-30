namespace AvatarBA.Patterns
{
    public class TransitionState : State
    {
        protected State targetState;

        public TransitionState(StateMachine owner, State state) : base(owner)
        {
            targetState = state;
        }

        public override void OnEnter() { }

        public override void OnExit() { }

        public override void OnUpdate() { }

        protected virtual void TransitionToTarget()
        {
            owner.SetState(targetState);
        }
    }
}