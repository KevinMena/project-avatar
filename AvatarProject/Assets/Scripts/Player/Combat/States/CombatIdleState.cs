namespace AvatarBA.Combat
{
    public class CombatIdleState : CombatState
    {
        public CombatIdleState(CombatHandler owner) : base(owner)
        {
        }

        public override void OnEnter()
        {
            UnityEngine.Debug.Log("State: Idle");
        }

        public override void OnExit() { }

        public override void OnUpdate() { }
    }
}
