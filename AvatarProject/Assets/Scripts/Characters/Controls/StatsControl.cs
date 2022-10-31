using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using AvatarBA.Stats;
using AvatarBA.Managers;

namespace AvatarBA
{
    public struct StatRecord
    {
        public float Value;
        public string DisplayName;

        public StatRecord(string name, float value)
        {
            DisplayName = name;
            Value = value;
        }
    }

    public class StatsControl : MonoBehaviour, IStatContainer
    {
        [SerializeField]
        private StatMiddleware _displayMiddleware = default;
        private Core _core;

        private Dictionary<string, Stat> _runtimeStats;

        private void Awake()
        {
            _core = GetComponent<Core>();
        }

        private void Start()
        {
            CreateRuntimeValues();
        }

        public void CreateRuntimeValues()
        {
            _runtimeStats = new Dictionary<string, Stat>();
            List<KeyValuePair<string, StatRecord>> records = new List<KeyValuePair<string, StatRecord>>();

            foreach (StatBase originalStat in _core.Data.Stats)
            {
                _runtimeStats[originalStat.Type.Id] = new Stat(originalStat.Type.DisplayName, originalStat.DefaultValue);
                records.Add(new KeyValuePair<string, StatRecord>(originalStat.Type.Id, new StatRecord(originalStat.Type.DisplayName, originalStat.DefaultValue)));
            }

            _displayMiddleware.SetupDisplay(records.ToArray());
        }

        public KeyValuePair<string, float>[] GetAllStatsValues()
        {
            List<KeyValuePair<string, float>> stats = new List<KeyValuePair<string, float>>();

            foreach (var stat in _runtimeStats)
            {
                stats.Add(new KeyValuePair<string, float>(stat.Key, stat.Value.Value));
            }

            return stats.ToArray();
        }

        public KeyValuePair<string, string>[] GetAllStatsDisplayName()
        {
            List<KeyValuePair<string, string>> stats = new List<KeyValuePair<string, string>>();

            foreach (var stat in _runtimeStats)
            {
                stats.Add(new KeyValuePair<string, string>(stat.Key, stat.Value.Name));
            }

            return stats.ToArray();
        }

        public float GetStat(string id)
        {
            if (_runtimeStats.TryGetValue(id, out var stat))
                return stat.Value;
            return -1;
        }

        public string GetStatDisplayName(string id)
        {
            if (_runtimeStats.TryGetValue(id, out var stat))
                return stat.Name;
            return "";
        }

        public void ApplyChangeToStat(string statId, string modifierId, float value, StatModifierType modifierType)
        {
            if (_runtimeStats.TryGetValue(statId, out var stat))
            {
                StatModifier modifier = new StatModifier(modifierId, value, modifierType);
                stat.AddModifier(modifier);
            }
        }

        public void ApplyChangeToStat(string statId, string modifierId, float value, StatModifierType modifierType, object owner)
        {
            if (_runtimeStats.TryGetValue(statId, out var stat))
            {
                StatModifier modifier = new StatModifier(modifierId, value, modifierType, owner);
                stat.AddModifier(modifier);
            }
        }

        public void RemoveChangeToStat(string statId, string modifierId)
        {
            if (_runtimeStats.TryGetValue(statId, out var stat))
                stat.RemoveModifier(modifierId);
        }

        public void RemoveChangeToStat(string statId, StatModifier modifier)
        {
            if (_runtimeStats.TryGetValue(statId, out var stat))
                stat.RemoveModifier(modifier);
        }

        public void RemoveChangeToStatFromSource(string id, object source)
        {
            if (_runtimeStats.TryGetValue(id, out var stat))
                stat.RemoveModifiersFromSource(source);
        }
    }
}
