using UnityEngine;

using AvatarBA.UI;

namespace AvatarBA.Managers
{
    [CreateAssetMenu(fileName = "AbilityDisplayManager", menuName ="Managers/Ability Display Manager")]
    public class AbilityDisplayManager : ScriptableObject
    {
        private AbilityUI _currentUI = null;

        public void Subscribe(AbilityUI current)
        {
            if (_currentUI != null)
                return;

            _currentUI = current;
        }

        public void UnSubscribe(AbilityUI current)
        {
            if(current == _currentUI)
                _currentUI = null;
        }

        public void UpdateDisplay(int slot, float current)
        {
            _currentUI?.UpdateDisplay(slot, current);
        }

        public void UpdateIcon(int slot, Sprite icon)
        {
            _currentUI?.UpdateIcon(slot, icon);
        }
    }
}
