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
        private List<Middleware> middlewares = new List<Middleware>();

        public InputState GetState()
        {
            currentState = new InputState();

            foreach (var middleware in middlewares)
            {
                middleware.Process(ref currentState);
            }

            return currentState;
        }

        public void Subscribe(Middleware middleware)
        {
            if (middlewares.Contains(middleware))
                return;

            middlewares.Add(middleware);
        }

        public void UnSubscribe(Middleware middleware)
        {
            if (middlewares.Contains(middleware))
            {
                middlewares.Remove(middleware);
            }
        }
    }
}