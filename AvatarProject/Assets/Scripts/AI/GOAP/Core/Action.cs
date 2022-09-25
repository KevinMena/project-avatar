using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AvatarBA.AI.Core
{
    public abstract class Action : ScriptableObject
    {
        [SerializeField]
        private string _id;

        [SerializeField]
        private WorldState[] _preconditions;

        [SerializeField]
        private WorldState[] _effects;

        public string Id => _id;

        public WorldState[] Preconditions => _preconditions;

        public WorldState[] Effects => _effects;

        public abstract bool IsValid();

        public abstract IEnumerator Perform(GameObject agent);

        public bool MatchState(WorldState state)
        {
            for(int i = 0; i < Effects.Length; i++)
            {
                if (state.Id == Effects[i].Id)
                    return true;
            }

            return false;
        }
    }
}
