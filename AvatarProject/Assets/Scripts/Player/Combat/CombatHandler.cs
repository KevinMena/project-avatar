using UnityEngine;

namespace AvatarBA.Combat
{
    public class CombatHandler : MonoBehaviour
    {
        protected CombatState currentState;
        protected CombatState initialState;

        // States
        protected CombatIdleState _idleState;
        protected CombatEntryState _entryState;
        protected CombatFollowUpState _followUpState;
        protected CombatFinisherState _finisherState;

        public CombatIdleState IdleState => _idleState;
        public CombatEntryState EntryState => _entryState;
        public CombatFollowUpState FollowUpState => _followUpState;
        public CombatFinisherState FinisherState => _finisherState;

        protected virtual void Start()
        {
            if(initialState != null)
                SetState(initialState);
        }

        protected virtual void Update()
        {
            currentState?.OnUpdate();
        }

        public void SetState(CombatState nextState)
        {
            currentState?.OnExit();
            currentState = nextState;
            currentState.OnEnter();
        }

        public void SetStateToInitial()
        {
            SetState(initialState);
        }
    }
}