namespace AvatarBA.Attributes
{
    [System.Serializable]
    public class AttributeModifier
    {
        private float _value;
        private AttributeModifierType _modifierType;

        public float Value => _value;

        public AttributeModifierType ModifierType => _modifierType;

        public AttributeModifier(float value, AttributeModifierType modifierType)
        {
            _value = value;
            _modifierType = modifierType;
        }

        public int CompareAttribute(AttributeModifier other)
        {
            if (ModifierType < other.ModifierType)
                return -1;
            else if (ModifierType > other.ModifierType)
                return 1;
            return 0;
        }
    }

    public enum AttributeModifierType
    {
        Flat = 0,
        PercentAdditive = 1,
        PercentMultiplicative = 2
    }
}
