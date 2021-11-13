using UnityEngine;
using System;

namespace AvatarBA
{    
    [Serializable]
    public struct InputState
    {
        public Vector2 movementDirection;

        public Vector2 mousePosition;

        public bool canDash;
    }
}
