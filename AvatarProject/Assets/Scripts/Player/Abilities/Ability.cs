using System.Collections;
using UnityEngine;

namespace AvatarBA
{
    public class Ability : ScriptableObject
    {

        public Sprite icon;

        public string abilityName = "New Ability";

        [TextArea]
        public string description = "Description";

        public float cooldown;

        public AbilityState state = AbilityState.ready;

        public virtual void Perform() {}

        public IEnumerator CooldownCountdown()
        {
            if(state == AbilityState.cooldown)
                yield break;
            
            state = AbilityState.cooldown;
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
