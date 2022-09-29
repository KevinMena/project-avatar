using System.Collections.Generic;

using UnityEngine;

using AvatarBA.Stats;

namespace AvatarBA
{
    public class CharacterStatsController : MonoBehaviour, IStatContainer
    {
        [Header("References")]
        [SerializeField]
        protected CharacterData _characterData;

        protected Dictionary<string, KeyValuePair<string, Stat>> _runtimeStats;

        protected virtual void Start()
        {
            CreateRuntimeValues();
        }

        public void CreateRuntimeValues()
        {
            _runtimeStats = new Dictionary<string, KeyValuePair<string, Stat>>();

            foreach (StatBase originalStat in _characterData.Stats)
            {
                _runtimeStats[originalStat.Type.Id] = new KeyValuePair<string, Stat>(originalStat.Type.DisplayName, new Stat(originalStat.DefaultValue));
            }
        }

        public KeyValuePair<string, float>[] GetAllStats()
        {
            List<KeyValuePair<string, float>> stats = new List<KeyValuePair<string, float>>();

            foreach(var stat in _runtimeStats.Values)
            {
                stats.Add(new KeyValuePair<string, float>(stat.Key, stat.Value.Value));
            }

            return stats.ToArray();
        }

        public float GetStat(string id)
        {
            if (_runtimeStats.TryGetValue(id, out var stat))
                return stat.Value.Value;
            return -1;
        }

        public string GetStatDisplayName(string id)
        {
            if (_runtimeStats.TryGetValue(id, out var stat))
                return stat.Key;
            return "";
        }

        public void ApplyChangeToStat(string id, float value, StatModifierType modifierType)
        {
            if (_runtimeStats.TryGetValue(id, out var stat))
            {
                StatModifier modifier = new StatModifier(value, modifierType);
                stat.Value.AddModifier(modifier);
            }
        }

        public void RemoveChangeToStat(string id, StatModifier modifier)
        {
            if (_runtimeStats.TryGetValue(id, out var stat))
                stat.Value.RemoveModifier(modifier);
        }

        public void RemoveChangeToStatFromSource(string id, object source)
        {
            if (_runtimeStats.TryGetValue(id, out var stat))
                stat.Value.RemoveModifiersFromSource(source);
        }
    }
}
