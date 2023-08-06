using System;
using System.Collections.Generic;
using System.Linq;
using AvatarBA.Common.DataStructures;
using AvatarBA.Debugging;
using UnityEngine;

namespace AvatarBA.AI.GOAP
{
    public struct WorldState
    {
        public string Id { get; private set; }
        public bool Value { get; private set; }

        public WorldState(string key, bool value)
        {
            Id = key;
            Value = value;
        }
    }

    [Serializable]
    public class WorldContext : GenericDictionary<string, bool>
    {
        public WorldContext() : base() { }
    }

    public struct StateNode
    {
        public string Id;
        public WorldContext State;

        public int CostSoFar;
        public int Priority;

        public StateNode(string id, WorldContext state)
        {
            Id = id;
            State = state;
            CostSoFar = 0;
            Priority = state.Count;
        }
    }

    public class Planner
    {
        public Queue<Action> GetPlan(Goal goal, Action[] availableActions, WorldContext currentWorldContext)
        {
            // Create the root node of the tree of actions, initiating from the Goal
            WorldContext desiredContext = new WorldContext();
            goal.DesiredContext.CopyTo(desiredContext);
            StateNode root = new StateNode(goal.Id, desiredContext);

            // Get the series of actions to take to satisfie the goal
            Queue<Action> bestPlan = BuildPlan(root, availableActions, currentWorldContext);

            return bestPlan;
        }

        private Queue<Action> BuildPlan(StateNode root, Action[] availableActions, WorldContext currentWorldContext)
        {
            // Initialize the data to order the actions
            PriorityQueue<int, StateNode> frontier = new PriorityQueue<int, StateNode>();
            Stack<StateNode> actionsToTake = new Stack<StateNode>();
            frontier.Enqueue(root.Priority, root);

            // If we have still actions to check, keep looking
            while (frontier.Count > 0)
            {
                // Get the most preferable action (the one with less WorldStates to satisfie)
                StateNode currentNode = frontier.Dequeue();
                actionsToTake.Push(currentNode);

                // GameDebug.Log($"Current node {currentNode.Id}");

                // If the WorldState to satisfie is 0, then we have a plan
                if (currentNode.Priority == 0)
                    break;

                // Check all the available actions in this moment to see if is part of the plan
                for (int i = 0; i < availableActions.Length; i++)
                {
                    Action currentAction = availableActions[i];
                    WorldContext desiredContext = new WorldContext();
                    currentNode.State.CopyTo(desiredContext);
                    bool shouldUse = false;

                    // Check if any of the effects of this action satisfies the desired state
                    // If we do we take into account this action, if not discard it
                    foreach(var context in desiredContext.ToList())
                    {
                        if(currentAction.MatchState(new WorldState(context.Key, context.Value)))
                        {
                            desiredContext.Remove(context.Key);
                            shouldUse = true;
                        }
                    }

                    if (!shouldUse)
                        continue;

                    // Add the preconditions of this action to the desire state
                    foreach(var newState in currentAction.PreConditions)
                    {
                        desiredContext.Add(newState);
                    }

                    // Evaluate if any WorldState can be satisfied with the world data
                    foreach (var worldState in currentWorldContext)
                    {
                        foreach (var context in desiredContext.ToList())
                        {
                            if (context.Key == worldState.Key)
                            {
                                desiredContext.Remove(context.Key);
                            }
                        }
                    }

                    // Create the new child of the tree calculating the new priorities
                    StateNode child = new StateNode(currentAction.Id, desiredContext);

                    int newCost = currentNode.CostSoFar + 1;

                    child.CostSoFar = newCost;
                    child.Priority = desiredContext.Count;
                    // GameDebug.Log($"\tPriority {child.Priority} for {child.Id}");
                    frontier.Enqueue(child.Priority, child);

                }
            }

            return AssemblePlan(actionsToTake, availableActions);
        }

        private Queue<Action> AssemblePlan(Stack<StateNode> actionsToTake, Action[] actions)
        {
            Queue<Action> plan = new Queue<Action>();
            
            // Check if the first action to take has a priority of 0, meaning the plan is complete
            // If not then we need to return an empty plan, because goal not satisfied
            if(actionsToTake.Peek().Priority == 0)
            {
                int actionsNum = actionsToTake.Count - 1;
                for (int i = 0; i < actionsNum; i++)
                {
                    StateNode action = actionsToTake.Pop();
                    plan.Enqueue(GetAction(action.Id, actions));
                }
            }

            return plan;
        }

        private Action GetAction(string id, Action[] actions)
        {
            for (int i = 0; i < actions.Length; i++)
            {
                if (actions[i].Id.Equals(id))
                    return actions[i];
            }

            return null;
        }

        public Action[] GetAvailableActions(Action[] actions)
        {
            List<Action> availableActions = new List<Action>();

            for (int i = 0; i < actions.Length; i++)
            {
                if (actions[i].IsValid())
                    availableActions.Add(actions[i]);
            }

            return availableActions.ToArray();
        }

        public Goal[] GetValidGoals(GameObject agent, Goal[] goals)
        {
            List<Goal> validGoals = new List<Goal>();

            for (int i = 0; i < goals.Length; i++)
            {
                if (goals[i].IsValid(agent))
                    validGoals.Add(goals[i]);
            }

            return validGoals.ToArray();
        }
    }
}