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

        private void UnsetInterface()
        {
            _currentInterface = null;
        }

        public void UpdateIcon(float current, int slot)
        {
            _currentInterface?.UpdateIcon(current, slot);
        }
    }
}
