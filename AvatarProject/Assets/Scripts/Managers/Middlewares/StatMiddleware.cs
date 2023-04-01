using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using AvatarBA.UI;
using AvatarBA.Debugging;

namespace AvatarBA.Managers
{
    [CreateAssetMenu(fileName = "Middleware_StatsDisplay", menuName = "Middlewares/Stats Display")]
    public class StatMiddleware : ScriptableObject
    {
        private StatDisplay _display = null;

        public void Subscribe(StatDisplay display)
        {
            if(_display != null)
            {
                GameDebug.LogWarning("Stat display already exists, something is trying to subscribe.");
                return;
            }

            _display = display;
        }

        public void Unsubscribe(StatDisplay display)
        {
            if(_display == null || _display != display)
            {
                GameDebug.LogWarning("Stat display not set or something is trying to unsubscribe");
                return;
            }

            _display = null;
        }

        public void SetupDisplay(KeyValuePair<string, StatRecord>[] currentStats)
        {
            _display?.Setup(currentStats);
        }

        public void UpdateStat(string id, StatRecord record)
        {
            _display?.UpdateStat(id, record);
        }
    }
}