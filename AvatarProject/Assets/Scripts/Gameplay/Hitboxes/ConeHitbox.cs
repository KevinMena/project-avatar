using System.Collections.Generic;
using UnityEngine;

namespace AvatarBA.Combat
{
    public class ConeHitbox : Hitbox
    {
        private float _distance;
        private float _angle;
        Vector3 _forward;
        Vector3 _right;

        private float _cosAngle;

        public float Distance => _distance;
        public float Angle => _angle;
        public float CosAngle => _cosAngle;
        public Vector3 Forward { get => _forward; set => _forward = value; }
        public Vector3 Right { get => _right; set => _right = value; }

        public ConeHitbox(float distance, float angle, LayerMask mask) : base(mask)
        {
            _distance = distance;
            _angle = angle;

            _cosAngle = Mathf.Cos(_angle * Mathf.Deg2Rad);
        }

        public override void CheckCollision()
        {
            if (_state == ColliderState.Closed)
                return;

            // Get all entities in a range defined by the distance
            Collider[] hitColliders = Physics.OverlapSphere(Position, Distance, _mask);

            _state = ColliderState.Open;
            ChangeGizmosColor();

            // Check if any entity hit is inside our cone of vision
            foreach (var hit in hitColliders)
            {
                Vector3 directionToTarget = hit.transform.position - Position;

                // Calculating the dot product of the angle towards the entity and
                // our facing direction gives us if vectors overlaps
                float hitAngle = Vector3.Dot(directionToTarget.normalized, Forward);
                
                // If the angle if less than the Cosine of our cone angle then
                // is not inside
                if (hitAngle < CosAngle)
                    continue;

                _state = ColliderState.Colliding;
                ChangeGizmosColor();

                OnCollision?.Invoke(hit);
            }
        }

        public override void OnDrawGizmos()
        {
            Gizmos.color = _currentColor;
            Gizmos.DrawWireSphere(Position, Distance);
            GizmosExtensions.DrawWireArc(Position, Forward, Angle, Distance, 2);
        }
    }
}