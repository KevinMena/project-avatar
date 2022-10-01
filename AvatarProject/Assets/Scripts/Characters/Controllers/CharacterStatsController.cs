using System.Collections.Generic;

using UnityEngine;

using AvatarBA.Stats;
using static UnityEngine.Rendering.DebugUI;

namespace AvatarBA
{
    public struct StatRecord
    {
        public string DisplayName;
        public Stat Stat;

        public StatRecord(string name, Stat currentStat)
        {
            DisplayName = name;
            Stat = currentStat;
        }
    }

    public class CharacterStatsController : MonoBehaviour, IStatContainer
    {
        [Header("References")]
        [SerializeField]
        protected CharacterData _characterData;

        protected Dictionary<string, StatRecord> _runtimeStats;

        protected virtual void Start()
        {
            CreateRuntimeValues();
        }

        public virtual void CreateRuntimeValues()
        {
            _runtimeStats = new Dictionary<string, StatRecord>();

            foreach (StatBase originalStat in _characterData.Stats)
            {
                _runtimeStats[originalStat.Type.Id] = new StatRecord(originalStat.Type.DisplayName, new Stat(originalStat.DefaultValue));
            }
        }

        public virtual KeyValuePair<string, float>[] GetAllStats()
        {
            List<KeyValuePair<string, float>> stats = new List<KeyValuePair<string, float>>();

            foreach(var stat in _runtimeStats)
            {
                stats.Add(new KeyValuePair<string, float>(stat.Key, stat.Value.Stat.Value));
            }

            return stats.ToArray();
        }

        public virtual KeyValuePair<string, StatRecord>[] GetAllStatsDisplayName()
        {
            List<KeyValuePair<string, StatRecord>> stats = new List<KeyValuePair<string, StatRecord>>();

            foreach (var stat in _runtimeStats)
            {
                stats.Add(new KeyValuePair<string, StatRecord>(stat.Key, stat.Value));
            }

            return stats.ToArray();
        }

        public virtual float GetStat(string id)
        {
            if (_runtimeStats.TryGetValue(id, out var record))
                return record.Stat.Value;
            return -1;
        }

        public virtual string GetStatDisplayName(string id)
        {
            if (_runtimeStats.TryGetValue(id, out var record))
                return record.DisplayName;
            return "";
        }

        public virtual void ApplyChangeToStat(string statId, string modifierId, float value, StatModifierType modifierType)
        {
            if (_runtimeStats.TryGetValue(statId, out var record))
            {
                StatModifier modifier = new StatModifier(modifierId, value, modifierType);
                record.Stat.AddModifier(modifier);
            }
        }

        public void ApplyChangeToStat(string statId, string modifierId, float value, StatModifierType modifierType, object owner)
        {
            if (_runtimeStats.TryGetValue(statId, out var record))
            {
                StatModifier modifier = new StatModifier(modifierId, value, modifierType, owner);
                record.Stat.AddModifier(modifier);
            }
        }

        public virtual void RemoveChangeToStat(string statId, string modifierId)
        {
            if (_runtimeStats.TryGetValue(statId, out var record))
                record.Stat.RemoveModifier(modifierId);
        }

        public virtual void RemoveChangeToStat(string statId, StatModifier modifier)
        {
            if (_runtimeStats.TryGetValue(statId, out var record))
                record.Stat.RemoveModifier(modifier);
        }

        public virtual void RemoveChangeToStatFromSource(string id, object source)
        {
            if (_runtimeStats.TryGetValue(id, out var record))
                record.Stat.RemoveModifiersFromSource(source);
        }
    }
}
