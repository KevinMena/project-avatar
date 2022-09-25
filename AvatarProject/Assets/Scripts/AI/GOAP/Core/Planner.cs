using System.Collections;
using System.Collections.Generic;
using System;

using AvatarBA.Common.DataStructures;
using AvatarBA.Debugging;

namespace AvatarBA.AI.Core
{
    [Serializable]
    public struct WorldState
    {
        public string Id;
        public bool Value;

        public WorldState(string id, bool value)
        {
            Id = id;
            Value = value;
        }
    }

    public struct StateNode
    {
        public string Id;
        public WorldState[] State;

        public int CostSoFar;
        public int Priority;

        public StateNode(string id, WorldState[] state)
        {
            Id = id;
            State = state;
            CostSoFar = 0;
            Priority = state.Length;
        }
    }

    public class Planner
    {
        public Queue<Action> GetPlan(Goal goal, Action[] availableActions, List<WorldState> currentWorldState)
        {
            StateNode root = new StateNode(goal.Id, goal.DesiredState);

            Queue<Action> bestPlan = BuildPlan(root, availableActions, currentWorldState);

            return bestPlan;
        }

        private Queue<Action> BuildPlan(StateNode root, Action[] availableActions, List<WorldState> currentWorldState)
        {
            PriorityQueue<int, StateNode> frontier = new PriorityQueue<int, StateNode>();
            Stack<StateNode> actionsToTake = new Stack<StateNode>();
            frontier.Enqueue(root.Priority, root);

            while(frontier.Count > 0)
            {
                StateNode currentNode = frontier.Dequeue();
                actionsToTake.Push(currentNode);

                GameDebug.Log($"Current node {currentNode.Id}");

                if (currentNode.Priority == 0)
                    break;

                for (int i = 0; i < availableActions.Length; i++)
                {
                    Action currentAction = availableActions[i];
                    List<WorldState> desiredState = new List<WorldState>(currentNode.State);
                    bool shouldUse = false;

                    for (int k = desiredState.Count - 1; k >= 0; k--)
                    {
                        if(currentAction.MatchState(desiredState[k]))
                        {
                            desiredState.RemoveAt(k);
                            shouldUse = true;
                        }
                    }

                    if (!shouldUse)
                        continue;

                    desiredState.AddRange(currentAction.Preconditions);
                    
                    foreach (WorldState currentState in currentWorldState)
                    {
                        for (int k = desiredState.Count - 1; k >= 0; k--)
                        {
                            if (currentState.Id.Equals(desiredState[k].Id))
                            {
                                desiredState.RemoveAt(k);
                            }
                        }
                    }

                    StateNode child = new StateNode(currentAction.Id, desiredState.ToArray());

                    int newCost = currentNode.CostSoFar + 1;

                    child.CostSoFar = newCost;
                    child.Priority = desiredState.Count;
                    GameDebug.Log($"\tPriority {child.Priority} for {child.Id}");
                    frontier.Enqueue(child.Priority, child);

                }
            }

            return AssemblePlan(actionsToTake, availableActions);
        }

        private Queue<Action> AssemblePlan(Stack<StateNode> actionsToTake, Action[] actions)
        {
            Queue<Action> plan = new Queue<Action>();

            int actionsNum = actionsToTake.Count - 1;
            for (int i = 0; i < actionsNum; i++)
            {
                StateNode action = actionsToTake.Pop();
                plan.Enqueue(GetAction(action.Id, actions));
            }

            return plan;
        }

        private Action GetAction(string id, Action[] actions)
        {
            for(int i = 0; i < actions.Length; i++)
            {
                if (actions[i].Id.Equals(id))
                    return actions[i];
            }

            return null;
        }

        public Action[] GetAvailableActions(Action[] actions)
        {
            List<Action> availableActions = new List<Action>();

            for(int i = 0; i < actions.Length; i++)
            {
                if (actions[i].IsValid())
                    availableActions.Add(actions[i]);
            }

            return availableActions.ToArray();
        }

        public Goal[] GetValidGoals(Goal[] goals)
        {
            List<Goal> validGoals = new List<Goal>();

            for (int i = 0; i < goals.Length; i++)
            {
                if (goals[i].IsValid())
                    validGoals.Add(goals[i]);
            }

            return validGoals.ToArray();
        }
    }
}