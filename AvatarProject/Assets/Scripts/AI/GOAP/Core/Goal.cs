using System;
using UnityEngine;

namespace AvatarBA.AI.Core
{
    public abstract class Goal : ScriptableObject
    {
        [SerializeField]
        private string _id;

        [SerializeField]
        private WorldState[] _desiredState;

        public string Id => _id;

        public WorldState[] DesiredState => _desiredState;

        public abstract bool IsValid();

        public abstract int CalculatePriority();

        public override bool Equals(object obj)
        {
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
    }
}