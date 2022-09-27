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

            List<Collider> alreadyHit = new List<Collider>();

            _state = ColliderState.Open;
            ChangeGizmosColor();

            // Check if any entity hit is inside our cone of vision
            foreach (var hit in hitColliders)
            {
                if (alreadyHit.Contains(hit))
                    continue;

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

                _collider?.CollisionedWith(hit);

                alreadyHit.Add(hit);
            }
        }

        public override void OnDrawGizmos()
        {
            Gizmos.color = _currentColor;
            Gizmos.DrawWireSphere(Position, Distance);
            DrawWireArc(Position, Forward, Angle, Distance, 2);
        }

        private void DrawWireArc(Vector3 position, Vector3 dir, float anglesRange, float radius, float maxSteps = 20)
        {
            var srcAngles = GetAnglesFromDir(position, dir);
            var initialPos = position;
            var posA = initialPos;
            var stepAngles = anglesRange / maxSteps;
            var angle = srcAngles - anglesRange / 2;
            for (var i = 0; i <= maxSteps; i++)
            {
                var rad = Mathf.Deg2Rad * angle;
                var posB = initialPos;
                posB += new Vector3(radius * Mathf.Cos(rad), 0, radius * Mathf.Sin(rad));

                Gizmos.DrawLine(posA, posB);

                angle += stepAngles;
                posA = posB;
            }
            Gizmos.DrawLine(posA, initialPos);
        }

        private float GetAnglesFromDir(Vector3 position, Vector3 dir)
        {
            var forwardLimitPos = position + dir;
            var srcAngles = Mathf.Rad2Deg * Mathf.Atan2(forwardLimitPos.z - position.z, forwardLimitPos.x - position.x);

            return srcAngles;
        }
    }
}