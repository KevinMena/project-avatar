using UnityEngine;

namespace AvatarBA
{
    public abstract class Middleware : ScriptableObject
    {
        public virtual void Process(ref InputState currentState) { }
    }
}