namespace AvatarBA.Combat
{
    public abstract class CombatState
    {
        protected CombatHandler owner;

        public CombatState(CombatHandler owner)
        {
            this.owner= owner;
        }

        public abstract void OnEnter();

        public abstract void OnUpdate();

        public abstract void OnExit();
    }
}