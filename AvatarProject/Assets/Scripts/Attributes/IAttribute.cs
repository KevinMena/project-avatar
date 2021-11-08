namespace AvatarBA.Attributes
{
    public interface IAttribute
    {
        float Value { get; }

        void AddModifier(AttributeModifier modifier);

        void RemoveModifier(AttributeModifier modifier);
    }
}