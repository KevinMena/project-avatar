using UnityEngine;

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
            _manager.SetInterface(this);    
        }

        public void UpdateIcon(float current, int slotNumber)
        {
            _slots[slotNumber].ChangeCurrent(current);
        }
    }
}
