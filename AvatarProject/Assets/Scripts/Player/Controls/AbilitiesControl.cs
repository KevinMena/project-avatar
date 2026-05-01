using System.Collections;
using UnityEngine;

using AvatarBA.Common;
using AvatarBA.Abilities;
using AvatarBA.Managers;
using AvatarBA.Abilities.Effects;
using AvatarBA.Debugging;

namespace AvatarBA
{
    public enum AbilitySlot
    {
        Dash,
        Left,
        Right,
        Ultimate
    }

    public class AbilitiesControl : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private InputManager m_InputManager;

        [SerializeField]
        private AbilityMiddleware m_DisplayMiddleware;

        [Header("Abilities")]
        [SerializeField]
        private Ability m_Dash;

        [SerializeField]
        private Ability m_LeftAbility;

        [SerializeField]
        private Ability m_RightAbility;

        [SerializeField]
        private Ability m_Ultimate;

        private Core m_Core;
        private AbilityState[] m_AbilityStates;
        private Timer[] m_CooldownTimers;
        private Timer[] m_ActiveTimers;

        private ExecutionContext m_currentContext;

        private void Awake()
        {
            m_InputManager.DashEvent += TriggerDash;
            m_InputManager.LeftAbilityEvent += TriggerLeftSlot;
            m_InputManager.RightAbilityEvent += TriggerRightSlot;
            m_InputManager.UltimateAbilityEvent += TriggerUltimate;
            m_Core = GetComponent<Core>();
        }

        private void OnDisable()
        {
            m_InputManager.DashEvent -= TriggerDash;
            m_InputManager.LeftAbilityEvent -= TriggerLeftSlot;
            m_InputManager.RightAbilityEvent -= TriggerRightSlot;
            m_InputManager.UltimateAbilityEvent -= TriggerUltimate;
        }

        private void Start()
        {
            m_AbilityStates = new AbilityState[4]
            {
                AbilityState.Ready,
                AbilityState.Ready,
                AbilityState.Ready,
                AbilityState.Ready
            };

            m_CooldownTimers = new Timer[4]
            {
                new Timer(0),
                new Timer(0),
                new Timer(0),
                new Timer(0)
            };

            m_ActiveTimers = new Timer[4]
            {
                new Timer(0),
                new Timer(0),
                new Timer(0),
                new Timer(0)
            };

            SetupAbilities();
            m_currentContext = new ExecutionContext(m_Core);
        }

        private void Update()
        {
            for (int i = 0; i < m_AbilityStates.Length; i++)
            {
                AbilityState currentState = m_AbilityStates[i];

                if (currentState != AbilityState.Ready)
                {
                    UpdateAbility((AbilitySlot)i);
                }
            }
        }

        public void TriggerDash()
        {
            TriggerAbility(AbilitySlot.Dash, m_Dash);
        }

        public void TriggerLeftSlot()
        {
            TriggerAbility(AbilitySlot.Left, m_LeftAbility);
        }

        public void TriggerRightSlot()
        {
            TriggerAbility(AbilitySlot.Right, m_RightAbility);
        }

        public void TriggerUltimate()
        {
            TriggerAbility(AbilitySlot.Ultimate, m_Ultimate);
        }

        public void TriggerAbility(AbilitySlot slot, Ability currentAbility)
        {
            // TODO: Send message of ability not unlocked
            if (currentAbility == null)
                return;

            // TODO: Send message of not meeting requirements
            if (!PassRequirements(currentAbility.Cost))
                return;

            AbilityState currentState = GetCurrentState(slot);

            if (currentState != AbilityState.Ready)
                return;

            // Trigger ability
            UpdateExecutionContext();
            currentAbility.Cast(m_currentContext);

            //Setup cooldown timer of the ability
            Timer cooldownTimer = m_CooldownTimers[(int)slot];
            cooldownTimer.TotalTime = currentAbility.Cooldown;
            cooldownTimer.OnTimerCompleted += () =>
            {
                cooldownTimer.ClearOnTimerCompleted();

                UpdateState(slot, AbilityState.Ready);
                EndDisplay(slot);
            };

            // Use Active Timer if the ability has active time
            if (currentAbility.ActiveTime != 0)
            {
                Timer activeTimer = m_ActiveTimers[(int)slot];
                activeTimer.TotalTime = currentAbility.ActiveTime;
                activeTimer.OnTimerCompleted += () =>
                {
                    activeTimer.ClearOnTimerCompleted();

                    UpdateState(slot, AbilityState.Cooldown);
                    cooldownTimer.Start();
                    StartCooldownDisplay(slot, currentAbility.Cooldown);
                };
                UpdateState(slot, AbilityState.Active);

                activeTimer.Start();
                StartActiveDisplay(slot, currentAbility.ActiveTime);
                return;
            }

            UpdateState(slot, AbilityState.Cooldown);
            cooldownTimer.Start();
            StartCooldownDisplay(slot, currentAbility.Cooldown);
        }

