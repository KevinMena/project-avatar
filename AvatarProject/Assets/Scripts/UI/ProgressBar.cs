using UnityEngine;
using UnityEngine.UI;

namespace AvatarBA.UI
{
    public class ProgressBar : MonoBehaviour
    {
        [SerializeField]
        private Image _bar;

        [SerializeField]
        private int _minimum = 0;

        [SerializeField]
        private int _maximum = 1;

        private float _current;

        public void ChangeCurrent(float amount)
        {
            _current = amount;
            GetCurrentFill();
        }

        private void GetCurrentFill()
        {
            float currentOffset = _current - _minimum;
            float maximumOffset = _maximum - _minimum;
            float fillAmount = currentOffset / maximumOffset;
            _bar.fillAmount = fillAmount;
        }
        
        public void ChangeIcon(Sprite icon)
        {
            _bar.sprite = icon;
        }
    }
}
