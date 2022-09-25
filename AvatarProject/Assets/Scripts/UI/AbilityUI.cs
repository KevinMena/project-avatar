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
        private ProgressBar[] _slots;

        private void Start() 
        {
            _manager.Subscribe(this);    
        }

        private void OnDestroy()
        {
            _manager.UnSubscribe(this);
        }

        public void UpdateDisplay(int slotNumber, float current)
        {
            _slots[slotNumber].ChangeCurrent(current);
        }

        public void UpdateIcon(int slotNumber, Sprite icon)
        {
            _slots[(slotNumber)].ChangeIcon(icon);
        }
    }
}
