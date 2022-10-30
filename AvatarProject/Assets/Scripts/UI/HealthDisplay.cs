using UnityEngine;

using AvatarBA.Managers;

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

        private void Awake() 
        {
            _displayMiddleware.OnHealthChange += UpdateHealthBar;
            _displayMiddleware.OnMaximumHealthChange += UpdateMaxHealth;
        }

        private void OnDestroy() 
        {
            _displayMiddleware.OnHealthChange -= UpdateHealthBar;
            _displayMiddleware.OnMaximumHealthChange -= UpdateMaxHealth;
        }

        public void UpdateMaxHealth(float value)
        {
            _healthBar.Maximum = (int) value;
        }

        public void UpdateHealthBar(float current)
        {
            _healthBar.ChangeCurrent(current);
        }
    }
}