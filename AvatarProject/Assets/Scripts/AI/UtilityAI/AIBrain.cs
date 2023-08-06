using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using AvatarBA.AI.States;

namespace AvatarBA.AI
{
    public class AIBrain : MonoBehaviour
    {
        [SerializeField]
        private Action[] _actionsAvailable;
        
        private Action _bestAction;

        private EnemyStateMachine _stateMachine;

        private bool _actionDecided = false;
        private bool _doingAction = false;

        private void Awake()
        {
            _stateMachine = GetComponent<EnemyStateMachine>();
        }

        private void Start()
        {
            if (_bestAction == null)
                CalculateBestAction();
        }

        private void Update()
        {
            // Execute best action calculated
            if (_actionDecided && !_doingAction && _bestAction != null)
            {
                ExecuteBestAction();
                _actionDecided = false;
            }

            // Check if the action state is done to recalculate action
            if (_stateMachine.StateComplete)
            {
                _doingAction = false;
                CalculateBestAction();
            }
        }

        private void ExecuteBestAction()
        {
            _doingAction = true;
            _bestAction.Execute(_stateMachine);
        }

        private void CalculateBestAction()
        {
            float bestScore = 0;
            int bestActionIndex = 0;

            for (int i = 0; i < _actionsAvailable.Length; i++)
            {
                float actionScore = ScoreAction(_actionsAvailable[i]);
                if (actionScore > bestScore)
                {
                    bestScore = actionScore;
                    bestActionIndex = i;
                }
            }

            _bestAction = _actionsAvailable[bestActionIndex];
            _actionDecided = true;
        }

        private float ScoreAction(Action action)
        {
            float finalScore = 1f;

            // Compensation value created by Dave Mark for normalizing the values of the scores
            float compensationFactor = 1 - (1 / action.Considerations.Length);

            for (int i = 0; i < action.Considerations.Length; i++)
            {
                // "Averaging" the scores
                float considerationScore = action.Considerations[i].CalculateScore(gameObject);
                float makeupValue = (1 - considerationScore) * compensationFactor;
                considerationScore = considerationScore + (makeupValue * considerationScore);

                finalScore *= considerationScore;

                // No point computing any further
                if (finalScore == 0)
                    return 0;
            }

            return finalScore;
        }
    }
}