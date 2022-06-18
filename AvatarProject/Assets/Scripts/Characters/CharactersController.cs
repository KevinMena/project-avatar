using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AvatarBA
{
    public class CharactersController : MonoBehaviour
    {
        private CharacterMovementController _movementController;

        private CharacterStatsController _statsController;

        public CharacterMovementController MovementController => _movementController;

        public CharacterStatsController StatsController => _statsController;

        protected virtual void Awake() 
        {
            _movementController = GetComponent<CharacterMovementController>();
            _statsController = GetComponent<CharacterStatsController>();
        }
    }
}
