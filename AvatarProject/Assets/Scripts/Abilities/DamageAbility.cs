using UnityEngine;

namespace AvatarBA.Abilities
{
    public abstract class DamageAbility : Ability
    {
        [SerializeField]
        protected GameObject _prefab;

        [SerializeField]
        protected float _baseDamage;

        [SerializeField]
        protected LayerMask _mask;

        public float BaseDamage => _baseDamage;
        public LayerMask Mask => _mask;
        public GameObject Prefab => _prefab;

        protected const string ATTACK_STAT = "attackPower";

        protected float CalculateDamage(GameObject owner)
        {
            if(owner.TryGetComponent(out Core ownerCore))
            {
                float ownerAttack = ownerCore.Stats.GetStat(ATTACK_STAT);
                return BaseDamage * ownerAttack;
            }

            return 0;
        }
    }
}
