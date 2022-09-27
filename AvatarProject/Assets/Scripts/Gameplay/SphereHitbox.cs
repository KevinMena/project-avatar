using System.Collections.Generic;
using UnityEngine;

namespace AvatarBA.Combat
{
    public class SphereHitbox : Hitbox
    {
        private float _radius;

        public float Radius { get => _radius; }

        public SphereHitbox(float radius, LayerMask mask) : base(mask)
        {
            _radius = radius;
        }

        public override void CheckCollision()
        {
            if (_state == ColliderState.Closed)
                return;

            Collider[] hitColliders = Physics.OverlapSphere(Position, Radius, _mask);

            List<Collider> alreadyHit = new List<Collider>();

            _state = hitColliders.Length > 0 ? ColliderState.Colliding : ColliderState.Open;
            ChangeGizmosColor();

            foreach (var hit in hitColliders)
            {
                if (alreadyHit.Contains(hit))
                    continue;

                _collider?.CollisionedWith(hit);

                alreadyHit.Add(hit);
            }
        }

        public override void OnDrawGizmos()
        {
            Gizmos.color = _currentColor;
            Gizmos.DrawWireSphere(Position, Radius);
        }
    }
}