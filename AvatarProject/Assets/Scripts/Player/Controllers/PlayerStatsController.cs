using System.Collections.Generic;
using UnityEngine;

using AvatarBA.Managers;

namespace AvatarBA
{    
    public class PlayerStatsController : CharacterStatsController
    {
        [SerializeField]
        protected StatsDisplayManager _displayManager;

        protected override void Start()
        {
            base.Start();
            _displayManager.UpdateMaxHealth(BaseHealth);
        }
    }
}

