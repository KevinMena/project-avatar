using UnityEngine;

namespace AvatarBA.Managers
{
    public abstract class Middleware : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        protected InputProvider _provider = default;

        [Header("Data")]
        [SerializeField]
        protected int _priority;

        public abstract void Process(ref InputState currentState);
    }
}