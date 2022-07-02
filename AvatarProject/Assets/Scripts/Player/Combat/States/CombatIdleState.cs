using AvatarBA.Patterns;

namespace AvatarBA.Combat
{
    public class CombatIdleState : CombatTransitionState
    {
        private CombatManager _owner;

        public CombatIdleState(CombatManager owner, CombatState state) : base(owner, state)
        {
            _owner = owner;
        }

        public override void OnEnter()
        {
            UnityEngine.Debug.Log("State: Idle");
        }

        public override void OnUpdate() 
        { 
            if(_owner.AttackTriggered)
                TransitionToTarget();
        }
    }
}
