using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

namespace AvatarBA.Common
{    
    public class AnimationController : MonoBehaviour
    {
        private Animator _animator;

        private Dictionary<string, AnimatorState> _states;
        private int _currentAnimation;
        private readonly int IdleAnimation = Animator.StringToHash("Idle");

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void Start() 
        {
            _currentAnimation = IdleAnimation;
            //TODO: Change this to have every state for layer
            AnimatorState[] _allStates = GetAnimatorStateInfo(0);
            _states = new Dictionary<string, AnimatorState>();

            foreach (var state in _allStates)
            {
                _states[state.name] = state;
            }
        }

        public void PlayInitialAnimation()
        {
            PlayAnimation(IdleAnimation);
        }

        public void PlayAnimation(int animation, float transitionDelay = 0.25f, int layerIndex = 0)
        {
            if(_currentAnimation == animation)
                return;
            
            _currentAnimation = animation;
            AnimatorStateInfo current = _animator.GetCurrentAnimatorStateInfo(layerIndex);
            _animator.CrossFade(animation, transitionDelay/current.length, layerIndex);
        }

        public float GetAnimationLength(string animationName)
        {
            if(_states.TryGetValue(animationName, out var animationState))
            {
                return (animationState.motion as AnimationClip).length;
            }

            return 0;
        }

        public void ChangeAnimationSpeed(string parameter, float value)
        {
            float currentValue = _animator.GetFloat(parameter);
            if (currentValue != 0 && currentValue - value < 0.01)
                return;

            _animator.SetFloat(parameter, value);
        }

        public AnimatorState[] GetAnimatorStateInfo(int layer)
        {
            AnimatorController ac = _animator.runtimeAnimatorController as AnimatorController;
            List<AnimatorState> allStates = new List<AnimatorState>();
            AnimatorControllerLayer currentLayer = ac.layers[layer];
            ChildAnimatorState[] animStates = currentLayer.stateMachine.states;
            foreach (ChildAnimatorState j in animStates)
            {
                allStates.Add(j.state);
            }

            return allStates.ToArray();
        }
    }

}
