using UnityEngine;

using AvatarBA.Combat;

namespace AvatarBA
{
    public class PlayerCombatController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private InputManager _inputManager;

        [SerializeField]
        private CombatManager _combatManager;

        // For now
        private Animator _animator;

        private void Awake() 
        {
            _inputManager.MeleeAttackEvent += OnAttack;
            _animator = GetComponent<Animator>();
        }

        private void OnDisable() 
        {
            _inputManager.MeleeAttackEvent -= OnAttack;
        }

        private void Start() 
        {
            _combatManager.Start();
            _combatManager.Anim = _animator;
        }

        private void Update() 
        {
            _combatManager.Update();
        }

        private void OnAttack()
        {
            _combatManager.TriggerCombo();
        }
    }
}
