using System.Collections.Generic;
using UnityEngine;
using AvatarBA.Stats;

namespace AvatarBA
{    
    public class PlayerStatsController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CharacterData _characterData;

        private Dictionary<string, Stat> _currentStats;

        void Start()
        {
            _currentStats = _characterData.CreateRuntimeValues();
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
    }
}

