using UnityEngine;

using AvatarBA.UI;
using AvatarBA.Debugging;

namespace AvatarBA.Managers
{
    [CreateAssetMenu(fileName = "Middleware_AbilitiesDisplay", menuName = "Middlewares/Abilities Display")]
    public class AbilityMiddleware : ScriptableObject
    {
        private AbilityDisplay _display = null;

        public void Subscribe(AbilityDisplay display)
        {
            if (_display != null)
            {
                GameDebug.LogWarning("Abilities display already exists, something is trying to subscribe.");
                return;
            }

            _display = display;
        }

        public void Unsubscribe(AbilityDisplay display)
        {
            if (_display == null || _display != display)
            {
                GameDebug.LogWarning("Abilities display not set or something is trying to unsubscribe");
                return;
            }

            _display = null;
        }

        public void StartCooldownTimer(int slot, float maxTimer)
        {
            _display?.StartCooldownTimer(slot, maxTimer);
        }

        public void StartActiveTimer(int slot, float maxTimer)
        {
            _display.StartActiveTimer(slot, maxTimer);
        }

        public void EndTimer(int slot)
        {
            _display?.EndTimer(slot);
        }

        public void UpdateDisplay(int slot, float current, float timer)
        {
            _display?.UpdateDisplay(slot, current, timer);
        }

        public void UpdateIcon(int slot, Sprite icon)
        {
            _display?.UpdateIcon(slot, icon);
        }
    }
}