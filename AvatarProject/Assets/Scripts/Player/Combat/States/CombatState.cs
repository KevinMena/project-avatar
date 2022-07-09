using UnityEngine;
using System;
using System.Collections.Generic;

using AvatarBA.Patterns;
using AvatarBA.Debugging;
using AvatarBA.Interfaces;

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
    public class CombatState : State, ICollisionable
    {
        private CombatManager _owner; 
        private Hitbox _hitbox;

        private string _stateName;
        private string _animationName;
        private readonly int _animationHash;

        private float _attackDuration = 0;
        private float _attackRange = 0;
        private float _attackDamage = 0;

        private float _timer = 0;

        public CombatState(CombatManager owner, CombatStateData data) : base(owner) 
        {
            _owner = owner as CombatManager;
            _stateName = data.StateName;
            _animationName = data.AnimationName;
            _animationHash = Animator.StringToHash(data.AnimationName);
            _attackRange = data.AttackRange;
            
            _hitbox = new Hitbox(_attackRange / 2, _owner.HittableLayer);
            _hitbox.SubscribeCollider(this);
        }

        public override void OnEnter()
        {
            _timer = 0;
            _attackDamage = _owner.CalculateAttackDamage();
            _attackDuration = _owner.CalculateAttackDuration(_animationName);
            _owner.SetAnimation(_animationHash);
            GameDebug.Log($"State: {_stateName}. Attack Duration: {_attackDuration}");

            _hitbox.StartCheckCollision();
        }

        public override void OnUpdate()
        {
            _timer += Time.deltaTime;

            // If the attack is done then we transition to the transition state.
            if(_timer > _attackDuration)
            {
                _owner.SetNextState();
            }

            Attack();
        }

        public override void OnExit() 
        {
            _hitbox.StopCheckCollision();
        }

        public void CollisionedWith(Collider collider)
        {
            GameDebug.Log($"Collisioned with {collider.name} in state: {_stateName}");
            if(collider.TryGetComponent<Character>(out Character character))
            {
                character.DoDamage(_attackDamage);
            }
        }

        /// <summary>
        /// Logic for finding if any targets hit and damage the enemies
        /// </summary>
        public void Attack()
        {
            _hitbox.Position = _owner.HitPoint.position;
            _hitbox.CheckCollision();
        }

        public void OnDrawGizmos()
        {
            _hitbox.OnDrawGizmos();
        }
    }
}