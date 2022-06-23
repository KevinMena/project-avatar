using UnityEngine;
using UnityEngine.UI;

namespace AvatarBA.Debugging
{
    public class DisplayStats : MonoBehaviour
    {
        [SerializeField]
        private GameObject _player;

        [SerializeField]
        private string[] _statsIds = new string[9];

        private PlayerStatsController _statsController;

        private Text[] _statsText = new Text[9];

        private bool _isDirty = false;

        private void Awake() 
        {
            _statsController = _player.GetComponent<PlayerStatsController>();
        }

        private void Start() 
        {
            for(int i = 0; i < _statsIds.Length; i++)
            {
                Transform child = this.transform.GetChild(i);
                _statsText[i] = child.GetComponent<Text>();
            }

            _isDirty = true;
        }

        private void Update() 
        {
            if(_isDirty)
                UpdateStats();
        }

        private void UpdateStats()
        {
            for(int i = 0; i < _statsIds.Length; i++)
            {
                float value = _statsController.GetStatValue(_statsIds[i]);
                string displayName = _statsController.GetStatDisplayName(_statsIds[i]);
                _statsText[i].text = $"{displayName} = {value}";
            }

            _isDirty = false;
        }
    }
}
