using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AvatarBA
{
    public class PlayerController : CharactersController
    {
        private PlayerMovementController _playerMovementController;
        private PlayerStatsController _playerStatsController;

        public PlayerMovementController PMovementController => _playerMovementController;

        public PlayerStatsController PStatsController => _playerStatsController;

        protected override void Awake()
        {
            base.Awake();
            _playerMovementController = base.MovementController as PlayerMovementController;
            _playerStatsController = base.StatsController as PlayerStatsController;
        }
    }
}