using UnityEngine;

namespace AvatarBA.Stats
{
    /// <summary>
    /// Scriptable object that holds the information of a stat for reusability
    /// </summary>
    [CreateAssetMenu(fileName = "StatType_New", menuName = "Stats/Type")]
    public class StatType : ScriptableObject 
    {
        [SerializeField] private string _id = "stat";
        [SerializeField] private string _displayName = "New Stat";
        [SerializeField] private float _defaultValue = 0f;

        /// <summary>
        /// Id for looking into collections
        /// </summary>
        public string Id
        {
            get => _id;
        }

        /// <summary>
        /// Name that the stat have ingame
        /// </summary>
        public string DisplayName
        {
            get => _displayName;
        }

        /// <summary>
        /// Default value to initialize stat
        /// </summary>
        public float DefaultValue
        {
            get => _defaultValue;
        }
    }
}