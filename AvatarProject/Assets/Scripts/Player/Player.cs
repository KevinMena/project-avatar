using UnityEngine;

namespace AvatarBA
{
    public class Player : Character
    {
        private PlayerStatsController playerStatsController;

        private const string DEFENSE_STAT = "defense";

        private void Awake()
        {
            playerStatsController = GetComponent<PlayerStatsController>();    
        }

        public override void DoDamage(float damage)
        {
            // Damage formula
            float appliedDamage = damage / playerStatsController.GetStat(DEFENSE_STAT);
            playerStatsController.ApplyChangeToHealth("damage", -appliedDamage, Stats.StatModifierType.Flat);
        }

        public override void DoHit(float damage, Vector2 hitPoint, Vector2 hitDirection)
        {
            // lose health and also move the character
        }
    }
}