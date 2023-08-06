using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AvatarBA.AI.GOAP
{
    public abstract class Action : ScriptableObject
    {
        [SerializeField]
        private string _id;

        [SerializeField]
        private WorldContext _preConditions = new WorldContext();

        [SerializeField]
        private WorldContext _effects = new WorldContext();

        public string Id => _id;
        public ref readonly WorldContext PreConditions => ref _preConditions;
        public ref readonly WorldContext Effects => ref _effects;

        public abstract bool IsValid();

        public abstract IEnumerator Perform(GameObject agent);

        public bool MatchState(WorldState state)
        {
            return Effects.Contains(new KeyValuePair<string, bool>(state.Id, state.Value));
        }
    }
}
