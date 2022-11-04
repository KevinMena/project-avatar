using UnityEngine;

using AvatarBA.Managers;
using TMPro;

namespace AvatarBA.UI
{ 
    public class HealthDisplay : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private HealthMiddleware _displayMiddleware = default;

        [Header("Display")]
        [SerializeField]
        private ProgressBar _healthBar;

        [SerializeField]
        private TMP_Text _healthText;

        private void Awake() 
        {
            _displayMiddleware.OnHealthChange += UpdateHealthBar;
            _displayMiddleware.OnMaximumHealthChange += UpdateMaxHealth;
        }

        private void OnDisable() 
        {
            _displayMiddleware.OnHealthChange -= UpdateHealthBar;
            _displayMiddleware.OnMaximumHealthChange -= UpdateMaxHealth;
        }

        public void UpdateMaxHealth(float value)
        {
            _healthBar.Maximum = (int) value;
            UpdateText();
        }

        public void UpdateHealthBar(float current)
        {
            _healthBar.ChangeCurrent(current);
            UpdateText();
        }

        private void UpdateText()
        {
            _healthText.text = $"{_healthBar.Current}/{_healthBar.Maximum}";
        }
    }
}