        private void UpdateAbility(AbilitySlot slot)
        {
            AbilityState currentState = GetCurrentState(slot);
            Timer currentTimer = null;

            if (currentState == AbilityState.Active)
            {
                currentTimer = m_ActiveTimers[(int)slot];
            }
            else if (currentState == AbilityState.Cooldown)
            {
                currentTimer = m_CooldownTimers[(int)slot];
            }

            if (currentTimer != null && !currentTimer.IsComplete)
            {
                UpdateDisplay(slot, currentTimer.PercentElapsed, currentTimer.RemainingTime);
                currentTimer.Update(Time.deltaTime);
            }
        }


        private bool PassRequirements(float cost)
        {
            return true;
        }

        private void SetupAbilities()
        {
            // Get the initial abilities of the current character
            Ability[] initialAbilities = m_Core.Data.InitialAbilities;

            for (int i = 0; i < initialAbilities.Length; i++)
            {
                if (i == ((int)AbilitySlot.Dash))
                {
                    m_Dash = initialAbilities[i];
                    UpdateIcon(AbilitySlot.Dash, m_Dash);
                }
                else if (i == ((int)AbilitySlot.Left))
                {
                    m_LeftAbility = initialAbilities[i];
                    UpdateIcon(AbilitySlot.Left, m_LeftAbility);
                }
                else if (i == ((int)AbilitySlot.Right))
                {
                    m_RightAbility = initialAbilities[i];
                    UpdateIcon(AbilitySlot.Right, m_RightAbility);
                }
                else if (i == ((int)AbilitySlot.Ultimate))
                {
                    m_Ultimate = initialAbilities[i];
                    UpdateIcon(AbilitySlot.Ultimate, m_Ultimate);
                }
            }
        }

        public void ReplaceAbility(AbilitySlot slot, Ability newAbility)
        {
            switch (slot)
            {
                case AbilitySlot.Dash:
                    m_Dash = newAbility;
                    break;
                case AbilitySlot.Left:
                    m_LeftAbility = newAbility;
                    break;
                case AbilitySlot.Right:
                    m_RightAbility = newAbility;
                    break;
                case AbilitySlot.Ultimate:
                    m_Ultimate = newAbility;
                    break;
                default:
                    break;
            }

            UpdateIcon(slot, newAbility);
        }

        private void UpdateExecutionContext()
        {
            // TODO: Update context
        }

        private void UpdateState(AbilitySlot slot, AbilityState state)
        {
            m_AbilityStates[(int)slot] = state;
        }

        private AbilityState GetCurrentState(AbilitySlot slot)
        {
            return m_AbilityStates[(int)slot];
        }

        private void StartCooldownDisplay(AbilitySlot slot, float maxTimer)
        {
            m_DisplayMiddleware.StartCooldownTimer((int)slot, maxTimer);
        }

        private void StartActiveDisplay(AbilitySlot slot, float maxTimer)
        {
            m_DisplayMiddleware.StartActiveTimer((int)slot, maxTimer);
        }

        private void EndDisplay(AbilitySlot slot)
        {
            m_DisplayMiddleware.EndTimer((int)slot);
        }

        private void UpdateDisplay(AbilitySlot slot, float current, float currentTimer)
        {
            m_DisplayMiddleware.UpdateDisplay((int)slot, current, currentTimer);
        }

        private void UpdateIcon(AbilitySlot slot, Ability ability)
        {
            m_DisplayMiddleware.UpdateIcon((int)slot, ability.Icon);
        }
    }
}