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

        protected Dictionary<string, Stat> _currentStats;

        protected Dictionary<string, string> _displayNamesStats;

        protected virtual void Start()
        {
            CreateRuntimeValues();
        }

        public void CreateRuntimeValues()
        {
            _currentStats = new Dictionary<string, Stat>();
            _displayNamesStats = new Dictionary<string, string>();

            foreach (var stat in _characterData.Stats)
            {
                _currentStats.Add(stat.Type.Id, new Stat(stat.DefaultValue));
                _displayNamesStats.Add(stat.Type.Id, stat.Type.DisplayName);
            }
        }

        /// <summary>
        /// Returns the current runtime value for a stat
        /// </summary>
        /// <param name="id">Identifier for the stat</param>
        public float GetStatValue(string id)
        {
            if(_currentStats.TryGetValue(id, out var result))
                return result.Value;

            return -1;
        }

        public string GetStatDisplayName(string id)
        {
            if(_displayNamesStats.TryGetValue(id, out var result))
                return result;

            return "";
        }

        public void ApplyChangeToStat(string id, float value, StatModifierType modifierType)
        {
            if(_currentStats.TryGetValue(id, out var stat))
            {
                StatModifier modifier = new StatModifier(value, modifierType);
                stat.AddModifier(modifier);
            }
        }

        public void RemoveChangeToStat(string id, float value, StatModifierType modifierType)
        {
            if(_currentStats.TryGetValue(id, out var stat))
            {
                StatModifier modifier = new StatModifier(value, modifierType);
                stat.RemoveModifier(modifier);
            }
        }

        public void RemoveChangeToStatFromSource(string id, object source)
        {
            if(_currentStats.TryGetValue(id, out var stat))
            {
                stat.RemoveModifiersFromSource(source);
            }
        }
    }
}
