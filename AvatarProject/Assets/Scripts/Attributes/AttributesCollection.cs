using System.Collections.Generic;
using UnityEngine;

namespace AvatarBA.Attributes
{
    [CreateAssetMenu(fileName = "AttributesCollection_New", menuName ="Attributes/Collection")]
    public class AttributesCollection : ScriptableObject
    {
        [SerializeField] private List<AttributeType> _attributes = new List<AttributeType>();

        public List<AttributeType> Attributes
        {
            get => _attributes;
        }
    }
}
