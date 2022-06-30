using UnityEngine;

using AvatarBA.Combat;

namespace AvatarBA
{
    public class PlayerCombatController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private InputManager _inputManager;

        [SerializeField]
        private CombatManager _combatManager;

        private void Awake() 
        {
            _inputManager.MeleeAttackEvent += OnAttack;
            // _finisherState = new CombatFinisherState(this);
            // _followUpState = new CombatFollowUpState(this);
            // _entryState = new CombatEntryState(this);
            // _idleState = new CombatIdleState(this);
            // initialState = _idleState;
        }

        private void OnDisable() 
        {
            _inputManager.MeleeAttackEvent -= OnAttack;
        }

        private void Start() 
        {
            _combatManager.Start();
        }

        private void Update() 
        {
            _combatManager.Update();
        }

        private void OnAttack()
        {
            // if(currentState.GetType() == typeof(CombatIdleState))
            // {
            //     SetState(EntryState);
            //     return;
            // }

            // CombatBaseState comboState = currentState as CombatBaseState;
            // comboState.TriggerInput();

            _combatManager.TriggerCombo();
        }
    }
}
