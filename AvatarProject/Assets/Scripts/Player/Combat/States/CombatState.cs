using UnityEngine;
using System;

using AvatarBA.Patterns;

namespace AvatarBA.Combat
{
    [Serializable]
    public struct CombatStateData
    {
        public string StateName;

        public string AnimationName;

        public float AttackRange;
    }

    public class CombatState : State
    {
        private CombatManager _owner; 
        protected string stateName;

        // Animator for now
        // protected Animator animator;
        
        protected string animationName;

        protected float attackDuration = 0;
        protected float attackRange = 0;
        protected float damage = 0;

        protected float timer = 0;

        public CombatState(CombatManager owner, CombatStateData data) : base(owner) 
        {
            _owner = owner as CombatManager;
            //animator = owner.GetComponent<Animator>();
            stateName = data.StateName;
            animationName = data.AnimationName;
            attackRange = data.AttackRange;
        }

        public override void OnEnter()
        {
            timer = 0;
            damage = CalculateDamage();
            attackDuration = CalculateAttackDuration();
            //animator.SetTrigger(animationName);
            UnityEngine.Debug.Log($"State: {stateName}");
        }

        public override void OnExit()
        {
            
        }

        public override void OnUpdate()
        {
            timer += Time.deltaTime;

            if(timer > attackDuration)
            {
                _owner.SetNextState();
            }
        }

        private float CalculateDamage()
        {
            return 0;
        }

        private float CalculateAttackDuration()
        {
            // AnimationClip[] animations = animator.runtimeAnimatorController.animationClips;
            // foreach (var animation in animations)
            // {
            //     if(animation.name == animationName)
            //         return animation.length;
            // }

            return 1f;
        }
    }
}