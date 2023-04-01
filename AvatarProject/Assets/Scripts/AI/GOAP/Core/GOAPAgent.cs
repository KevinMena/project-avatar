using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using AvatarBA.Common.DataStructures;
using AvatarBA.Debugging;
using TMPro;

namespace AvatarBA.AI.Core
{
    public class GOAPAgent : MonoBehaviour
    {
        [SerializeField]
        private List<Goal> _goals;

        [SerializeField]
        private List<Action> _actions;

        [SerializeField]
        private TMP_Text _statusText;

        private Planner _planner;

        private GOAPMemory _memory;

        Queue<Action> _currentPlan;
        [SerializeField]
        private Goal _currentGoal = null;
        private Action _currentAction = null;

        private bool _performingAction;
        private bool _runningPlan;
        private bool _interruptPlan;
        private float _goalTimer;

        private const float GOAL_COOLDOWN = 10f;

        public bool PlanEmpty => _currentPlan is not null && _currentPlan.Count == 0;

        private void Awake()
        {
            _memory = GetComponent<GOAPMemory>();
        }

        private void Start()
        {
            _planner = new Planner();
            _currentPlan = new Queue<Action>();
            _goalTimer = 0;
            _interruptPlan = false;
            _runningPlan = false;
            _performingAction = false;

            // Testing
            _memory.AddWorldState("EquippedMelee", true);
        }

        private void Update()
        {
            // Every GOAL_COOLDOWN try and get a new plan
            _goalTimer += Time.deltaTime;

            // If not goal or goal cooldown ended try an choose a new goal
            if(_currentGoal is null || _goalTimer > GOAL_COOLDOWN)
            {
                GameDebug.Log("Choosing Goal");
                ChooseGoal();
                _goalTimer = 0;
            }

            // If new goal found while doing another plan then replace the plan
            if(_interruptPlan)
            {
                GameDebug.Log("Plan interrupted");
                _interruptPlan = false;
                _runningPlan = false;
                FindPlan();
            }

            // If not plan started for new goal, find and start plan
            if(!_runningPlan && _currentGoal is not null)
            {
                GameDebug.Log("No plan started, finding...");
                _statusText.text = "No plan";
                FindPlan();
            }
            
            // If we still have actions available in the plan, keep doing it
            if(!_performingAction && !PlanEmpty)
            {
                _currentAction = _currentPlan.Dequeue();
                _statusText.text = $"{_currentAction.Id}";
                StartCoroutine(PerformAction());
            }

            // If we finish last action then we finish the plan
            if(!_performingAction && PlanEmpty)
            {
                _runningPlan = false;
                _currentGoal = null;
                GameDebug.Log("Finished plan.");
            }
        }

        private IEnumerator PerformAction()
        {
            _performingAction = true;
            yield return _currentAction.Perform(gameObject);
            _performingAction = false;
        }

        private void FindPlan()
        {
            Action[] availableActions = _planner.GetAvailableActions(_actions.ToArray());
            _currentPlan = _planner.GetPlan(_currentGoal, availableActions, _memory.CurrentContext);
            _runningPlan = true;
        }

        private void ChooseGoal()
        {
            Goal[] validGoals = _planner.GetValidGoals(gameObject, _goals.ToArray());

            PriorityQueue<int, Goal> goalsPriorities = new PriorityQueue<int, Goal>();
            foreach(Goal goal in validGoals)
            {
                goalsPriorities.Enqueue(goal.CalculatePriority(), goal);
            }

            if (_currentGoal is not null && goalsPriorities.Peek() != _currentGoal)
                _interruptPlan = true;

            _currentGoal = goalsPriorities.Peek();
        }
    }
}

