using UnityEngine;

using AvatarBA.Combat;

namespace AvatarBA
{
    public class PlayerCombatController : CombatHandler
    {
        [Header("References")]
        [SerializeField]
        private InputManager _inputManager;

        private void Awake() 
        {
            _inputManager.MeleeAttackEvent += OnAttack;
            _finisherState = new CombatFinisherState(this);
            _followUpState = new CombatFollowUpState(this);
            _entryState = new CombatEntryState(this);
            _idleState = new CombatIdleState(this);
            initialState = _idleState;
        }

        private void OnDisable() 
        {
            _inputManager.MeleeAttackEvent -= OnAttack;
        }

        private void OnAttack()
        {
            if(currentState.GetType() == typeof(CombatIdleState))
            {
                SetState(EntryState);
                return;
            }

            CombatBaseState comboState = currentState as CombatBaseState;
            comboState.TriggerInput();
        }
    }
}
