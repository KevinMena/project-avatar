using System.Collections.Generic;
using System.Linq;
using AvatarBA.Patterns;
using TMPro;
using UnityEngine;

namespace AvatarBA.AI
{
    public class BasicEnemy : MonoBehaviour
    {
        [SerializeReference, SerializeReferenceDropdown]
        private List<BaseState> m_States;

        [Header("Debugging")]
        [SerializeField]
        private TMP_Text m_stateText;

        private StateMachine m_StateMachine;
        private Core m_Core;
        private Sensors m_Sensors;
        private Dictionary<string, BaseState> m_StatesRegistry;

        private void Awake()
        {
            m_Core = GetComponent<Core>();
            m_Sensors = GetComponent<Sensors>();
        }

        private void Start()
        {
            for (int i = 0; i < m_States.Count; i++)
            {
                m_States[i].Setup(m_Core, m_Sensors);
            }
            m_StatesRegistry = m_States.ToDictionary(s => s.Id, s => s);

            m_StateMachine = new StateMachine(m_States[0], UpdateText);
        }

        private void Update()
        {
            string nextStateId = ((BaseState)m_StateMachine.CurrentState).ShouldTransition();

            if (!string.IsNullOrEmpty(nextStateId))
            {
                BaseState nextState = m_StatesRegistry.ContainsKey(nextStateId) ? m_StatesRegistry[nextStateId] : m_States[0];
                m_StateMachine.SetState(nextState);
                return;
            }

            m_StateMachine.Update();
        }

        private void FixedUpdate()
        {
            m_StateMachine.FixedUpdate();
        }

        private void UpdateText(string value)
        {
            m_stateText.text = value;
        }
    }
}
