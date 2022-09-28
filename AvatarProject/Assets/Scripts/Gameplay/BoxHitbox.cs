using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AvatarBA.Combat
{
    public class BoxHitbox : Hitbox
    {
        Vector3 _size;

        public Vector3 Size => _size;

        public BoxHitbox(Vector3 size, LayerMask mask) : base(mask)
        {
            _size = size;
        }

        public override void CheckCollision()
        {
            if (_state == ColliderState.Closed)
                return;

            Collider[] hitColliders = Physics.OverlapBox(Position, Size, Rotation, _mask);

            List<Collider> alreadyHit = new List<Collider>();

            _state = hitColliders.Length > 0 ? ColliderState.Colliding : ColliderState.Open;
            ChangeGizmosColor();

            foreach (var hit in hitColliders)
            {
                if (alreadyHit.Contains(hit))
                    continue;

                OnCollision?.Invoke(hit);

                alreadyHit.Add(hit);
            }
        }

        public override void OnDrawGizmos()
        {
            Gizmos.color = _currentColor;
            Gizmos.matrix = Matrix4x4.TRS(Position, Rotation, Vector3.one);
            Gizmos.DrawWireCube(Vector3.zero, new Vector3(Size.x * 2, Size.y * 2, Size.z * 2));
        }
    }
}