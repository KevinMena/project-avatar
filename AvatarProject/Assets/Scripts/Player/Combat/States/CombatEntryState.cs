namespace AvatarBA.Combat
{
    public class CombatEntryState : CombatBaseState
    {
        public CombatEntryState(CombatHandler owner) : base(owner)
        {
            stateName = "Entry Combo";
            animationName = "AttackMagic1";
            nextState = owner.FollowUpState;
        }

        public override void OnEnter()
        {
            base.OnEnter();

            UnityEngine.Debug.Log($"State: {stateName}");
        }
    }
}