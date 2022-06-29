namespace AvatarBA.Combat
{
    public class CombatFollowUpState : CombatBaseState
    {
        public CombatFollowUpState(CombatHandler owner) : base(owner)
        {
            stateName = "FollowUp Combo";
            animationName = "AttackMagic2";
            nextState = owner.FinisherState;
        }

        public override void OnEnter()
        {
            base.OnEnter();

            UnityEngine.Debug.Log($"State: {stateName}");
        }
    }
}