using System;
using UnityEngine;
using AvatarBA.Patterns;

namespace AvatarBA.Combat
{
    /// <summary>
    /// Manager of the combat system. The system is combo based combat of a number of states.
    /// We keep the current combo index to know when to finish the combo and to reset all the states.
    /// </summary>
    [Serializable]
    public class CombatManager : StateMachine
    {
        [SerializeField]
        private CombatStateData[] _combatStatesData;

        [SerializeField]
        private LayerMask _hittableLayer;

        [SerializeField]
        private Transform _hitPoint;
        
        private CombatState[] _combatStates;
        private CombatTransitionState[] _transitions;

        private int _currentComboIndex = 0;

        private bool _attackTriggered = false;
        private bool _isTransitioning = false;
        private bool _fromLastCombo = false;
        private const float timerForInput = 0.8f;
        private const float lastComboDelay = 0.5f;

        public LayerMask HittableLayer => _hittableLayer;
        public Transform HitPoint => _hitPoint;
        public bool Comboing => _currentComboIndex > 0;
        public bool AttackTriggered => _attackTriggered;
        public bool IsLastCombo => _currentComboIndex >= _combatStates.Length;
        public bool FromLastCombo => _fromLastCombo;
        public float LastComboDelay => lastComboDelay;

        protected override void Start()
        {
            GenerateStates();
            initialState = _transitions[0];
            _isTransitioning = true;
            SetInitialState();
        }

        /// <summary>
        /// Instantiate all the necessary combat states and transitions states
        /// depending on the length of the combo. Also instantiate the idle state
        /// </summary>
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

        /// <summary>
        /// Transition to one of the main combo states. In this states we don't register inputs
        /// </summary>
        /// <param name="nextState"> Combo state to transition </param>
        public void SetMainState(State nextState)
        {
            _currentComboIndex++;
            _isTransitioning = false;
            _attackTriggered = false;
            _fromLastCombo = false;
            SetState(nextState);
        }

        /// <summary>
        /// Reset the state machine and reset combo count
        /// </summary>
        public override void SetStateToInitial()
        {
            _currentComboIndex = 0;
            base.SetStateToInitial();
        }

        /// <summary>
        /// Transition to the next transition state in which we receive inputs to keep the combo
        /// </summary>
        public void SetNextState()
        {
            _isTransitioning = true;
            if(IsLastCombo)
            {
                _fromLastCombo = true;
                SetStateToInitial();
                return;
            }
            SetState(_transitions[_currentComboIndex]);
        }

        /// <summary>
        /// Trigger if input received, only if we are in a transition state
        /// </summary>
        protected void OnAttack()
        {
            if(_isTransitioning)
                _attackTriggered = true;
        }

        /// <summary>
        /// Change the current state of the movement, we want to stop movement whenever
        /// doing an attack and get it back on idle
        /// </summary>
        /// <param name="state"></param>
        public virtual void ChangeMovement(bool state) { }

        /// <summary>
        /// Move the user forward depending on the part of the combo
        /// </summary>
        /// <param name="distance"> Amount of distance has to move</param>
        public virtual void AddMovement(float distance, float duration) { }

        /// <summary>
        /// TODO: CHANGE THIS
        /// Trigger the animation of the current combo state
        /// </summary>
        /// <param name="animation"> Hash name of the animation </param>
        public virtual void SetAnimation(int animation) { }

        /// <summary>
        /// Calculate current damage of the attack depending on the user stats
        /// </summary>
        /// <returns> Damage of the attack </returns>
        public virtual float CalculateAttackDamage() 
        {
            return 0;
        }

        /// <summary>
        /// Calculate how long it will take for the attack to finish depending on the
        /// user stats
        /// </summary>
        /// <returns> Length of the attack </returns>
        public virtual float CalculateAttackDuration(string animationName)
        {
            return  0;
        }

        private void OnDrawGizmos() 
        {
            if(currentState != null && currentState.GetType() == typeof(CombatState))
                (currentState as CombatState).OnDrawGizmos();
        }
    }
}