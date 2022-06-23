using UnityEngine;

namespace AvatarBA
{
    public class Player : Character
    {
        public override void DoDamage(float damage)
        {
            // lose health and stuff
        }

        public override void DoHit(float damage, Vector2 hitPoint, Vector2 hitDirection)
        {
            // lose health and also move the character
        }
    }
}