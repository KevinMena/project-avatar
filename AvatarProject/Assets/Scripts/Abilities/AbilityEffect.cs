using System.Collections;
using UnityEngine;

namespace AvatarBA.Abilities.Effects
{
    public abstract class AbilityEffect : ScriptableObject
    {
        [SerializeField]
        private string _name;

        public string Name => _name;

        public abstract IEnumerator Cast(GameObject owner);
    }
}
