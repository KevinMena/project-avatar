using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AvatarBA.UI
{
    public class AbilityUI : MonoBehaviour
    {
        [SerializeField]
        private ProgressBar[] _slots;

        public void UpdateIcon(float current, int slotNumber)
        {
            _slots[slotNumber].ChangeCurrent(current);
        }
    }
}
