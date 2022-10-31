using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using AvatarBA.Managers;

namespace AvatarBA.UI
{
    public class StatDisplay : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private StatMiddleware _displayMiddleware = default;

        [Header("Display")]
        [SerializeField]
        private GameObject _textPrefab;

        private Dictionary<string, TMP_Text> _runtimeStats = new Dictionary<string, TMP_Text>();

        private void Awake()
        {
            _displayMiddleware.Subscribe(this);
        }

        private void OnDisable()
        {
            _displayMiddleware.Unsubscribe(this);
        }

        public void Setup(KeyValuePair<string, StatRecord>[] currentStats)
        {
            for(int i = 0; i < currentStats.Length; i++)
            {
                GameObject statText = Instantiate(_textPrefab, transform);
                TMP_Text text = statText.GetComponent<TMP_Text>();
                text.text = $"{currentStats[i].Value.DisplayName} : {currentStats[i].Value.Value}";
                _runtimeStats.Add(currentStats[i].Key, text);
            }
        }

        public void UpdateStat(string id, StatRecord record)
        {
            _runtimeStats[id].text = $"{record.DisplayName} : {record.Value}";
        }
    }
}