using System;
using System.Collections.Generic;
using UnityEngine;

namespace AvatarBA.AI.GOAP
{
    public abstract class Goal : ScriptableObject
    {
        [SerializeField]
        private string _id;

        [SerializeField]
        private WorldContext _desiredContext = new WorldContext();

        public string Id => _id;

        public ref readonly WorldContext DesiredContext => ref _desiredContext;

        public abstract bool IsValid(GameObject agent);

        /// <summary>
        /// Calculates the priority of the current goal, REMEMBER LESS IS MORE PRIORITY
        /// </summary>
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
    }
}