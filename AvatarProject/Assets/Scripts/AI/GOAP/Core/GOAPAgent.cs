using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using AvatarBA.Common.DataStructures;
using AvatarBA.Debugging;

namespace AvatarBA.AI.Core
{
    public class GOAPAgent : MonoBehaviour
    {
        [SerializeField]
        private List<Goal> _goals;

        [SerializeField]
        private List<Action> _actions;

        private Planner _planner;

        private GOAPMemory _memory;

        private WorldContext _currentWorldContext;
        Queue<Action> _currentPlan;
        private Goal _currentGoal = null;
        private Action _currentAction = null;

        private bool _performingAction;
        private bool _runningPlan;
        private bool _interruptPlan;
        private float _goalTimer;

        private const float GOAL_COOLDOWN = 10f;

        public bool PlanEmpty => _currentPlan is not null && _currentPlan.Count == 0;

        private void Start()
        {
            _planner = new Planner();
            _memory = new GOAPMemory();
            SetupWorld();
            _currentWorldContext = new WorldContext
                                {
                                    new WorldState("equippedMelee", true) 
                                };
            _currentPlan = new Queue<Action>();
            _goalTimer = 0;
            _interruptPlan = false;
            _runningPlan = false;
            _performingAction = false;
        }

        private void OnDisable()
        {
            CleanWorld();
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
                FindPlan();
            }
            
            // If we still have actions available in the plan, keep doing it
            if(!_performingAction && !PlanEmpty)
            {
                GameDebug.Log("Performing next action");
                _currentAction = _currentPlan.Dequeue();
                StartCoroutine(PerformAction());
            }

            // If we finish last action then we finish the plan
            if(!_performingAction && PlanEmpty)
                _runningPlan = false;
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
            _currentPlan = _planner.GetPlan(_currentGoal, availableActions, _currentWorldContext);
            _runningPlan = true;
        }

        private void ChooseGoal()
        {
            Goal[] validGoals = _planner.GetValidGoals(_goals.ToArray());

            PriorityQueue<int, Goal> goalsPriorities = new PriorityQueue<int, Goal>();
            foreach(Goal goal in validGoals)
            {
                goalsPriorities.Enqueue(goal.CalculatePriority(), goal);
            }

            if (_currentGoal is not null && goalsPriorities.Peek() != _currentGoal)
                _interruptPlan = true;

            _currentGoal = goalsPriorities.Peek();
        }

        // This is for testing purposes
        private void SetupWorld()
        {
            for(int i = 0; i < _actions.Count; i++)
            {
                _actions[i].SetupWorld();
            }

            for (int i = 0; i < _goals.Count; i++)
            {
                _goals[i].SetupWorld();
            }
        }

        private void CleanWorld()
        {
            for (int i = 0; i < _actions.Count; i++)
            {
                _actions[i].CleanWorld();
            }

            for (int i = 0; i < _goals.Count; i++)
            {
                _goals[i].CleanWorld();
            }
        }
    }
}

