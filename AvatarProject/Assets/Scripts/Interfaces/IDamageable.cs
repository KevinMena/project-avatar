using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AvatarBA.Interfaces
{
    public interface IDamageable
    {
        void DoDamage(float damage);

        void DoHit(float damage, Vector2 hitPoint, Vector2 hitDirection);
    }
}