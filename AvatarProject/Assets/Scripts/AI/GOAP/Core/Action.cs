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
        private WorldStateSerializable[] _goalConditions;

        [SerializeField]
        private WorldStateSerializable[] _afterEffects;

        private WorldContext _preConditions = new WorldContext();

        private WorldContext _effects = new WorldContext();

        public string Id => _id;
        public ref readonly WorldContext PreConditions => ref _preConditions;
        public ref readonly WorldContext Effects => ref _effects;

        public abstract bool IsValid();

        public abstract IEnumerator Perform(GameObject agent);

        public bool MatchState(WorldState state)
        {
            return Effects.Contains(state);
        }

        // This is just for inserting information early, has to be change to look in the database
        #region TESTING
        public void SetupWorld()
        {
            for (int i = 0; i < _goalConditions.Length; i++)
            {
                _preConditions.Add(new WorldState(_goalConditions[i].Id, _goalConditions[i].Value));
            }

            for (int i = 0; i < _afterEffects.Length; i++)
            {
                _effects.Add(new WorldState(_afterEffects[i].Id, _afterEffects[i].Value));
            }
        }

        public void CleanWorld()
        {
            _preConditions.Clear();
            _effects.Clear();
        }
        #endregion
    }
}
