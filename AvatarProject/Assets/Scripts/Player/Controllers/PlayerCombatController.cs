using UnityEngine;

using AvatarBA.Combat;
using AvatarBA.Common;

namespace AvatarBA
{
    public class PlayerCombatController : CombatManager
    {
        [Header("References")]
        [SerializeField]
        private InputManager _inputManager;

        private AnimationController _animationController;

        private PlayerStatsController _statsController;

        private PlayerMovementController _movementController;

        private void Awake() 
        {
            _inputManager.MeleeAttackEvent += OnAttack;
            _animationController = GetComponent<AnimationController>();
            _statsController = GetComponent<PlayerStatsController>();
            _movementController = GetComponent<PlayerMovementController>();
        }

        private void OnDisable() 
        {
            _inputManager.MeleeAttackEvent -= OnAttack;
        }

        public override void SetAnimation(int animation)
        {
            _animationController.PlayAnimation(animation);
        }

        public override float CalculateAttackDamage()
        {
            // TODO: Change this to take into account critical chance
            return _statsController.GetStatValue("attackPower");
        }

        public override float CalculateAttackDuration(string animationName)
        {
            float animationLenght = _animationController.GetAnimationLength(animationName);
            float attackSpeed = _statsController.GetStatValue("attackSpeed");

            // TODO: If attack speed is different than 1 then change play rate of the animation
            return (1 / attackSpeed) * animationLenght;
        }

        public override void ChangeMovement(bool state)
        {
            if(state)
                _movementController.EnableMovement();
            else
                _movementController.DisableMovement();
        }
    }
}
