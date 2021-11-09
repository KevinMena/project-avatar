using System.Collections.Generic;

namespace AvatarBA.Stats
{
    /// <summary>
    /// Interface for all the containers of stats
    /// </summary>
    public interface IStatContainer
    {
        /// <summary>
        /// Collection of stats
        /// </summary>
        /// <value></value>
        List<StatType> Stats { get; }

        /// <summary>
        /// Returns a map with the runtime stats to be change.
        /// </summary>
        /// <returns></returns>
        Dictionary<string, Stat> CreateRuntimeValues();

        /// <summary>
        /// Returns stat type with all the information
        /// </summary>
        /// <param name="id">Identifier of the stat</param>
        /// <returns></returns>
        StatType GetStat(string id);

        /// <summary>
        /// Returns base value of the stat
        /// </summary>
        /// <param name="id">Identifier of the stat</param>
        /// <returns></returns>
        float GetStatValue(string id);
    }
    
}
