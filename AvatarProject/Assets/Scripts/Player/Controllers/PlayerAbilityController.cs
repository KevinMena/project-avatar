using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using AvatarBA.UI;
using AvatarBA.Common;

namespace AvatarBA
{ 
    public class PlayerAbilityController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private InputManager _inputManager;

        [SerializeField]
        private AbilityUI _abilitiesDisplay;
        
        [Header("Abilities")]
        [SerializeField]
        private Ability _dash;

        [SerializeField]
        private Ability _leftAbility;

        [SerializeField]
        private Ability _rightAbility;

        [SerializeField]
        private Ability _ultimate;

        private const int _dashSlot = 0;
        private const int _leftSlot = 1;
        private const int _rightSlot = 2;
        private const int _ultimateSlot = 3;

        private Dictionary<int, AbilityState> _states;

        private void Awake() 
        {
            _inputManager.DashEvent += TriggerDash;
        }

        private void OnDisable()
        {
            _inputManager.DashEvent -= TriggerDash;
        }

        private void Start() 
        {
            _states = new Dictionary<int, AbilityState>()
            {
                {_dashSlot, AbilityState.ready},
                {_leftSlot, AbilityState.ready},
                {_rightSlot, AbilityState.ready},
                {_ultimateSlot, AbilityState.ready},
            };
        }

        public void TriggerDash()
        {
            AbilityState currentState = GetCurrentState(_dashSlot);

            if(currentState != AbilityState.ready)
                return;
            
            StartCoroutine(TriggerRoutine(_dashSlot, _dash));
        }

        private IEnumerator TriggerRoutine(int slot, Ability currentAbility)
        {
            CooldownTimer cooldownTimer = new CooldownTimer(currentAbility.Cooldown);

            StartCoroutine(currentAbility.Trigger(gameObject));
            UpdateState(slot, AbilityState.cooldown);

            while(!cooldownTimer.IsComplete)
            {
                cooldownTimer.Update(Time.deltaTime);
                UpdateDisplay(cooldownTimer.PercentElapsed, slot);
                yield return null;
            }

            UpdateState(slot, AbilityState.ready);
        }

        private void UpdateDisplay(float current, int slot)
        {
            _abilitiesDisplay.UpdateIcon(current, slot);
        }

        private void UpdateState(int slot, AbilityState state)
        {
            _states[slot] = state;
        }

        private AbilityState GetCurrentState(int slot)
        {
            return _states[slot];
        }
    }
}