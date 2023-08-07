using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using AvatarBA.AI.States;
using AvatarBA.Common;
using AvatarBA.Debugging;

namespace AvatarBA.AI
{
    public class AIBrain : MonoBehaviour
    {
        [SerializeField]
        private float _actionDelay;

        [SerializeField]
        private Action[] _actionsAvailable;

        [SerializeField]
        private TMP_Text _statusText;

        private Action _bestAction;
        private Action _lastAction;
        private Timer _timer;

        private EnemyStateMachine _stateMachine;

        private bool _actionDecided = false;
        private bool _doingAction = false;

        public Action LastAction => _lastAction;

        private void Awake()
        {
            _stateMachine = GetComponent<EnemyStateMachine>();
            _stateMachine.OnStateFinish += FinishAction;
        }

        private void OnDestroy()
        {
            _stateMachine.OnStateFinish -= FinishAction;
            if (_timer != null)
                _timer.OnTimerCompleted -= CalculateBestAction;
        }

        private void Start()
        {
            _timer = new Timer(_actionDelay);
            _timer.OnTimerCompleted += CalculateBestAction;

            CalculateBestAction();
            ExecuteBestAction();
            _actionDecided = false;
        }

        private void Update()
        {
            // Execute best action calculated
            if (_actionDecided && !_doingAction && _bestAction != null)
            {
                ExecuteBestAction();
                _actionDecided = false;
            }

            if (!_doingAction)
                _timer.Update(Time.deltaTime);
        }

        private void ExecuteBestAction()
        {
            GameDebug.Log($"Starting execute of action {_bestAction.Name}");
            _doingAction = true;
            _statusText.text = _bestAction.Name;
            _bestAction.Execute(_stateMachine);
        }

        private void FinishAction()
        {
            GameDebug.Log($"Finished action {_bestAction.Name}");
            _doingAction = false;
            _lastAction = _bestAction;
            _statusText.text = "Deciding";
            _timer.Start();
        }

        private void CalculateBestAction()
        {
            GameDebug.Log($"Calculating action...");

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