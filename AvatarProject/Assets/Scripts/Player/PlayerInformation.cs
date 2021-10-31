using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AvatarBA
{    
    [CreateAssetMenu(fileName = "Player Information", menuName = "Player/Player Information")]
    public class PlayerInformation : ScriptableObject
    {
        public float runningSpeed = 5f;

        public float rotationSpeed = 14f;

        public float dashingSpeed = 15f;

        public float dashTime = 0.25f;
    }
}
