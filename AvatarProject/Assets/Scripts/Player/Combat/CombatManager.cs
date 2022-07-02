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

        private bool _attackTriggered = false;
        private bool _isTransitioning = false;

        // TODO: CHANGED THIS
        private const float timerForInput = 0.8f;

        private Animator _animator;

        public Animator Anim
        {
            set => _animator = value;
        }

        public bool AttackTriggered => _attackTriggered;

        public override void Start()
        {
            GenerateStates();
            initialState = _transitions[0];
            _isTransitioning = true;
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
            _isTransitioning = false;
            _attackTriggered = false;
            SetState(nextState);
        }

        public override void SetStateToInitial()
        {
            _currentComboIndex = 0;
            base.SetStateToInitial();
        }

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

        public void TriggerCombo()
        {
            if(_isTransitioning)
                _attackTriggered = true;
        }

        public void SetAnimation(string animationName)
        {
            _animator.SetTrigger(animationName);
        }

        public float GetAnimationLength(string animationName)
        {
            AnimationClip[] animations = _animator.runtimeAnimatorController.animationClips;
            foreach (var animation in animations)
            {
                if(animation.name == animationName)
                    return animation.length;
            }

            return 0;
        }
    }
}