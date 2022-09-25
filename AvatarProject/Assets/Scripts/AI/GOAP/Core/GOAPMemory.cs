using System.Collections;
using System.Collections.Generic;

namespace AvatarBA.AI.Core
{
    public class GOAPMemory
    {
        private GenericDictionary<string, bool> _currentWorldState;

        public GOAPMemory()
        {
            _currentWorldState = new GenericDictionary<string, bool>();
        }

        public ref readonly GenericDictionary<string, bool> CurrentWorldState => ref _currentWorldState;

        public void AddWorldState(string id, bool value)
        {
            _currentWorldState[id] = value;
        }
    }
}
