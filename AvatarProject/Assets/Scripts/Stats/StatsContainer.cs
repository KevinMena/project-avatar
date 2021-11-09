using UnityEngine;
using System.Collections.Generic;

namespace AvatarBA.Stats
{
    [CreateAssetMenu(fileName = "StatsContainer_New", menuName ="Stats/Container")]
    public class StatsContainer : ScriptableObject, IStatContainer
    {
        [SerializeField] private StatsCollection _collection;

        private Dictionary<string, StatType> _statsMap = new Dictionary<string, StatType>();

        private bool _isInitialized = false;

        public List<StatType> Stats => _collection.Stats;

        private void Initialize()
        {
            if(_isInitialized) return;

            foreach (StatType currentStat in _collection.Stats)
            {
                _statsMap.Add(currentStat.Id, currentStat);
            }

            _isInitialized = true;
        }

        public Dictionary<string, Stat> CreateRuntimeValues()
        {
            Dictionary<string, Stat> runtime = new Dictionary<string, Stat>();
            foreach (StatType currentStat in _collection.Stats)
            {
                runtime.Add(currentStat.Id, new Stat(currentStat.DefaultValue));
            }

            return runtime;
        }

        public StatType GetStat(string id)
        {
            if(_statsMap.TryGetValue(id, out var result))
                return result;

            return null;
        }

        public float GetStatValue(string id)
        {
            if(_statsMap.TryGetValue(id, out var result))
                return result.DefaultValue;

            return -1;
        }
    }
}
