using UnityEngine;

using AvatarBA.UI;

namespace AvatarBA
{
    [CreateAssetMenu(fileName = "AbilityDisplayManager", menuName ="Managers/Ability Display Manager")]
    public class AbilityDisplayManager : ScriptableObject
    {
        private AbilityUI _currentInterface = null;

        public void SetInterface(AbilityUI ui)
        {
            if(_currentInterface != null)
                return;
            
            _currentInterface = ui;
        }

        public void UnsetInterface()
        {
            _currentInterface = null;
        }

        public void UpdateDisplay(int slot, float current)
        {
            _currentInterface?.UpdateDisplay(slot, current);
        }

        public void UpdateIcon(int slot, Sprite icon)
        {
            _currentInterface?.UpdateIcon(slot, icon);
        }
    }
}
