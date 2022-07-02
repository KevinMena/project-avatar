using UnityEngine;
using System;

using AvatarBA.Patterns;

namespace AvatarBA.Combat
{
    /// <summary>
    /// Data necessary for every state in the combo
    /// </summary>
    [Serializable]
    public struct CombatStateData
    {
        public string StateName;

        public string AnimationName;

        public float AttackRange;
    }

    /// <summary>
    /// State for the combat system. We calculate the damage if we trigger attacks on enemies
    /// and play the animations of the combat.
    /// </summary>
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
            damage = _owner.CalculateAttackDamage();
            attackDuration = _owner.CalculateAttackDuration(animationName);
            _owner.SetAnimation(animationName);
            UnityEngine.Debug.Log($"State: {stateName}");
        }

        public override void OnUpdate()
        {
            timer += Time.deltaTime;

            // If the attack is done then we transition to the transition state.
            if(timer > attackDuration)
            {
                _owner.SetNextState();
            }
        }

        public override void OnExit() { }

        /// <summary>
        /// Logic for finding if anny targets hit and damage the enemies
        /// </summary>
        public void Attack()
        {

        }
    }
}