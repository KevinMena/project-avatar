using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AvatarBA.Combat
{
    public class BoxHitbox : Hitbox
    {
        Vector3 _distance;
        Quaternion _rotation;

        public Vector3 Distance => _distance;
        public Quaternion Rotation => _rotation;

        public BoxHitbox(float distance, Quaternion rotation, LayerMask mask) : base(mask)
        {
            _distance = new Vector3(distance, distance / 2, distance);
            _rotation = rotation;
        }

        public override void CheckCollision()
        {
            if (_state == ColliderState.Closed)
                return;

            Collider[] hitColliders = Physics.OverlapBox(Position, Distance, Rotation, _mask);

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
            Gizmos.DrawWireCube(Position, Distance);
        }
    }
}