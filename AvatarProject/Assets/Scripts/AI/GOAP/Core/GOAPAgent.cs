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
        private Coroutine ActionPerformed = null;

        private const float GOAL_COOLDOWN = 10f;

        public bool start = false;

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
            // Interrupt current plan and execute new one
            if (start)
            {
                start = false;
                StartCoroutine(RunPlan());
            }

            _goalTimer += Time.deltaTime;

            // If not goal or goal cooldown ended try an choose a new goal
            if(_currentGoal is null || _goalTimer > GOAL_COOLDOWN)
            {
                GameDebug.Log("Choosing Goal");
                ChooseGoal();
                _goalTimer = 0;
            }

            if(_interruptPlan)
            {
                GameDebug.Log("Plan interrupted");
                _interruptPlan = false;
                FindPlan();
            }

            if(!_runningPlan && _currentGoal is not null)
            {
                GameDebug.Log("No plan started, finding...");
                FindPlan();
            }

            if(!_performingAction && _currentPlan.Count > 0)
            {
                GameDebug.Log("Performing next action");
                _currentAction = _currentPlan.Dequeue();
                StartCoroutine(PerformAction());
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
            _currentPlan = _planner.GetPlan(_currentGoal, availableActions, _currentWorldContext);
        }

        private IEnumerator RunPlan()
        {
            GameDebug.Log("Finding Plan");
            Action[] availableActions = _planner.GetAvailableActions(_actions.ToArray());
            ChooseGoal();
            Queue<Action> plan = _planner.GetPlan(_currentGoal, availableActions, _currentWorldContext);

            GameDebug.Log("Started Plan");
            _runningPlan = true;
            while (plan.Count > 0)
            {
                Action currentAction = plan.Dequeue();
                ActionPerformed = StartCoroutine(currentAction.Perform(gameObject));
                yield return ActionPerformed;
            }
            GameDebug.Log("Finished Plan");
            _runningPlan = false;
            yield return null;
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

