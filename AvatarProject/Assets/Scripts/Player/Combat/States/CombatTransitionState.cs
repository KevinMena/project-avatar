using UnityEngine;
using AvatarBA.Patterns;

namespace AvatarBA.Combat
{
    public class CombatTransitionState : TransitionState
    {
        private CombatManager _owner; 
        private float _baseTriggeredTimer = 0;
        private float _attackTriggeredTimer = 0;
        protected bool attackTriggered = false;

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
            
            if(attackTriggered)
                TransitionToTarget();

            if(_attackTriggeredTimer <= 0)
                owner.SetStateToInitial();
        }

        public override void OnExit()
        {
            attackTriggered = false;
        }

        public void ContinueCombo()
        {
            attackTriggered = true;
        }

        protected override void TransitionToTarget()
        {
            _owner.SetMainState(targetState);
        }
    }
}