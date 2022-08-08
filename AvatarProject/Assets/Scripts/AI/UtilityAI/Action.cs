using System.Collections;
using UnityEngine;

namespace AvatarBA.AI
{
    public abstract class Action : ScriptableObject
    {
        [SerializeField]
        private string _name;

        [SerializeField]
        private Consideration[] _considerations;
        
        public string Name => _name;
        public  Consideration[] Considerations => _considerations;

        public abstract IEnumerator Execute(GameObject owner);
    }
}

