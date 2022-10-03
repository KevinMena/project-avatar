using UnityEngine;
using System;

using AvatarBA.Patterns;
using AvatarBA.Debugging;
using System.Collections.Generic;

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

        public float AttackAngle;

        public float AttackMovementDistance;
    }

    /// <summary>
    /// State for the combat system. We calculate the damage if we trigger attacks on enemies
    /// and play the animations of the combat.
    /// </summary>
    public class CombatState : State
    {
        private CombatManager _owner; 
        private ConeHitbox _hitbox;

        private string _stateName;
        private string _animationName;
        private readonly int _animationHash;

        private float _attackMovementDistance = 0; 
        private float _attackDuration = 0;
        private float _attackDamage = 0;

        private List<Collider> _alreadyHit;

        private float _timer = 0;

        public CombatState(CombatManager owner, CombatStateData data) : base(owner) 
        {
            _owner = owner;
            _stateName = data.StateName;
            _animationName = data.AnimationName;
            _animationHash = Animator.StringToHash(data.AnimationName);
            _attackMovementDistance = data.AttackMovementDistance;
            _alreadyHit = new List<Collider>();

            _hitbox = new ConeHitbox(data.AttackRange, data.AttackAngle, _owner.HittableLayer);
            _hitbox.OnCollision += CollisionedWith;
        }

        public override void OnEnter()
        {
            _timer = 0;
            _attackDamage = _owner.CalculateAttackDamage();
            _attackDuration = _owner.CalculateAttackDuration(_animationName);
            _owner.SetAnimation(_animationHash);
            GameDebug.Log($"State: {_stateName}. Attack Duration: {_attackDuration}");

            _owner.AddMovement(_attackMovementDistance, _attackDuration);
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
            _alreadyHit.Clear();
        }

        /// <summary>
        /// Handle what to do with entities collided
        /// </summary>
        public void CollisionedWith(Collider hit)
        {
            if (_alreadyHit.Contains(hit))
                return;

            _alreadyHit.Add(hit);

            GameDebug.Log($"Collisioned with {hit.name} in state: {_stateName}");
            if(hit.TryGetComponent(out Character character))
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
            _hitbox.Forward = _owner.HitPoint.forward;
            _hitbox.Right = _owner.HitPoint.right;
            _hitbox.CheckCollision();
        }

        public void OnDrawGizmos()
        {
            _hitbox.OnDrawGizmos();
        }
    }
}