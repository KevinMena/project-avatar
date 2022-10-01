using UnityEngine;

namespace AvatarBA.Combat
{ 
    public interface IProjectile
    {
        LayerMask Mask { get; }
        float BaseDamage { get; }
        float Speed { get; }
        float LifeTime { get; }

        void Setup(float damage, LayerMask mask, GameObject owner);
        void Move();
        void Explode();
        void OnEntityHit(Collider hit);
    }
}


