using UnityEngine;

namespace AvatarBA.Interfaces
{
    public interface ICollisionable
    {
        void CollisionedWith(Collider collider);
    }
}