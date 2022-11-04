using System;
using System.Collections.Generic;
using UnityEngine;

namespace AvatarBA.AI.Core
{
    public abstract class Goal : ScriptableObject
    {
        [SerializeField]
        private string _id;

        [SerializeField]
        private WorldStateSerializable[] _goalState;

        private WorldContext _desiredContext = new WorldContext();

        public string Id => _id;

        public ref readonly WorldContext DesiredContext => ref _desiredContext;

        public abstract bool IsValid();

        public abstract int CalculatePriority();

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            return obj is Goal goal &&
                   base.Equals(obj) &&
                   Id == goal.Id;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), Id);
        }

        public static bool operator ==(Goal lhs, Goal rhs)
        {
            return lhs.Id == rhs.Id;
        }

        public static bool operator !=(Goal lhs, Goal rhs)
        {
            return lhs.Id != rhs.Id;
        }

        // This is just for inserting information early, has to be change to look in the database
        #region TESTING
        public void SetupWorld()
        {
            for (int i = 0; i < _goalState.Length; i++)
            {
                _desiredContext.Add(new WorldState(_goalState[i].Id, _goalState[i].Value));
            }
        }

        public void CleanWorld()
        {
            _desiredContext.Clear();
        }
        #endregion
    }
}