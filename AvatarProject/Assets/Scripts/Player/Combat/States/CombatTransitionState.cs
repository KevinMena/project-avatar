using UnityEngine;
using AvatarBA.Patterns;

namespace AvatarBA.Combat
{
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
            
            if(_owner.AttackTriggered)
                TransitionToTarget();

            if(_attackTriggeredTimer <= 0)
                owner.SetStateToInitial();
        }

        protected override void TransitionToTarget()
        {
            _owner.SetMainState(targetState);
        }
    }
}