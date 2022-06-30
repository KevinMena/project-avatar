using AvatarBA.Patterns;

namespace AvatarBA.Combat
{
    public class CombatIdleState : CombatTransitionState
    {
        public CombatIdleState(CombatManager owner, CombatState state) : base(owner, state)
        {
        }

        public override void OnEnter()
        {
            UnityEngine.Debug.Log("State: Idle");
        }

        public override void OnUpdate() 
        { 
            if(attackTriggered)
                TransitionToTarget();
        }
    }
}
