using UnityEngine;
using System.Collections.Generic;
using System;

namespace AvatarBA
{

    [Serializable]
    public struct InputState
    {
        public Vector3 movementDirection;

        public Vector3 targetPosition;
    }

    public abstract class IInputProvider : ScriptableObject
    {
        protected InputState currentState;
        protected List<Middleware> middlewares = new List<Middleware>();

        public abstract InputState GetState();
     
        public void Subscribe(Middleware middleware)
        {
            if(middlewares.Contains(middleware))
                return;
            
            middlewares.Add(middleware);
        }

        public void UnSubscribe(Middleware middleware)
        {
            if(middlewares.Contains(middleware))
            {
                middlewares.Remove(middleware);
            }
        }
    }
}
