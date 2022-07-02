using AvatarBA.Patterns;

namespace AvatarBA.Combat
{
    /// <summary>
    /// Base transition state in which the player doesn't do anything just wait for the input 
    /// to trigger the combos.
    /// </summary>
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
