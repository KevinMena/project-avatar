namespace AvatarBA.Combat
{
    public class CombatFinisherState : CombatBaseState
    {
        public CombatFinisherState(CombatHandler owner) : base(owner)
        {
            stateName = "Finisher Combo";
            animationName = "AttackMagic3";
        }

        public override void OnEnter()
        {
            base.OnEnter();

            attackDuration += 0.5f;
            UnityEngine.Debug.Log($"State: {stateName}");
        }
    }
}