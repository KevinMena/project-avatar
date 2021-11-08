using UnityEngine;
using System.Collections.Generic;

namespace AvatarBA.Attributes
{
    [CreateAssetMenu(fileName = "AttributesContainer_New", menuName ="Attributes/Container")]
    public class AttributesContainer : ScriptableObject
    {
        [SerializeField] private AttributesCollection _collection;

        private Dictionary<string, AttributeType> _attributesMap = new Dictionary<string, AttributeType>();

        private bool _isInitialized = false;

        private void Initialize()
        {
            if(_isInitialized) return;

            foreach (AttributeType currentAttribute in _collection.Attributes)
            {
                _attributesMap.Add(currentAttribute.Id, currentAttribute);
            }

            _isInitialized = true;
        }

        public Dictionary<string, Attribute> CreateRuntimeValues()
        {
            Dictionary<string, Attribute> runtime = new Dictionary<string, Attribute>();
            foreach (AttributeType currentAttribute in _collection.Attributes)
            {
                runtime.Add(currentAttribute.Id, new Attribute(currentAttribute.DefaultValue));
            }

            return runtime;
        }

        public AttributeType GetAttribute(string id)
        {
            if(_attributesMap.TryGetValue(id, out var result))
                return result;

            return null;
        }

        public float GetAttributeValue(string id)
        {
            if(_attributesMap.TryGetValue(id, out var result))
                return result.DefaultValue;

            return -1;
        }
    }
}
