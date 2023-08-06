using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using AvatarBA.Patterns;

namespace AvatarBA.AI.States
{
    public class EnemyStateMachine : StateMachine
    {
        public bool StateComplete => currentState == null ? true : (currentState as BaseState).Completed;

        protected override void Start() { }

        protected override void Update() 
        {
            base.Update();

            if(StateComplete)
                FinishState();
        }

        public override void SetState(IState nextState)
        {
            currentState = nextState;
            currentState.OnEnter();
        }

        private void FinishState()
        {
            currentState?.OnExit();
            currentState = null;
        }
    }
}
