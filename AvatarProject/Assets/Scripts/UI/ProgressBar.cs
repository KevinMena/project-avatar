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

        public int Minimum { get { return _minimum; } set { _minimum = value; } }
        public int Maximum { get { return _maximum; } set { _maximum = value; } }
        public int Current => Mathf.FloorToInt(_current);

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
    }
}
