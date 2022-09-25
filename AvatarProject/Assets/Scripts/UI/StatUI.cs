using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        private void Start()
        {
            _manager.Subscribe(this);
        }

        private void OnDestroy()
        {
            _manager.UnSubscribe(this);
        }

        public void UpdateMaxHealth(float value)
        {
            _health.Maximum = (int) value;
        }

        public void UpdateHealthBar(float current)
        {
            _health.ChangeCurrent(current);
        }
    }
}