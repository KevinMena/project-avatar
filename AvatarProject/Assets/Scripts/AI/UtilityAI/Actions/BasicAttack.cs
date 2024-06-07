using System.Collections.Generic;
using UnityEngine;

using AvatarBA.AI.Considerations;
using AvatarBA.Combat;
using AvatarBA.Stats;
using AvatarBA.Interfaces;

namespace AvatarBA.AI.Actions
{
    public class BasicAttack : Action
    {
        [SerializeField]
        private CombatStateData _data;

        [SerializeField]
        private LayerMask _hittableLayer;

        [SerializeField]
        private StatDefinition _attackStat;

        [SerializeField]
        private float _meleeRange;

        private ConeHitbox _hitbox;
        private Core _ownerCore;

        private readonly int _animationHash;
        private float _attackDuration = 0;
        private float _attackDamage = 0;

        private float _startupDuration = 0;
        private float _activeDuration = 0;

        private List<Collider> _alreadyHit;

        private float _timer = 0;

        protected override void Start()
        {
            _considerations = new Consideration[1] { new InRange(_meleeRange) };
            _ownerCore = GetComponentInParent<Core>();
            _alreadyHit = new List<Collider>();
            _hitbox = new ConeHitbox(_data.AttackRange, _data.AttackAngle, _hittableLayer);
            _hitbox.OnCollision += CollisionedWith;
        }

        public override void OnEnter()
        {
            _timer = 0;
            _attackDuration = 1f;
            _startupDuration = _attackDuration * 0.1f;
            _activeDuration = _attackDuration * 0.8f;
            _attackDamage = _ownerCore.Stats.GetStat(_attackStat.Id);

            _hitbox.StartCheckCollision();
        }

        public override void OnUpdate()
        {
            _timer += Time.deltaTime;

            // If the attack is done then we transition to the transition state.
            if (_timer > _attackDuration)
            {
                _completed = true;
                return;
            }

            // If the attack is just starting, then do nothing
            if (_timer < _startupDuration)
                return;

            // Window of attack activated
            if (_timer < _activeDuration)
                Attack();
        }

        public override void OnExit()
        {
            _hitbox.StopCheckCollision();
            _alreadyHit.Clear();
            _completed = false;
        }

        public void Attack()
        {
            _hitbox.Position = _ownerCore.HitPoint.position;
            _hitbox.Forward = _ownerCore.HitPoint.forward;
            _hitbox.Right = _ownerCore.HitPoint.right;
            _hitbox.CheckCollision();
        }

        public void CollisionedWith(Collider hit)
        {
            if (_alreadyHit.Contains(hit))
                return;

            _alreadyHit.Add(hit);

            if (hit.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(_attackDamage);
            }
        }
    }
}
