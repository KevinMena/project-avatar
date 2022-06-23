namespace AvatarBA.Stats
{
    public interface IStat
    {
        float Value { get; }

        void AddModifier(StatModifier modifier);

        bool RemoveModifier(StatModifier modifier);

        bool RemoveModifiersFromSource(object source);
    }
}