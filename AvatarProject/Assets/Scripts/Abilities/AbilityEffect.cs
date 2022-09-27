using UnityEngine;

namespace AvatarBA.Abilities.Effects
{
    public abstract class AbilityEffect : ScriptableObject
    {
        [SerializeField]
        private string _name;

        public string Name => _name;

        public abstract void Cast(GameObject owner);
    }
}
