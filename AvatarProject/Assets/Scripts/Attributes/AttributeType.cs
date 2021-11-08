using UnityEngine;

namespace AvatarBA.Attributes
{
    [CreateAssetMenu(fileName = "AttributeType_New", menuName = "Attributes/Type")]
    public class AttributeType : ScriptableObject 
    {
        [SerializeField] private string _id = "Attribute";
        [SerializeField] private string _displayName = "New Attribute";
        [SerializeField] private float _defaultValue = 0f;

        public string Id
        {
            get => _id;
        }

        public string DisplayName
        {
            get => _displayName;
        }

        public float DefaultValue
        {
            get => _defaultValue;
        }
    }
}