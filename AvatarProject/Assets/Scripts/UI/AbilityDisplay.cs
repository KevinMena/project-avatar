using UnityEngine;

using AvatarBA.Managers;

namespace AvatarBA.UI
{
    public class AbilityDisplay : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private AbilityMiddleware _displayMiddleware;

        [Header("Display")]
        [SerializeField]
        private IconTimer[] _slots;

        private void Awake()
        {
            _displayMiddleware.Subscribe(this);
        }

        private void OnDestroy()
        {
            _displayMiddleware.Unsubscribe(this);
        }

        public void StartCooldownTimer(int slot, float maxTimer)
        {
            _slots[slot].StartCooldownTimer(maxTimer);
        }

        public void StartActiveTimer(int slot, float maxTimer)
        {
            _slots[slot].StartActiveTimer(maxTimer);
        }

        public void EndTimer(int slot)
        {
            _slots[slot].EndTimer();
        }

        public void UpdateDisplay(int slot, float current, float timer)
        {
            _slots[slot].ChangeCurrent(current, timer);
        }

        public void UpdateIcon(int slot, Sprite icon)
        {
            _slots[slot].ChangeIcon(icon);
        }
    }
}