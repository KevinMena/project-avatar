using UnityEngine;
using AvatarBA.Patterns;

namespace AvatarBA.Combat
{
    /// <summary>
    /// Middle states that live in the middle of two combo states. This state is merely
    /// for receiving input in a span of time
    /// </summary>
    public class CombatTransitionState : TransitionState
    {
        private CombatManager _owner; 
        private float _baseTriggeredTimer = 0;
        private float _attackTriggeredTimer = 0;

        public CombatTransitionState(CombatManager owner, CombatState state, float timer = 0) : base(owner, state)
        {
            _owner = owner as CombatManager;
            _baseTriggeredTimer = timer;
            _attackTriggeredTimer = timer;
        }

        public override void OnEnter()
        {
            _attackTriggeredTimer = _baseTriggeredTimer;
            UnityEngine.Debug.Log($"State: Transition");
        }

        public override void OnUpdate()
        {
            _attackTriggeredTimer -= Time.deltaTime;
            
            // If the input buffer time of the attack is alive and we 
            // receive an input transition to the next combo state
            if(_owner.AttackTriggered)
                TransitionToTarget();

            // If the time for the input is done then transition to idle
            if(_attackTriggeredTimer <= 0)
                owner.SetStateToInitial();
        }

        protected override void TransitionToTarget()
        {
            _owner.SetMainState(targetState);
        }
    }
}