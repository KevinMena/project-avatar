using System.Collections;
using UnityEngine;

namespace AvatarBA
{
    public abstract class Ability : ScriptableObject
    {

        public Sprite icon;

        public string abilityName = "New Ability";

        [TextArea]
        public string description = "Description";

        public float cooldown;

        public AbilityState state = AbilityState.ready;

        public abstract void Initialize();
        public abstract void Trigger();

        public IEnumerator CooldownCountdown()
        {
            yield return new WaitForSecondsRealtime(cooldown);
            state= AbilityState.ready;
        }
    }

    public enum AbilityState
    {
        ready,
        active,
        cooldown   
    }
}
