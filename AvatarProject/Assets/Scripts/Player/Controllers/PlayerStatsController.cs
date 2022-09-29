using System.Collections.Generic;
using UnityEngine;

using AvatarBA.Managers;

namespace AvatarBA
{    
    public class PlayerStatsController : CharacterStatsController
    {
        [SerializeField]
        protected StatsDisplayManager _displayManager;

        private const string HEALTH_STAT = "health";
        private const string MAX_HEALTH_STAT = "maxHealth";

        protected override void Start()
        {
            base.Start();
            _displayManager.UpdateMaxHealth(GetStat(MAX_HEALTH_STAT));
        }
    }
}

