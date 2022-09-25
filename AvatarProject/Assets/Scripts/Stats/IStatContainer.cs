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

        void ApplyChangeToStat(Stat stat, float amount, StatModifierType modifierType);

        void RemoveChangeToStat(Stat stat, float amount, StatModifierType modifierType);

        void RemoveChangeToStatFromSource(Stat stat, object source);
    }
    
}
