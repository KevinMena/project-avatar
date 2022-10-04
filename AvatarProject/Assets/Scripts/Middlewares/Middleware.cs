using UnityEngine;

namespace AvatarBA.Managers
{
    public abstract class Middleware : MonoBehaviour
    {
        [SerializeField]
        protected InputProvider _provider = default;

        public abstract void Process(ref InputState currentState);
    }
}