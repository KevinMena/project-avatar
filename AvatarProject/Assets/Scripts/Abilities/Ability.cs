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

        public List<AbilityEffect> effects = new List<AbilityEffect>();

        public abstract void Initialize();
        public abstract IEnumerator Trigger(CharactersController owner, CharactersController target = null);

        public IEnumerator OnCooldown()
        {
            yield return new WaitForSeconds(cooldown);
        }
    }

    public enum AbilityState
    {
        ready,
        active,
        cooldown   
    }
}
