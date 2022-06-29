using System.Collections.Generic;

namespace AvatarBA.Patterns
{
    public abstract class State
    {
        protected List<Transition> transitions = new List<Transition>();

        public abstract void OnEnter();

        public abstract void OnUpdate();

        public abstract void OnExit();

        public void AddTransition(Transition t)
        {
            transitions.Add(t);
        }

        public Transition CheckTransitions()
        {
            foreach (Transition t in transitions)
            {
                if(t.IsTriggered())
                    return t;
            }

            return null;
        }
    }
}