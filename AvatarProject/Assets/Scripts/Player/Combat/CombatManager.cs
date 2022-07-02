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
        
        private CombatState[] _combatStates;
        private CombatTransitionState[] _transitions;

        private int _currentComboIndex = 0;

        private bool _attackTriggered = false;
        private bool _isTransitioning = false;

        // TODO: CHANGED THIS
        private const float timerForInput = 0.8f;

        protected Animator animator;

        public bool AttackTriggered => _attackTriggered;

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
            if(_currentComboIndex >= _combatStates.Length)
            {
                SetStateToInitial();
                return;
            }
            SetState(_transitions[_currentComboIndex]);
        }

        /// <summary>
        /// Trigger if input received, only if we are in a transition state
        /// </summary>
        public void OnAttack()
        {
            if(_isTransitioning)
                _attackTriggered = true;
        }

        /// <summary>
        /// TODO: CHANGE THIS
        /// Trigger the animation of the current combo state
        /// </summary>
        /// <param name="animationName"> Name of the animation </param>
        public void SetAnimation(string animationName)
        {
            animator.SetTrigger(animationName);
        }

        /// <summary>
        /// Get the length of the animation we want to play
        /// </summary>
        /// <param name="animationName"> Name of the animation </param>
        /// <returns> Length </returns>
        public float GetAnimationLength(string animationName)
        {
            AnimationClip[] animations = animator.runtimeAnimatorController.animationClips;
            foreach (var animation in animations)
            {
                if(animation.name == animationName)
                    return animation.length;
            }

            return 0;
        }

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
    }
}