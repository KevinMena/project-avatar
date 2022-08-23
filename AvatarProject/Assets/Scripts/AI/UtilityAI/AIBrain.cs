using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AvatarBA.AI
{
    public class AIBrain : MonoBehaviour
    {
        [SerializeField]
        private Action[] _actionsAvailable;
        
        private Action bestAction;

        private bool _isDeciding = false;
        private bool _doingAction = false;

        private void Start()
        {
            if (bestAction == null)
                StartCoroutine(CalculateBestAction());
        }

        private void Update()
        {
            if (!_isDeciding && !_doingAction)
                StartCoroutine(ExecuteBestAction());
        }

        private IEnumerator ExecuteBestAction()
        {
            _doingAction = true;
            yield return bestAction.Execute(gameObject);
            _doingAction = false;
            yield return CalculateBestAction();
        }

        private IEnumerator CalculateBestAction()
        {
            _isDeciding = true;

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

            bestAction = _actionsAvailable[bestActionIndex];
            
            _isDeciding = false;
            yield return null;
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