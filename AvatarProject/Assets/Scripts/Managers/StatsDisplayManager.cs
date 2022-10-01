
using UnityEngine;

using AvatarBA.UI;
using System.Collections.Generic;

namespace AvatarBA.Managers
{
    [CreateAssetMenu(fileName = "StatsDisplayManager", menuName = "Managers/Stats Display Manager")]
    public class StatsDisplayManager : ScriptableObject
    {
        private StatUI _currentUI = null;

        public void Subscribe(StatUI current)
        {
            if (_currentUI != null)
                return;

            _currentUI = current;
        }

        public void UnSubscribe(StatUI current)
        {
            if (current == _currentUI)
                _currentUI = null;
        }

        public void Setup(KeyValuePair<string, StatRecord>[] runtimeStats)
        {
            if (_currentUI == null)
                return;

            _currentUI.Setup(runtimeStats);
        }

        public void UpdateMaxHealth(float value)
        {
            _currentUI?.UpdateMaxHealth(value);
        }

        public void UpdateHealthBar(float current)
        {
            _currentUI?.UpdateHealthBar(current);
        }

        public void UpdateStat(string id, StatRecord record)
        {
            _currentUI.UpdateStat(id, record);
        }
    }
}