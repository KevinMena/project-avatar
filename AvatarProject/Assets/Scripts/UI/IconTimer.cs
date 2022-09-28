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
        private TMP_Text _timer;

        private float _current;

        private const float MINIMUM = 0;
        private const float MAXIMUM = 1;

        public void StartTimer(float maxTimer)
        {
            _timer.SetText(maxTimer.ToString("F1"));
            _cooldown.fillAmount = 1;
            _cooldown.gameObject.SetActive(true);
            _timer.gameObject.SetActive(true);
        }

        public void EndTimer()
        {
            _cooldown.gameObject.SetActive(false);
            _timer.gameObject.SetActive(false);
            _cooldown.fillAmount = 1;
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
            _cooldown.fillAmount = fillAmount;
        }

        public void ChangeIcon(Sprite icon)
        {
            _icon.sprite = icon;
        }
    }
}