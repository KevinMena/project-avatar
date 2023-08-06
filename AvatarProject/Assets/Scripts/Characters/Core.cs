using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AvatarBA
{
    public class Core : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField]
        private CharacterData _characterData;

        [SerializeField]
        private Transform _shootPosition;

        private Vector3 _spawnPosition;

        // Controls
        private AnimationControl _animations;
        private StatsControl _stats;
        private MovementControl _movement;

        public CharacterData Data => _characterData;
        public Transform ShootPosition => _shootPosition;
        public Vector3 SpawnPosition => _spawnPosition;

        public AnimationControl Animation => _animations;
        public StatsControl Stats => _stats;
        public MovementControl Movement => _movement;

        private void Awake()
        {
            _animations = GetComponent<AnimationControl>();
            _stats = GetComponent<StatsControl>();
            _movement = GetComponent<MovementControl>();
        }
    }
}
