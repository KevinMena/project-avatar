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
    
    public abstract class Hitbox
    {
        protected Vector3 _position;
        protected LayerMask _mask;
        protected ColliderState _state;

        protected Color _closedColor;
        protected Color _openColor;
        protected Color _collidingColor;

        protected Color _currentColor;

        protected ICollisionable _collider;

        public Vector3 Position { get => _position; set => _position = value; }
        public ColliderState State { get => _state; }

        public Hitbox(LayerMask mask)
        {
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

        public abstract void CheckCollision();

        protected void ChangeGizmosColor()
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

        public abstract void OnDrawGizmos();
    }
}
