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
        private InputManager m_inputManager;

        [Header("Data")]
        [SerializeField]
        private CombatStateData[] m_combatStatesData;

        [SerializeField]
        private LayerMask m_hittableLayer;

        private CombatState[] m_combatStates;
        private CombatTransitionState[] m_transitions;

        protected int m_currentComboIndex = 0;

        protected bool m_attackTriggered = false;
        protected bool m_isTransitioning = false;
        protected bool m_fromLastCombo = false;
        protected const float TIMER_FOR_INPUT = 0.5f;
        protected const float LAST_COMBO_DELAY = 0.5f;
        protected const float IMPULSE_SPEED = 3f;

        private Core m_core;
        private InputProcessor m_inputMovement;

        private const string ATTACK_STAT = "attackPower";
        private const string ATTACK_SPEED_STAT = "attackSpeed";

        public LayerMask HittableLayer => m_hittableLayer;
        public Transform HitPoint => m_core.HitPoint;
        public bool Comboing => m_currentComboIndex > 0;
        public bool AttackTriggered => m_attackTriggered;
        public bool IsLastCombo => m_currentComboIndex >= m_combatStates.Length;
        public bool FromLastCombo => m_fromLastCombo;
        public float LastComboDelay => LAST_COMBO_DELAY;

        private void Awake()
        {
            m_inputManager.MeleeAttackEvent += OnAttack;
            m_core = GetComponent<Core>();
            m_inputMovement = GetComponent<InputProcessor>();
        }

        private void OnDisable()
        {
            m_inputManager.MeleeAttackEvent -= OnAttack;
        }

        protected override void Start()
        {
            GenerateStates();
            initialState = m_transitions[0];
            m_isTransitioning = true;
            SetInitialState();
        }

        /// <summary>
        /// Instantiate all the necessary combat states and transitions states
        /// depending on the length of the combo. Also instantiate the idle state
        /// </summary>
        private void GenerateStates()
        {
            int numberStates = m_combatStatesData.Length;

            m_combatStates = new CombatState[numberStates];

            for (int i = 0; i < numberStates; i++)
            {
                m_combatStates[i] = new CombatState(this, m_combatStatesData[i]);
            }

            m_transitions = new CombatTransitionState[numberStates];

            m_transitions[0] = new CombatIdleState(this, m_combatStates[0]);

            for (int i = 1; i < numberStates; i++)
            {
                m_transitions[i] = new CombatTransitionState(this, m_combatStates[i], TIMER_FOR_INPUT);
            }
        }

        /// <summary>
        /// Transition to one of the main combo states. In this states we don't register inputs
        /// </summary>
        /// <param name="nextState"> Combo state to transition </param>
        public void SetMainState(IState nextState)
        {
            m_currentComboIndex++;
            m_isTransitioning = false;
            m_attackTriggered = false;
            m_fromLastCombo = false;
            SetState(nextState);
        }

        /// <summary>
        /// Reset the state machine and reset combo count
        /// </summary>
        public override void SetStateToInitial()
        {
            m_currentComboIndex = 0;
            base.SetStateToInitial();
        }

        /// <summary>
        /// Transition to the next transition state in which we receive inputs to keep the combo
        /// </summary>
        public void SetNextState()
        {
            m_isTransitioning = true;
            if (IsLastCombo)
            {
                m_fromLastCombo = true;
                SetStateToInitial();
                return;
            }
            SetState(m_transitions[m_currentComboIndex]);
        }

        /// <summary>
        /// Trigger if input received, only if we are in a transition state
        /// </summary>
        protected void OnAttack()
        {
            if (m_isTransitioning)
                m_attackTriggered = true;
        }

        /// <summary>
        /// Trigger the animation of the current combo state
        /// </summary>
        /// <param name="animation"> Hash name of the animation </param>
        public void SetAnimation(int animation)
        {
            m_core.Animation.PlayAnimation(animation);
        }

        /// <summary>
        /// Calculate current damage of the attack depending on the user stats
        /// </summary>
        /// <returns> Damage of the attack </returns>
        public float CalculateAttackDamage()
        {
            return m_core.Stats.GetStat(ATTACK_STAT);
        }

        /// <summary>
        /// Calculate how long it will take for the attack to finish depending on the
        /// user stats
        /// </summary>
        /// <returns> Length of the attack </returns>
        public float CalculateAttackDuration(string animationName)
        {
            float animationLenght = m_core.Animation.GetAnimationLength(animationName);
            float attackSpeed = m_core.Stats.GetStat(ATTACK_SPEED_STAT);

            // If attack speed is different than 1 change play rate of the animation so is faster
            m_core.Animation.ChangeAnimationSpeed("AttackSpeed", 1 + attackSpeed);
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
                m_core.Movement.EnableMovement();
            else
                m_core.Movement.DisableMovement();
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
            Vector3 targetPosition = transform.position + (m_core.Movement.AimDirection * distance);
            
            m_core.Movement.Impulse(m_core.Movement.AimDirection, IMPULSE_SPEED);

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
