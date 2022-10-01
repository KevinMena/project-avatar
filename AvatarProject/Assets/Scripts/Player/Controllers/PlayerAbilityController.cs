using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using AvatarBA.Common;
using AvatarBA.Abilities;
using AvatarBA.Managers;

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
        private Timer[] _cooldownTimers;
        private Timer[] _activeTimers;

        private void Awake() 
        {
            _inputManager.DashEvent += TriggerDash;
            _inputManager.LeftAbilityEvent += TriggerLeftSlot;
            _inputManager.RightAbilityEvent += TriggerRightSlot;
            _inputManager.UltimateAbilityEvent += TriggerUltimate;
        }

        private void OnDestroy()
        {
            _inputManager.DashEvent -= TriggerDash;
            _inputManager.LeftAbilityEvent -= TriggerLeftSlot;
            _inputManager.RightAbilityEvent -= TriggerRightSlot;
            _inputManager.UltimateAbilityEvent -= TriggerUltimate;
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

            _cooldownTimers = new Timer[4]
            {
                new Timer(0),
                new Timer(0),
                new Timer(0),
                new Timer(0)
            };

            _activeTimers = new Timer[4]
            {
                new Timer(0),
                new Timer(0),
                new Timer(0),
                new Timer(0)
            };

            SetupAbilities();
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
            // Send message of ability not unlocked
            if (currentAbility == null)
                return;

            // Send message of not meeting requirements
            if(!PassRequirements(currentAbility.Cost))
                return;
            
            AbilityState currentState = GetCurrentState(slot);

            if(currentState != AbilityState.Ready)
                return;
            
            StartCoroutine(TriggerRoutine(slot, currentAbility));
        }

        private IEnumerator TriggerRoutine(AbilitySlot slot, Ability currentAbility)
        {     
            // Trigger ability
            StartCoroutine(currentAbility.Trigger(gameObject));

            // Use Timer is the ability has active time
            if(currentAbility.ActiveTime != 0)
            {
                Timer activeTimer = _activeTimers[(int)slot];
                activeTimer.TotalTime = currentAbility.ActiveTime;
                UpdateState(slot, AbilityState.Active);

                activeTimer.Start();
                StartActiveDisplay(slot, currentAbility.ActiveTime);
                while (!activeTimer.IsComplete)
                {
                    activeTimer.Update(Time.deltaTime);
                    UpdateDisplay(slot, activeTimer.PercentElapsed, activeTimer.RemainingTime);
                    yield return null;
                }
            }

            // Use Timer for the cooldown of the ability
            Timer cooldownTimer = _cooldownTimers[(int)slot];
            cooldownTimer.TotalTime = currentAbility.Cooldown;
            UpdateState(slot, AbilityState.Cooldown);

            cooldownTimer.Start();
            StartCooldownDisplay(slot, currentAbility.Cooldown);
            while(!cooldownTimer.IsComplete)
            {
                cooldownTimer.Update(Time.deltaTime);
                UpdateDisplay(slot, cooldownTimer.PercentElapsed, cooldownTimer.RemainingTime);
                yield return null;
            }
            EndDisplay(slot);
            UpdateState(slot, AbilityState.Ready);
        }

        private bool PassRequirements(float cost)
        {
            return true;
        }

        private void SetupAbilities()
        {
            if (_dash != null)
                UpdateIcon(AbilitySlot.Dash, _dash);

            if (_leftAbility != null)
                UpdateIcon(AbilitySlot.Left, _leftAbility);

            if (_rightAbility != null)
                UpdateIcon(AbilitySlot.Right, _rightAbility);

            if (_ultimate != null)
                UpdateIcon(AbilitySlot.Ultimate, _ultimate);
        }

        public void ReplaceAbility(AbilitySlot slot, Ability newAbility)
        {
            switch (slot)
            {
                case AbilitySlot.Dash:
                    _dash = newAbility;
                    break;
                case AbilitySlot.Left:
                    _leftAbility = newAbility;
                    break;
                case AbilitySlot.Right:
                    _rightAbility = newAbility;
                    break;
                case AbilitySlot.Ultimate:
                    _ultimate = newAbility;
                    break;
                default:
                    break;
            }

            UpdateIcon(slot, newAbility);
        }

        private void UpdateState(AbilitySlot slot, AbilityState state)
        {
            _abilityStates[(int) slot] = state;
        }

        private AbilityState GetCurrentState(AbilitySlot slot)
        {
            return _abilityStates[(int) slot];
        }

        private void StartCooldownDisplay(AbilitySlot slot, float maxTimer)
        {
            _displayManager.StartCooldownTimer((int) slot, maxTimer);
        }

        private void StartActiveDisplay(AbilitySlot slot, float maxTimer)
        {
            _displayManager.StartActiveTimer((int)slot, maxTimer);
        }

        private void EndDisplay(AbilitySlot slot)
        {
            _displayManager.EndTimer((int)slot);
        }

        private void UpdateDisplay(AbilitySlot slot, float current, float currentTimer)
        {
            _displayManager.UpdateDisplay((int)slot, current, currentTimer);
        }

        private void UpdateIcon(AbilitySlot slot, Ability ability)
        {
            _displayManager.UpdateIcon((int)slot, ability.Icon);
        }
    }
}