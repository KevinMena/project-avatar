namespace AvatarBA.Stats
{
    public interface IStat
    {
        float Value { get; }

        void AddModifier(StatModifier modifier);

        void RemoveModifier(StatModifier modifier);
    }
}