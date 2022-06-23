using System.Collections.Generic;

namespace AvatarBA.Stats
{
    /// <summary>
    /// Interface for all the containers of stats
    /// </summary>
    public interface IStatContainer
    {
        /// <summary>
        /// Returns a map with the runtime stats to be change.
        /// </summary>
        /// <returns></returns>
        void CreateRuntimeValues();

        /// <summary>
        /// Returns base value of the stat
        /// </summary>
        /// <param name="id">Identifier of the stat</param>
        /// <returns></returns>
        float GetStatValue(string id);

        void ApplyChangeToStat(string id, float amount, StatModifierType modifierType);

        void RemoveChangeToStat(string id, float amount, StatModifierType modifierType);
    }
    
}
