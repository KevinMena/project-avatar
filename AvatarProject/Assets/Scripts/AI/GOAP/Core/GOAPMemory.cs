using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AvatarBA.AI.GOAP
{
    public class GOAPMemory : MonoBehaviour
    {
        [SerializeField]
        private WorldContext _context;

        private Dictionary<string, object> _shortTermMemory;
        private Dictionary<string, object> _longTermMemory;

        public ref readonly WorldContext CurrentContext => ref _context;

        private void Awake()
        {
            _shortTermMemory = new Dictionary<string, object>();
            _longTermMemory = new Dictionary<string, object>();
            _context = new WorldContext();
        }

        private void Start()
        {
            AddLongData("SpawnPoint", transform.position);
        }

        public void AddShortData(string id, object value)
        {
            _shortTermMemory[id] = value;
        }

        public object GetShortData(string id)
        {
            _shortTermMemory.TryGetValue(id, out object value);
            return value;
        }

        public void AddLongData(string id, object value)
        {
            _longTermMemory[id] = value;
        }

        public object GetLongData(string id)
        {
            _longTermMemory.TryGetValue(id, out object value);
            return value;
        }

        public void AddWorldState(string id, bool value)
        {
            if(_context.ContainsKey(id))
            {
                _context[id] = value;
                return;
            }

            _context.Add(id, value);
        }

        public bool CheckWorldState(WorldState state)
        {
            return _context.Contains(new KeyValuePair<string, bool>(state.Id, state.Value));
        }
    }
}
