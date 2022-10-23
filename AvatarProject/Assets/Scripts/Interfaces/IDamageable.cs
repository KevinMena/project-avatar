using UnityEngine;

namespace AvatarBA.Interfaces
{
    public interface IDamageable
    {
        void TakeDamage(float damage);

        void TakeHit(float damage, Vector2 hitDirection);
    }
}