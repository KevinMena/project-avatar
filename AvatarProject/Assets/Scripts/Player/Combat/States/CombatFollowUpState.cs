using AvatarBA.Patterns;

namespace AvatarBA.Combat
{
    public class CombatFollowUpState : CombatBaseState
    {
        public CombatFollowUpState(CombatHandler owner) : base(owner)
        {
            stateName = "FollowUp Combo";
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
                    owner.SetState(owner.FinisherState);
                else
                    owner.SetStateToInitial();
            }
        }
    }
}