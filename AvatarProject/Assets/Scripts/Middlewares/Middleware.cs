using UnityEngine;

namespace AvatarBA
{
    public abstract class Middleware : MonoBehaviour
    {
        public abstract void Process(ref InputState currentState);
    }
}