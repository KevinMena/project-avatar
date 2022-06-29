using System.Collections.Generic;

namespace AvatarBA.Patterns
{
    public class Transition
    {
        private State _targetState;
        private Dictionary<string, Condition> _conditions;
        private bool _needsAll;

        public State TargetState => _targetState;

        public Transition(State state, bool needs)
        {
            _targetState = state;
            _needsAll = needs;
            _conditions = new Dictionary<string, Condition>();
        }

        public void AddCondition(string name, Condition cond)
        {
            _conditions[name] = cond;
            UnityEngine.Debug.Log($"Added {name}");
        }

        public Condition GetCondition(string name)
        {
            return _conditions[name];
        }

        public bool IsTriggered()
        {
            foreach (KeyValuePair<string, Condition> slot in _conditions)
            {
                if(!slot.Value.Test() && _needsAll)
                    return false;
            }

            return true;
        }
    }
}