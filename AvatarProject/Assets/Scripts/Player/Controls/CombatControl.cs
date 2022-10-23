using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using AvatarBA.Combat;
using AvatarBA.Common;
using AvatarBA.Managers;

namespace AvatarBA
{
    public class CombatControl : CombatManager
    {
        [Header("References")]
        [SerializeField]
        private InputManager _inputManager;

        private Core _core;
        private InputMiddleware _inputMovement;

        private const string ATTACK_STAT = "attackPower";
        private const string ATTACK_SPEED_STAT = "attackSpeed";

        private void Awake()
        {
            _inputManager.MeleeAttackEvent += OnAttack;
            _core = GetComponent<Core>();
            _inputMovement = GetComponent<InputMiddleware>();
        }

        private void OnDisable()
        {
            _inputManager.MeleeAttackEvent -= OnAttack;
        }

        public override void SetAnimation(int animation)
        {
            _core.Animation.PlayAnimation(animation);
        }

        public override float CalculateAttackDamage()
        {
            return _core.Stats.GetStat(ATTACK_STAT);
        }

        public override float CalculateAttackDuration(string animationName)
        {
            float animationLenght = _core.Animation.GetAnimationLength(animationName);
            float attackSpeed = _core.Stats.GetStat(ATTACK_SPEED_STAT);

            // If attack speed is different than 1 change play rate of the animation so is faster
            _core.Animation.ChangeAnimationSpeed("AttackSpeed", 1 + attackSpeed);
            return (1 - attackSpeed) * animationLenght;
        }

        public override void ChangeMovement(bool state)
        {
            if (state)
                _inputMovement.EnableMovementInput();
            else
                _inputMovement.DisableMovementInput();
        }

        public override void AddMovement(float distance)
        {
            StartCoroutine(AddMovementCoroutine(distance));
        }

        private IEnumerator AddMovementCoroutine(float distance)
        {
            Vector3 targetPosition = transform.position + (transform.forward * distance);
            Vector3 offset = targetPosition - transform.position;
            offset.y = 0;
            float cSquared = offset.x * offset.x + offset.z * offset.z;

            while (cSquared > 0.1f)
            {
                _core.Movement.Impulse(offset.normalized, moveSpeed);

                yield return null;
                offset = targetPosition - transform.position;
                offset.y = 0;
                cSquared = offset.x * offset.x + offset.z * offset.z;
            }
        }
    }
}
