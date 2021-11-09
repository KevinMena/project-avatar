using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AvatarBA.Stats;

namespace AvatarBA
{
    [CreateAssetMenu(fileName = "Characters_New", menuName ="Characters/Character")]
    public class CharacterData : ScriptableObject, IStatContainer
    {
        [SerializeField] private int _id = 0;

        [SerializeField] private string _characterName = "New Character";

        [TextArea]
        [SerializeField] private string _description = "Description";

        [SerializeField] private Sprite _icon = null;

        [SerializeField] private GameObject _prefab = null;

        [SerializeField] private List<StatType> _stats = new List<StatType>();

        private Dictionary<string, StatType> _statsCollection = new Dictionary<string, StatType>();

        private bool _isInitialized = false;

        public int Id => _id;

        public string Name => _characterName;

        public string Description => _description;

        public Sprite Icon => _icon;

        public GameObject Prefab => _prefab;

        public List<StatType> Stats => _stats;

        /// <summary>
        /// Initialize the map for the current stats
        /// </summary>
        [ContextMenu("Initialize")]
        private void Initialize()
        {
            if(_isInitialized) return;

            foreach (StatType currentStat in _stats)
            {
                _statsCollection.Add(currentStat.Id, currentStat);
            }

            if(_stats.Count < 1) return;

            _isInitialized = true;
        }

        public Dictionary<string, Stat> CreateRuntimeValues()
        {
            Dictionary<string, Stat> runtime = new Dictionary<string, Stat>();
            foreach (StatType currentStat in _stats)
            {
                runtime.Add(currentStat.Id, new Stat(currentStat.DefaultValue));
            }

            return runtime;
        }

        public StatType GetStat(string id)
        {
            if(_statsCollection.TryGetValue(id, out var result))
                return result;

            return null;
        }

        public float GetStatValue(string id)
        {
            if(_statsCollection.TryGetValue(id, out var result))
                return result.DefaultValue;

            return -1;
        }
    }
}
