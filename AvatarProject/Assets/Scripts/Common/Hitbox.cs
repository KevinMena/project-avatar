using System.Collections.Generic;
using UnityEngine;

using AvatarBA.Interfaces;

namespace AvatarBA.Combat
{
    public enum ColliderState
    {
        Closed,
        Open,
        Colliding
    }
    
    public class Hitbox
    {
        private Vector3 _position;
        private float _radius;
        private LayerMask _mask;
        public ColliderState _state;

        private Color _closedColor;
        private Color _openColor;
        private Color _collidingColor;

        private Color _currentColor;

        private ICollisionable _collider;

        public Vector3 Position { get => _position; set => _position = value; }
        public float Radius { get => _radius;}

        public Hitbox(float radius, LayerMask mask)
        {
            _radius = radius;
            _mask = mask;
            _state = ColliderState.Closed;

            _closedColor = Color.red;
            _openColor = Color.blue;
            _collidingColor = Color.green;
        }

        public void SubscribeCollider(ICollisionable collider)
        {
            _collider = collider;
        }

        public void StartCheckCollision()
        {
            _state = ColliderState.Open;
        }

        public void StopCheckCollision()
        {
            _state = ColliderState.Closed;
        }

        public void CheckCollision()
        {
            if(_state == ColliderState.Closed)
                return;
            
            Collider[] hitColliders = Physics.OverlapSphere(Position, Radius, _mask);

            List<Collider> alreadyHit = new List<Collider>();

            _state = hitColliders.Length > 0 ? ColliderState.Colliding : ColliderState.Open;
            ChangeGizmosColor();

            foreach (var hit in hitColliders)
            {
                if(alreadyHit.Contains(hit))
                    continue;
                
                _collider?.CollisionedWith(hit);

                alreadyHit.Add(hit);
            }
        }

        private void ChangeGizmosColor()
        {
            switch (_state)
            {
                case ColliderState.Closed:
                    _currentColor = _closedColor;
                    break;
                case ColliderState.Open:
                    _currentColor = _openColor;
                    break;
                case ColliderState.Colliding:
                    _currentColor = _collidingColor;
                    break;
                default:
                    _currentColor = Color.black;
                    break;
            }
        }

        public void OnDrawGizmos()
        {
            Gizmos.color = _currentColor;
            Gizmos.DrawWireSphere(Position, Radius);
        }
    }
}
