using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace AvatarBA.UI
{
    public class IconTimer : MonoBehaviour
    {
        [SerializeField]
        private Image _icon;

        [SerializeField]
        private Image _cooldown;

        [SerializeField]
        private Image _active;

        [SerializeField]
        private TMP_Text _timer;

        private float _current;
        private Image _currentEffect;

        private const float MINIMUM = 0;
        private const float MAXIMUM = 1;

        public void StartTimer(float maxTimer)
        {
            _timer.SetText(maxTimer.ToString("F1"));
            _currentEffect.fillAmount = 1;
            _currentEffect.gameObject.SetActive(true);
            _timer.gameObject.SetActive(true);
        }

        public void StartActiveTimer(float maxTimer)
        {
            _currentEffect = _active;
            StartTimer(maxTimer);
        }

        public void StartCooldownTimer(float maxTimer)
        {
            _currentEffect = _cooldown;
            StartTimer(maxTimer);
        }

        public void EndTimer()
        {
            _currentEffect.gameObject.SetActive(false);
            _currentEffect.fillAmount = 1;
            _timer.SetText("");
        }

        public void ChangeCurrent(float amount, float timer)
        {
            _current = amount;
            _timer.SetText(timer.ToString("F1"));
            GetCurrentFill();
        }

        private void GetCurrentFill()
        {
            float currentOffset = MAXIMUM - _current;
            float maximumOffset = MAXIMUM - MINIMUM;
            float fillAmount = currentOffset / maximumOffset;
            _currentEffect.fillAmount = fillAmount;
        }

        public void ChangeIcon(Sprite icon)
        {
            _icon.sprite = icon;
        }
    }
}