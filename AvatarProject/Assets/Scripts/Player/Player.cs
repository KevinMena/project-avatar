using UnityEngine;

namespace AvatarBA
{
    public class Player : Character
    {
        private PlayerStatsController playerStatsController;

        private void Awake()
        {
            playerStatsController = GetComponent<PlayerStatsController>();    
        }

        public override void DoDamage(float damage)
        {
            playerStatsController.ApplyChangeToHealth("damage", -damage, Stats.StatModifierType.Flat);
        }

        public override void DoHit(float damage, Vector2 hitPoint, Vector2 hitDirection)
        {
            // lose health and also move the character
        }
    }
}