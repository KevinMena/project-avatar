using UnityEngine;
using UnityEngine.UI;

namespace AvatarBA.Debugging
{
    public class DisplayStats : MonoBehaviour
    {
        [SerializeField]
        private GameObject _player;
        
        [SerializeField]
        private Text _healthText;
        [SerializeField]
        private Text _attackPowerText;
        [SerializeField]
        private Text _attackSpeedText;
        [SerializeField]
        private Text _defenseText;
        [SerializeField]
        private Text _movementSpeedText;
        [SerializeField]
        private Text _spiritPowerText;

        private PlayerStatsController _statsController;

        private string[] _statsIds = new string[6] 
                                    { 
                                        "Health",
                                        "Attack Power", 
                                        "Attack Speed",
                                        "Defense",
                                        "Movement Speed",
                                        "Spirit Power"
                                    };

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
            float value = _statsController.Health;
            _healthText.text = $"{_statsIds[0]} = {value}";

            value = _statsController.AttackPower;
            _attackPowerText.text = $"{_statsIds[1]} = {value}";

            value = _statsController.AttackSpeed;
            _attackSpeedText.text = $"{_statsIds[2]} = {value}";

            value = _statsController.Defense;
            _defenseText.text = $"{_statsIds[3]} = {value}";

            value = _statsController.MovementSpeed;
            _movementSpeedText.text = $"{_statsIds[4]} = {value}";

            value = _statsController.SpiritPower;
            _spiritPowerText.text = $"{_statsIds[5]} = {value}";

            _isDirty = false;
        }
    }
}
