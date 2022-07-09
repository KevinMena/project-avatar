using System.Collections.Generic;
using UnityEngine;

namespace AvatarBA.Common
{    
    public class AnimationController : MonoBehaviour
    {
        private Animator _animator;
        private AnimationState[] _states;

        private Dictionary<string, AnimationClip> _clips;
        private int _currentAnimation;
        private readonly int IdleAnimation = Animator.StringToHash("Idle"); 

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void Start() 
        {
            _currentAnimation = IdleAnimation;
            _clips = new Dictionary<string, AnimationClip>();
            AnimationClip[] animations = _animator.runtimeAnimatorController.animationClips;

            foreach (AnimationClip animation in animations)
            {
                _clips[animation.name] = animation;
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
            if(_clips.TryGetValue(animationName, out var animation))
            {
                return animation.length;
            }

            return 0;
        }
    }
}
