using System.Collections;
using UnityEngine;

using AvatarBA.Combat;
using AvatarBA.Managers;

using AvatarBA.Patterns;

namespace AvatarBA
{
    /// <summary>
    /// Manager of the combat system. The system is combo based combat of a number of states.
    /// We keep the current combo index to know when to finish the combo and to reset all the states.
    /// </summary>
    public class CombatControl : StateMachine
    {
        [Header("References")]
        [SerializeField]
        private InputManager _inputManager;

        [Header("Data")]
        [SerializeField]
        private CombatStateData[] _combatStatesData;

        [SerializeField]
        private LayerMask _hittableLayer;

        private CombatState[] _combatStates;
        private CombatTransitionState[] _transitions;

        protected int _currentComboIndex = 0;

        protected bool _attackTriggered = false;
        protected bool _isTransitioning = false;
        protected bool _fromLastCombo = false;
        protected const float timerForInput = 0.5f;
        protected const float lastComboDelay = 0.5f;
        protected const float moveSpeed = 3f;

        private Core _core;
        private InputProcessor _inputMovement;

        private const string ATTACK_STAT = "attackPower";
        private const string ATTACK_SPEED_STAT = "attackSpeed";

        public LayerMask HittableLayer => _hittableLayer;
        public Transform HitPoint => _core.HitPoint;
        public bool Comboing => _currentComboIndex > 0;
        public bool AttackTriggered => _attackTriggered;
        public bool IsLastCombo => _currentComboIndex >= _combatStates.Length;
        public bool FromLastCombo => _fromLastCombo;
        public float LastComboDelay => lastComboDelay;

        private void Awake()
        {
            _inputManager.MeleeAttackEvent += OnAttack;
            _core = GetComponent<Core>();
            _inputMovement = GetComponent<InputProcessor>();
        }

        private void OnDisable()
        {
            _inputManager.MeleeAttackEvent -= OnAttack;
        }

        protected override void Start()
        {
            GenerateStates();
            initialState = _transitions[0];
            _isTransitioning = true;
            SetInitialState();
        }

        /// <summary>
        /// Instantiate all the necessary combat states and transitions states
        /// depending on the length of the combo. Also instantiate the idle state
        /// </summary>
        private void GenerateStates()
        {
            int numberStates = _combatStatesData.Length;

            _combatStates = new CombatState[numberStates];

            for (int i = 0; i < numberStates; i++)
            {
                _combatStates[i] = new CombatState(this, _combatStatesData[i]);
            }

            _transitions = new CombatTransitionState[numberStates];

            _transitions[0] = new CombatIdleState(this, _combatStates[0]);

            for (int i = 1; i < numberStates; i++)
            {
                _transitions[i] = new CombatTransitionState(this, _combatStates[i], timerForInput);
            }
        }

        /// <summary>
        /// Transition to one of the main combo states. In this states we don't register inputs
        /// </summary>
        /// <param name="nextState"> Combo state to transition </param>
        public void SetMainState(IState nextState)
        {
            _currentComboIndex++;
            _isTransitioning = false;
            _attackTriggered = false;
            _fromLastCombo = false;
            SetState(nextState);
        }

        /// <summary>
        /// Reset the state machine and reset combo count
        /// </summary>
        public override void SetStateToInitial()
        {
            _currentComboIndex = 0;
            base.SetStateToInitial();
        }

        /// <summary>
        /// Transition to the next transition state in which we receive inputs to keep the combo
        /// </summary>
        public void SetNextState()
        {
            _isTransitioning = true;
            if (IsLastCombo)
            {
                _fromLastCombo = true;
                SetStateToInitial();
                return;
            }
            SetState(_transitions[_currentComboIndex]);
        }

        /// <summary>
        /// Trigger if input received, only if we are in a transition state
        /// </summary>
        protected void OnAttack()
        {
            if (_isTransitioning)
                _attackTriggered = true;
        }

        /// <summary>
        /// Trigger the animation of the current combo state
        /// </summary>
        /// <param name="animation"> Hash name of the animation </param>
        public void SetAnimation(int animation)
        {
            _core.Animation.PlayAnimation(animation);
        }

        /// <summary>
        /// Calculate current damage of the attack depending on the user stats
        /// </summary>
        /// <returns> Damage of the attack </returns>
        public float CalculateAttackDamage()
        {
            return _core.Stats.GetStat(ATTACK_STAT);
        }

        /// <summary>
        /// Calculate how long it will take for the attack to finish depending on the
        /// user stats
        /// </summary>
        /// <returns> Length of the attack </returns>
        public float CalculateAttackDuration(string animationName)
        {
            float animationLenght = _core.Animation.GetAnimationLength(animationName);
            float attackSpeed = _core.Stats.GetStat(ATTACK_SPEED_STAT);

            // If attack speed is different than 1 change play rate of the animation so is faster
            _core.Animation.ChangeAnimationSpeed("AttackSpeed", 1 + attackSpeed);
            return (1 - attackSpeed) * animationLenght;
        }

        /// <summary>
        /// Change the current state of the movement, we want to stop movement whenever
        /// doing an attack and get it back on idle
        /// </summary>
        /// <param name="state"></param>
        public void ChangeMovement(bool state)
        {
            if (state)
                _inputMovement.EnableMovementInput();
            else
                _inputMovement.DisableMovementInput();
        }

        /// <summary>
        /// Move the user forward depending on the part of the combo
        /// </summary>
        /// <param name="distance"> Amount of distance has to move</param>
        public void AddMovement(float distance)
        {
            StartCoroutine(AddMovementCoroutine(distance));
        }

        private IEnumerator AddMovementCoroutine(float distance)
        {
            Vector3 targetPosition = transform.position + (transform.forward * distance);
            
            _core.Movement.Impulse(transform.forward, moveSpeed);

            float cSquared;
            do
            {
                Vector3 offset = transform.position.TargetDirection(targetPosition);
                offset.y = 0;
                cSquared = offset.Distance();
                yield return null;
            }while (cSquared > 0.1f);
        }

        private void OnDrawGizmos()
        {
            if (currentState != null && currentState.GetType() == typeof(CombatState))
                (currentState as CombatState).OnDrawGizmos();
        }
    }
}
