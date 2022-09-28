using AvatarBA.Debugging;
using UnityEngine;

namespace AvatarBA.Combat
{
    /// <summary>
    /// Base transition state in which the player doesn't do anything just wait for the input 
    /// to trigger the combos.
    /// </summary>
    public class CombatIdleState : CombatTransitionState
    {
        private CombatManager _owner;

        private float _startComboTimer = 0;

        private readonly int IdleAnimation = UnityEngine.Animator.StringToHash("Idle");

        public CombatIdleState(CombatManager owner, CombatState state) : base(owner, state)
        {
            _owner = owner;
        }

        public override void OnEnter()
        {
            _owner.ChangeMovement(true);
            _owner.SetAnimation(IdleAnimation);
            _startComboTimer = _owner.LastComboDelay;
            GameDebug.Log("State: Idle");
        }

        public override void OnExit()
        {
            _owner.ChangeMovement(false);
        }

        public override void OnUpdate() 
        { 
            if(_owner.FromLastCombo)
            {
                _startComboTimer -= Time.deltaTime;

                if (_startComboTimer > 0)
                    return;
            }

            if(_owner.AttackTriggered)
                TransitionToTarget();
        }
    }
}
