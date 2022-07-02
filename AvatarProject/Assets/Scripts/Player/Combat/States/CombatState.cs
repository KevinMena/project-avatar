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
        protected string animationName;

        protected float attackDuration = 0;
        protected float attackRange = 0;
        protected float damage = 0;

        protected float timer = 0;

        public CombatState(CombatManager owner, CombatStateData data) : base(owner) 
        {
            _owner = owner as CombatManager;
            stateName = data.StateName;
            animationName = data.AnimationName;
            attackRange = data.AttackRange;
        }

        public override void OnEnter()
        {
            timer = 0;
            damage = CalculateDamage();
            attackDuration = CalculateAttackDuration();
            _owner.SetAnimation(animationName);
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
            return _owner.GetAnimationLength(animationName);
        }
    }
}