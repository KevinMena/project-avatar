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

        float GetStat(string id);

        void ApplyChangeToStat(string id, float amount, StatModifierType modifierType);

        void RemoveChangeToStat(string id, StatModifier modifier);

        void RemoveChangeToStatFromSource(string id, object source);
    }
    
}
