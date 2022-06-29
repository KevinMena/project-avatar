namespace AvatarBA.Combat
{
    public class CombatEntryState : CombatBaseState
    {
        public CombatEntryState(CombatHandler owner) : base(owner)
        {
            stateName = "Entry Combo";
        }

        public override void OnEnter()
        {
            base.OnEnter();

            // Get duration
            attackDuration = 0.5f;
            // Set animation for this attack
            UnityEngine.Debug.Log($"State: {stateName}");
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            if(timer > attackDuration)
            {
                if(continueCombo)
                    owner.SetState(owner.FollowUpState);
                else
                    owner.SetStateToInitial();
            }
        }
    }
}