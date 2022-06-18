using UnityEngine;

namespace AvatarBA
{
    public abstract class AbilityEffect : ScriptableObject
    {
        [SerializeField]
        private string _name;

        [SerializeField, TextArea]
        private string _description;

        public string Name => _name;

        public string Description => _description;

        public abstract void Cast();
    }
}
