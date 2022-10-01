using System.Collections.Generic;
using UnityEngine;

using AvatarBA.Managers;
using AvatarBA.Stats;

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
            _displayManager.Setup(GetAllStatsDisplayName());
        }

        public void ApplyChangeToHealth(string modifierId, float value, StatModifierType modifierType)
        {
            ApplyChangeToStat(HEALTH_STAT, modifierId, value, modifierType);
            UpdateStatInUI(HEALTH_STAT);
        }

        public override void ApplyChangeToStat(string statId, string modifierId, float value, StatModifierType modifierType)
        {
            base.ApplyChangeToStat(statId, modifierId, value, modifierType);
            UpdateStatInUI(statId);
        }

        public override void RemoveChangeToStat(string statId, string modifierId)
        {
            base.RemoveChangeToStat(statId, modifierId);
            UpdateStatInUI(statId);
        }

        private void UpdateStatInUI(string id)
        {
            if (_runtimeStats.TryGetValue(id, out var record))
                _displayManager.UpdateStat(id, record);
        }
    }
}

