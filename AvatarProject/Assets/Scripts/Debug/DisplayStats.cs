using AvatarBA.Stats;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AvatarBA.Debugging
{
    public class DisplayStats : MonoBehaviour
    {
        [SerializeField]
        private GameObject _player;

        [SerializeField]
        private Text[] _stats;

        private PlayerStatsController _statsController;

        private bool _isDirty = false;

        private void Awake() 
        {
            _statsController = _player.GetComponent<PlayerStatsController>();
        }

        private void Start() 
        {
            _isDirty = true;
        }

        private void Update() 
        {
            if(_isDirty)
                UpdateStats();
        }

        private void UpdateStats()
        {
            //KeyValuePair<string, float>[] runtimeStats = _statsController.GetAllStats();
            //for(int i = 0; i < _stats.Length; i++)
            //{
            //    _stats[i].text = $"{runtimeStats[i].Key} = {runtimeStats[i].Value}";
            //}

            //_isDirty = false;
        }
    }
}
