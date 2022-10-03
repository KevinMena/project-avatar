using UnityEngine;

using AvatarBA.Debugging;

namespace AvatarBA.Combat
{
    public class BoxAreaAttack : AreaAttack
    {
        public BoxHitbox Hitbox => _hitbox as BoxHitbox;

        public override void Setup(string name, Vector3 size, float distance, float damage, LayerMask mask, GameObject owner)
        {
            _attackName = name;
            _damage = damage;
            _distance = distance;
            _hitbox = new BoxHitbox(size, mask);
            _hitbox.OnCollision += HitEntity;
        }

        public override void StartAttack()
        {
            _hitbox.StartCheckCollision();
            _hitbox.Position = (transform.forward * (_distance / 2)) + transform.position;
            _hitbox.Rotation = transform.rotation;
            _isAttacking = true;
        }

        public override void StopAttack()
        {
            _hitbox.StopCheckCollision();
            _isAttacking = false;
            _alreadyHit.Clear();
            Destroy(gameObject);
        }

        protected override void HitEntity(Collider hit)
        {
            if (_alreadyHit.Contains(hit))
                return;

            _alreadyHit.Add(hit);

            GameDebug.Log($"Collisioned with {hit.name} using {_attackName}");
            if (hit.TryGetComponent(out Character character))
            {
                character.DoDamage(_damage);
            }
        }
    }
}