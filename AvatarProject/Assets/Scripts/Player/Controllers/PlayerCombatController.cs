using UnityEngine;

using AvatarBA.Combat;

namespace AvatarBA
{
    public class PlayerCombatController : CombatManager
    {
        [Header("References")]
        [SerializeField]
        private InputManager _inputManager;

        private PlayerStatsController _statsController;

        private void Awake() 
        {
            _inputManager.MeleeAttackEvent += OnAttack;
            animator = GetComponent<Animator>();
            _statsController = GetComponent<PlayerStatsController>();
        }

        private void OnDisable() 
        {
            _inputManager.MeleeAttackEvent -= OnAttack;
        }

        public override float CalculateAttackDamage()
        {
            return _statsController.GetStatValue("attackPower");
        }

        public override float CalculateAttackDuration(string animationName)
        {
            float animationLenght = GetAnimationLength(animationName);
            float attackSpeed = _statsController.GetStatValue("attackSpeed");

            // TODO: If attack speed is different than 1 then change play rate of the animation
            return (1 / attackSpeed) * animationLenght;
        }
    }
}
