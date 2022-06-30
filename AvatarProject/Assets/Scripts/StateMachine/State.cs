namespace AvatarBA.Patterns
{
    public abstract class State
    {
        protected StateMachine owner;

        public State(StateMachine owner)
        {
            this.owner = owner;
        }

        public abstract void OnEnter();

        public abstract void OnUpdate();

        public abstract void OnExit();
    }
}