using System;
using UnityEngine;

namespace AvatarBA.Stats
{
    [CreateAssetMenu(fileName = "StatType_", menuName = "Stats/Type")]
    public class StatType : ScriptableObject
    {
        /// <summary>
        /// Id for looking into collections
        /// </summary>
        [SerializeField]
        private string _id;

        /// <summary>
        /// Name that the stat have ingame
        /// </summary>
        [SerializeField]
        private string _displayName;

        public string Id => _id;

        public string DisplayName => _displayName;
    }

    [Serializable]
    public struct StatBase
    {
        public StatType Type;

        public float DefaultValue;
    }
}
