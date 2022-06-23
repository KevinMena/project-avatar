using System;

namespace AvatarBA.Stats
{
    [Serializable]
    public class StatModifier
    {
        private float _value;
        private StatModifierType _modifierType;
        private object _source;

        public float Value => _value;
        public StatModifierType ModifierType => _modifierType;
        public object Source => _source;

        public StatModifier(float value, StatModifierType modifierType, object source)
        {
            _value = value;
            _modifierType = modifierType;
            _source = source;
        }

        public StatModifier(float value, StatModifierType modifierType) : this(value, modifierType, null) { }

        /// <summary>
        /// Compares the type with other modifier to create an order
        /// </summary>
        /// <param name="other">Other modifier</param>
        /// <returns>-1 if smaller, 1 if bigger or 0 if equals</returns>
        public int CompareStat(StatModifier other)
        {
            if (ModifierType < other.ModifierType)
                return -1;
            else if (ModifierType > other.ModifierType)
                return 1;
            return 0;
        }
    }

    /// <summary>
    /// Types of modifiers for the stats, depending of them
    /// the stats change in a different way
    /// </summary>
    public enum StatModifierType
    {
        Flat = 100,
        PercentAdditive = 200,
        PercentMultiplicative = 300
    }
}
