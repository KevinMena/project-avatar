using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AvatarBA.AI.Core
{
    public enum WorldStateType
    {
        Bool,
        Integer,
        Float,
        String
    }

    public class WorldStateDatabase : ScriptableObject
    {
        private GenericDictionary<string, object> _database = new GenericDictionary<string, object>();

        public void AddElementToDatabase(string id, object value)
        {
            _database[id] = value;
        }
    }
}