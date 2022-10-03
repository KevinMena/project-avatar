using System.Collections.Generic;
using UnityEngine;

using AvatarBA.Debugging;

namespace AvatarBA.Combat
{
    public abstract class AreaAttack : MonoBehaviour
    {
        protected string _attackName;
        protected Hitbox _hitbox;
        protected float _damage;
        protected float _distance;

        protected bool _isAttacking;
        protected List<Collider> _alreadyHit = new List<Collider>();

        protected void OnDestroy()
        {
            if(_hitbox != null)
                _hitbox.OnCollision -= HitEntity;
        }

        protected void Update()
        {
            if (_hitbox != null && _isAttacking)
                _hitbox.CheckCollision();
        }

        public abstract void Setup(string name, Vector3 size, float distance, float damage, LayerMask mask, GameObject owner);

        public abstract void StartAttack();
        public abstract void StopAttack();

        protected abstract void HitEntity(Collider hit);

        protected void OnDrawGizmos()
        {
            if(_hitbox != null)
                _hitbox.OnDrawGizmos();
        }
    }
}
