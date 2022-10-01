using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using AvatarBA.Managers;

namespace AvatarBA.UI
{ 
    public class StatUI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        StatsDisplayManager _manager;

        [Header("Display")]
        [SerializeField]
        private ProgressBar _health;

        [SerializeField]
        private Text[] _statText;

        private Dictionary<string, Text> _currentStats;

        private void Start()
        {
            _manager.Subscribe(this);
        }

        private void OnDestroy()
        {
            _manager.UnSubscribe(this);
        }

        public void Setup(KeyValuePair<string, StatRecord>[] runtimeStats)
        {
            _currentStats = new Dictionary<string, Text>();

            for(int i = 0; i < runtimeStats.Length; i++)
            {
                _statText[i].text = $"{runtimeStats[i].Value.DisplayName} : {runtimeStats[i].Value.Stat.Value}";
                _currentStats[runtimeStats[i].Key] = _statText[i];
            }
        }

        public void UpdateMaxHealth(float value)
        {
            _health.Maximum = (int) value;
        }

        public void UpdateHealthBar(float current)
        {
            _health.ChangeCurrent(current);
        }

        public void UpdateStat(string id, StatRecord record)
        {
            _currentStats[id].text = $"{record.DisplayName} : {record.Stat.Value}";
        }
    }
}