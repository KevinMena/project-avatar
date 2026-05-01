using UnityEngine;

namespace AvatarBA.Abilities.Effects
{
    [System.Serializable]
    public class DamageEffect : AbilityEffect
    {
        [SerializeField]
        protected float m_BaseDamage;

        public float BaseDamage => m_BaseDamage;
        protected const string ATTACK_STAT = "attackPower";

        public override void Execute(ExecutionContext context)
        {
            CalculateDamage(context.Owner);
            //TODO: Apply damage to target
        }

        protected float CalculateDamage(Core owner)
        {
            float ownerAttack = owner.Stats.GetStat(ATTACK_STAT);
            return BaseDamage * ownerAttack;
        }
    }
}