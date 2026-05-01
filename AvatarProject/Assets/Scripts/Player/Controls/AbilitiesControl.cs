using UnityEngine;

using AvatarBA.Abilities;
using AvatarBA.Managers;
using AvatarBA.Abilities.Effects;
using AvatarBA.Debugging;

namespace AvatarBA
{
    public class AbilitiesControl : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private InputManager m_InputManager;

        [SerializeField]
        private AbilityMiddleware m_DisplayMiddleware;

        [Header("Abilities")]
        [SerializeField]
        private uint m_AbilitiesSlots = 4;
        [SerializeField]
        private AbilitySlot[] m_Abilities;

        private Core m_Core;
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
            SetupAbilities();
            m_currentContext = new ExecutionContext(m_Core);
        }

        private void Update()
        {
            for (int i = 0; i < m_Abilities.Length; i++)
            {
                AbilitySlot currentSlot = m_Abilities[i];

                if (currentSlot.State != AbilityState.Ready)
                {
                    UpdateAbility(currentSlot);
                }
            }
        }

        public void TriggerDash()
        {
            TriggerAbility(m_Abilities[0]);
        }

        public void TriggerLeftSlot()
        {
            TriggerAbility(m_Abilities[1]);
        }

        public void TriggerRightSlot()
        {
            TriggerAbility(m_Abilities[2]);
        }

        public void TriggerUltimate()
        {
            TriggerAbility(m_Abilities[3]);
        }

        public void TriggerAbility(AbilitySlot slot)
        {
            Ability currentAbility = slot.Ability;

            // TODO: Send message of ability not unlocked
            if (currentAbility == null)
                return;

            // TODO: Send message of not meeting requirements
            if (!PassRequirements(currentAbility.Cost))
                return;

            if (slot.State != AbilityState.Ready)
                return;

            // Trigger ability
            UpdateExecutionContext();
            currentAbility.Cast(m_currentContext);

            //Setup cooldown timer of the ability
            slot.CooldownTimer.TotalTime = currentAbility.Cooldown;
            slot.CooldownTimer.OnTimerCompleted += () =>
            {
                slot.CooldownTimer.ClearOnTimerCompleted();

                UpdateState(slot, AbilityState.Ready);
                EndDisplay(slot.Key);
            };

            // Use Active Timer if the ability has active time
            if (currentAbility.ActiveTime != 0)
            {
                slot.ActiveTimer.TotalTime = currentAbility.ActiveTime;
                slot.ActiveTimer.OnTimerCompleted += () =>
                {
                    slot.ActiveTimer.ClearOnTimerCompleted();

                    UpdateState(slot, AbilityState.Cooldown);
                    slot.CooldownTimer.Start();
                    StartCooldownDisplay(slot.Key, currentAbility.Cooldown);
                };
                UpdateState(slot, AbilityState.Active);

                slot.ActiveTimer.Start();
                StartActiveDisplay(slot.Key, currentAbility.ActiveTime);
                return;
            }

            UpdateState(slot, AbilityState.Cooldown);
            slot.CooldownTimer.Start();
            StartCooldownDisplay(slot.Key, currentAbility.Cooldown);
        }

        private void UpdateAbility(AbilitySlot slot)
        {
            if (slot.State == AbilityState.Active)
            {
                if (!slot.ActiveTimer.IsComplete)
                {
                    UpdateDisplay(slot.Key, slot.ActiveTimer.PercentElapsed, slot.ActiveTimer.RemainingTime);
                    slot.ActiveTimer.Update(Time.deltaTime);
                }
            }
            else if (slot.State == AbilityState.Cooldown)
            {
                if (!slot.CooldownTimer.IsComplete)
                {
                    UpdateDisplay(slot.Key, slot.CooldownTimer.PercentElapsed, slot.CooldownTimer.RemainingTime);
                    slot.CooldownTimer.Update(Time.deltaTime);
                }
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
            m_Abilities = new AbilitySlot[m_AbilitiesSlots];

            for (int i = 0; i < m_AbilitiesSlots; i++)
            {
                Ability initialAbility = i <= initialAbilities.Length - 1 ? initialAbilities[i] : null;
                m_Abilities[i] = new AbilitySlot((AbilityKey)i, initialAbility);

                if (initialAbility != null)
                {
                    UpdateIcon(m_Abilities[i].Key, initialAbility);
                }
            }
        }

        public void ReplaceAbility(int slot, Ability newAbility)
        {
            m_Abilities[slot].Ability = newAbility;
            UpdateIcon(m_Abilities[slot].Key, newAbility);
        }

        private void UpdateExecutionContext()
        {
            // TODO: Update context
        }

        private void UpdateState(AbilitySlot slot, AbilityState state)
        {
            slot.State = state;
        }

        private void StartCooldownDisplay(AbilityKey slot, float maxTimer)
        {
            m_DisplayMiddleware.StartCooldownTimer((int)slot, maxTimer);
        }

        private void StartActiveDisplay(AbilityKey slot, float maxTimer)
        {
            m_DisplayMiddleware.StartActiveTimer((int)slot, maxTimer);
        }

        private void EndDisplay(AbilityKey slot)
        {
            m_DisplayMiddleware.EndTimer((int)slot);
        }

        private void UpdateDisplay(AbilityKey slot, float current, float currentTimer)
        {
            m_DisplayMiddleware.UpdateDisplay((int)slot, current, currentTimer);
        }

        private void UpdateIcon(AbilityKey slot, Ability ability)
        {
            m_DisplayMiddleware.UpdateIcon((int)slot, ability.Icon);
        }
    }
}