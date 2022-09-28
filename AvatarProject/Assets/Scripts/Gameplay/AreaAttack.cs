using AvatarBA.Debugging;

using UnityEngine;

namespace AvatarBA.Combat
{
    public abstract class AreaAttack : MonoBehaviour
    {
        protected string _attackName;
        protected Hitbox _hitbox;
        protected float _damage;
        protected float _distance;

        protected bool _isAttacking;

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

        public abstract void Setup(string name, float distance, float damage, LayerMask mask, GameObject owner);

        public abstract void StartAttack();
        public abstract void StopAttack();

        protected void HitEntity(Collider hit)
        {
            GameDebug.Log($"Collisioned with {hit.name} using {_attackName}");
            if (hit.TryGetComponent(out Character character))
            {
                character.DoDamage(_damage);
            }
        }

        protected void OnDrawGizmos()
        {
            if(_hitbox != null)
                _hitbox.OnDrawGizmos();
        }
    }
}
