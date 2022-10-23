using UnityEngine;

using AvatarBA.Combat;
using AvatarBA.Common;
using System.Collections;

namespace AvatarBA
{
    public class PlayerCombatController : CombatManager
    {
        [Header("References")]
        [SerializeField]
        private InputManager _inputManager;

        private AnimationControl _animationController;

        private PlayerStatsController _statsController;

        private PlayerMovementController _movementController;

        private const string ATTACK_STAT = "attackPower";
        private const string ATTACK_SPEED_STAT = "attackSpeed";

        private void Awake() 
        {
            _inputManager.MeleeAttackEvent += OnAttack;
            _animationController = GetComponent<AnimationControl>();
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
            return _statsController.GetStat(ATTACK_STAT);
        }

        public override float CalculateAttackDuration(string animationName)
        {
            float animationLenght = _animationController.GetAnimationLength(animationName);
            float attackSpeed = _statsController.GetStat(ATTACK_SPEED_STAT);

            // If attack speed is different than 1 change play rate of the animation so is faster
            _animationController.ChangeAnimationSpeed("AttackSpeed", attackSpeed);
            return (1 - attackSpeed) * animationLenght;
        }

        public override void ChangeMovement(bool state)
        {
            if(state)
                _movementController.EnableMovement();
            else
                _movementController.DisableMovement();
        }

        public override void AddMovement(float distance)
        {
            StartCoroutine(MoveOwner(distance));
        }

        private IEnumerator MoveOwner(float distance)
        {
            Vector3 targetPosition = transform.position + (transform.forward * distance);

            Vector3 offset = targetPosition - transform.position;

            while (offset.magnitude > 0.1f)
            {
                Vector3 movementDirection = moveSpeed * Time.deltaTime * offset;
                _movementController.AddMovement(movementDirection);
                yield return null;
                offset = targetPosition - transform.position;
            }
        }
    }
}
