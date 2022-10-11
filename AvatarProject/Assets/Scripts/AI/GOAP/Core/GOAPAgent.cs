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

        private Goal _currentGoal;

        private bool _interruptPlan;

        private WorldContext _currentWorldContext;

        private float GOAL_COOLDOWN = 0.5f;

        public bool start = false;

        private void Start()
        {
            _planner = new Planner();
            _memory = new GOAPMemory();
            SetupWorld();
            _currentWorldContext = new WorldContext();
            _currentWorldContext.Add(new WorldState("equippedMelee", true));
        }

        private void OnDestroy()
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
        }

        private IEnumerator RunPlan()
        {
            GameDebug.Log("Finding Plan");
            Action[] availableActions = _planner.GetAvailableActions(_actions.ToArray());
            ChooseGoal();
            Queue<Action> plan = _planner.GetPlan(_currentGoal, availableActions, _currentWorldContext);

            GameDebug.Log("Started Plan");

            while (plan.Count > 0)
            {
                Action currentAction = plan.Dequeue();
                yield return currentAction.Perform(gameObject);
            }
            GameDebug.Log("Finished Plan");
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

