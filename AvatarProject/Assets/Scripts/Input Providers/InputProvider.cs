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
        private SortedDictionary<int, Processor> processors = new SortedDictionary<int, Processor>();

        public InputState GetState()
        {
            currentState = new InputState();

            foreach (var processor in processors)
            {
                processor.Value.Process(ref currentState);
            }

            return currentState;
        }

        public void Subscribe(Processor middleware, int priority)
        {
            processors.TryAdd(priority, middleware);
        }

        public void UnSubscribe(Processor middleware, int priority)
        {
            if (processors.ContainsValue(middleware))
            {
                processors.Remove(priority);
            }
        }
    }
}