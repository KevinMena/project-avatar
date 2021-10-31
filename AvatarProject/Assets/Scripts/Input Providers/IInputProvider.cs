using UnityEngine;
using System.Collections.Generic;
using System;

namespace AvatarBA
{
    public abstract class IInputProvider : ScriptableObject
    {
        public event Action OnDash;
        public abstract InputState GetState();

        protected InputState currentState;

        protected List<Middleware> middlewares = new List<Middleware>();

        public void Subscribe(Middleware manager)
        {
            if(middlewares.Contains(manager))
                return;
            
            middlewares.Add(manager);
        }

        public void Dash()
        {
            if(currentState.canDash) OnDash?.Invoke();
        }
    }
}
