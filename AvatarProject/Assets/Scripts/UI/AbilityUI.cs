using UnityEngine;

using AvatarBA.Managers;

namespace AvatarBA.UI
{
    public class AbilityUI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        AbilityDisplayManager _manager;

        [Header("Display")]
        [SerializeField]
        private IconTimer[] _slots;

        private void Start() 
        {
            _manager.Subscribe(this);    
        }

        private void OnDestroy()
        {
            _manager.UnSubscribe(this);
        }

        public void StartTimer(int slot, float maxTimer)
        {
            _slots[slot].StartTimer(maxTimer);
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
