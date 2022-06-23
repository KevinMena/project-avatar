using System.Collections;
using System.Collections.Generic;
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

        public float activeTime;

        public List<AbilityEffect> effects = new List<AbilityEffect>();

        public abstract void Initialize();
        public abstract IEnumerator Trigger(GameObject owner);

        public IEnumerator OnCooldown(AbilityState currentState)
        {
            if(activeTime > 0)
            {
                currentState = AbilityState.active;
                yield return new WaitForSeconds(activeTime);
            }

            currentState = AbilityState.cooldown;
            yield return new WaitForSeconds(cooldown);
            currentState = AbilityState.ready;
        }
    }

    public enum AbilityState
    {
        ready,
        active,
        cooldown   
    }
}
