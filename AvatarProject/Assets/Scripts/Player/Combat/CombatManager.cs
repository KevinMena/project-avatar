using System;
using UnityEngine;
using AvatarBA.Patterns;

namespace AvatarBA.Combat
{
    [Serializable]
    public class CombatManager : StateMachine
    {
        [SerializeField]
        private CombatStateData[] _combatStatesData;
        
        private CombatState[] _combatStates;
        private CombatTransitionState[] _transitions;

        private int _currentComboIndex = 0;

        // TODO: CHANGED THIS
        private const float timerForInput = 0.8f;

        public override void Start()
        {
            GenerateStates();
            initialState = _transitions[0];
            SetInitialState();
        }

        private void GenerateStates()
        {
            int numberStates = _combatStatesData.Length;

            _combatStates = new CombatState[numberStates];

            for(int i = 0; i < numberStates; i++)
            {
                _combatStates[i] = new CombatState(this, _combatStatesData[i]);
            }

            _transitions = new CombatTransitionState[numberStates];

            _transitions[0] = new CombatIdleState(this, _combatStates[0]);

            for(int i = 1; i < numberStates; i++)
            {
                _transitions[i] = new CombatTransitionState(this, _combatStates[i], timerForInput);
            }
        }

        public void SetMainState(State nextState)
        {
            _currentComboIndex++;
            SetState(nextState);
        }

        public override void SetStateToInitial()
        {
            _currentComboIndex = 0;
            base.SetStateToInitial();
        }

        public void SetNextState()
        {
            if(_currentComboIndex >= _combatStates.Length)
            {
                SetStateToInitial();
                return;
            }
            
            SetState(_transitions[_currentComboIndex]);
        }

        public void TriggerCombo()
        {
            Type currentType = currentState.GetType();
            if(currentType == typeof(CombatTransitionState) || currentType == typeof(CombatIdleState))
            {
                (currentState as CombatTransitionState).ContinueCombo();
            }
        }
    }
}