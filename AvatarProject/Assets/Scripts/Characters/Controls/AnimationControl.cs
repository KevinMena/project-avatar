using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

using AvatarBA.Common;

namespace AvatarBA
{
    public class AnimationControl : MonoBehaviour
    {
        private Animator m_animator;

        private Dictionary<string, AnimatorState> m_states;
        private int m_currentAnimation;
        private Timer m_animationTimer;
        private bool m_backToInitial = false;

        private readonly int IdleAnimation = Animator.StringToHash("Idle");

        private void Awake()
        {
            m_animator = GetComponent<Animator>();
        }

        private void Start()
        {
            m_currentAnimation = IdleAnimation;
            //TODO: Change this to have every state for layer
            AnimatorState[] _allStates = GetAnimatorStateInfo(0);
            m_states = new Dictionary<string, AnimatorState>();

            foreach (var state in _allStates)
            {
                m_states[state.name] = state;
            }
        }

        private void Update()
        {
            if(m_backToInitial)
                m_animationTimer.Update(Time.deltaTime);
        }

        public void PlayInitialAnimation()
        {
            PlayAnimation(IdleAnimation);
        }

        public void PlayInitialAnimation(int animation)
        {
            if (m_currentAnimation != animation)
                return;
            PlayAnimation(IdleAnimation);
        }

        public void PlayAnimation(int animation, bool backToInitial = false, float transitionDelay = 0.25f, int layerIndex = 0)
        {
            if (m_currentAnimation == animation)
                return;

            m_currentAnimation = animation;
            AnimatorStateInfo current = m_animator.GetCurrentAnimatorStateInfo(layerIndex);
            m_animator.CrossFade(animation, transitionDelay / current.length, layerIndex);

            if(backToInitial)
            {
                m_backToInitial = true;
                m_animationTimer = new Timer(current.length);
                m_animationTimer.OnTimerCompleted += ResetAnimation;
                m_animationTimer.Start();
            }
        }

        public float GetAnimationLength(string animationName)
        {
            if (m_states.TryGetValue(animationName, out var animationState))
            {
                return (animationState.motion as AnimationClip).length;
            }

            return 0;
        }

        public void ChangeAnimationSpeed(string parameter, float value)
        {
            float currentValue = m_animator.GetFloat(parameter);
            if (currentValue != 0 && currentValue - value < 0.01)
                return;

            m_animator.SetFloat(parameter, value);
        }

        private AnimatorState[] GetAnimatorStateInfo(int layer)
        {
            AnimatorController ac = m_animator.runtimeAnimatorController as AnimatorController;
            List<AnimatorState> allStates = new List<AnimatorState>();
            AnimatorControllerLayer currentLayer = ac.layers[layer];
            ChildAnimatorState[] animStates = currentLayer.stateMachine.states;
            foreach (ChildAnimatorState j in animStates)
            {
                allStates.Add(j.state);
            }

            return allStates.ToArray();
        }

        private void ResetAnimation()
        {
            m_backToInitial = false;
            PlayInitialAnimation();
        }
    }
}