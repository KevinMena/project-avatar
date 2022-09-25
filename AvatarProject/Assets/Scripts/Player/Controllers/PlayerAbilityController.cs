using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using AvatarBA.Common;
using AvatarBA.Abilities;

namespace AvatarBA
{ 
    public enum AbilitySlot
    {
        Dash,
        Left,
        Right,
        Ultimate
    }

    public class PlayerAbilityController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private InputManager _inputManager;

        [SerializeField]
        private AbilityDisplayManager _displayManager;
        
        [Header("Abilities")]
        [SerializeField]
        private Ability _dash;

        [SerializeField]
        private Ability _leftAbility;

        [SerializeField]
        private Ability _rightAbility;

        [SerializeField]
        private Ability _ultimate;

        private AbilityState[] _abilityStates;

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
            _abilityStates = new AbilityState[4] 
                            { 
                                AbilityState.Ready, 
                                AbilityState.Ready, 
                                AbilityState.Ready, 
                                AbilityState.Ready 
                            };
        }

        public void TriggerDash()
        {
            TriggerAbility(AbilitySlot.Dash, _dash);
        }

        public void TriggerLeftSlot()
        {
            TriggerAbility(AbilitySlot.Left, _leftAbility);
        }

        public void TriggerRightSlot()
        {
            TriggerAbility(AbilitySlot.Right, _rightAbility);
        }

        public void TriggerUltimate()
        {
            TriggerAbility(AbilitySlot.Ultimate, _ultimate);
        }

        public void TriggerAbility(AbilitySlot slot, Ability currentAbility)
        {
            if(!PassRequirements(currentAbility.Cost))
                return;
            
            AbilityState currentState = GetCurrentState(slot);

            if(currentState != AbilityState.Ready)
                return;
            
            StartCoroutine(TriggerRoutine(slot, currentAbility));
        }

        private IEnumerator TriggerRoutine(AbilitySlot slot, Ability currentAbility)
        {
            CooldownTimer cooldownTimer = new CooldownTimer(currentAbility.Cooldown);

            StartCoroutine(currentAbility.Trigger(gameObject));
            UpdateState(slot, AbilityState.Cooldown);

            while(!cooldownTimer.IsComplete)
            {
                cooldownTimer.Update(Time.deltaTime);
                UpdateDisplay(slot, cooldownTimer.PercentElapsed);
                yield return null;
            }

            UpdateState(slot, AbilityState.Ready);
        }

        private bool PassRequirements(float cost)
        {
            return true;
        }

        public void ReplaceAbility(int slot, Ability newAbility)
        {
            switch (slot)
            {
                case 0:
                    _dash = newAbility;
                    break;
                case 1:
                    _leftAbility = newAbility;
                    break;
                case 2:
                    _rightAbility = newAbility;
                    break;
                case 3:
                    _ultimate = newAbility;
                    break;
                default:
                    break;
            }
        }

        private void UpdateState(AbilitySlot slot, AbilityState state)
        {
            _abilityStates[(int) slot] = state;
        }

        private AbilityState GetCurrentState(AbilitySlot slot)
        {
            return _abilityStates[(int) slot];
        }

        private void UpdateDisplay(AbilitySlot slot, float current)
        {
            _displayManager.UpdateDisplay((int)slot, current);
        }

        private void UpdateIcon(AbilitySlot slot, Ability ability)
        {
            _displayManager.UpdateIcon((int)slot, ability.Icon);
        }
    }
}