using System;
using System.Collections.Generic;
using UnityEngine;

namespace AvatarBA.Managers
{
    [Serializable]
    public struct InputState
    {
        public Vector3 MovementDirection;

        public Vector3 RotationDirection;

        public float Speed;
    }

    [CreateAssetMenu(fileName = "InputProvider_Movement", menuName = "Providers/Input Provider")]
    public class InputProvider : ScriptableObject
    {
        private InputState currentState;
        private SortedDictionary<int, Middleware> middlewares = new SortedDictionary<int, Middleware>();

        public InputState GetState()
        {
            currentState = new InputState();

            foreach (var middleware in middlewares)
            {
                middleware.Value.Process(ref currentState);
            }

            return currentState;
        }

        public void Subscribe(Middleware middleware, int priority)
        {
            middlewares.TryAdd(priority, middleware);
        }

        public void UnSubscribe(Middleware middleware, int priority)
        {
            if (middlewares.ContainsValue(middleware))
            {
                middlewares.Remove(priority);
            }
        }
    }
}