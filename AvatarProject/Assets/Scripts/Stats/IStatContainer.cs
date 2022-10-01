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

        void ApplyChangeToStat(string statId, string modifierId, float value, StatModifierType modifierType);

        void ApplyChangeToStat(string statId, string modifierId, float value, StatModifierType modifierType, object owner);

        void RemoveChangeToStat(string statId, string modifierId);

        void RemoveChangeToStat(string statId, StatModifier modifier);

        void RemoveChangeToStatFromSource(string id, object source);
    }
    
}
