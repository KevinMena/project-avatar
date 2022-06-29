using AvatarBA.Patterns;

namespace AvatarBA.Combat
{
    public class CombatFinisherState : CombatBaseState
    {
        public CombatFinisherState(CombatHandler owner) : base(owner)
        {
            stateName = "Finisher Combo";
        }

        public override void OnEnter()
        {
            base.OnEnter();

            // Get duration
            attackDuration = 1f;
            // Set animation for this attack
            UnityEngine.Debug.Log($"State: {stateName}");
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            if(timer > attackDuration)
                owner.SetStateToInitial();
        }
    }
}