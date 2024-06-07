using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AvatarBA
{
    [CreateAssetMenu(fileName = "Character_Enemy_", menuName = "Characters/New Enemy")]
    public class EnemyData : CharacterData
    {
        [SerializeField]
        private float _range;

        public float Range => _range;
    }
}
