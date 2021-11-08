using System.Collections.Generic;

namespace AvatarBA.Attributes
{
    [System.Serializable]
    public class Attribute: IAttribute
    {
        private float _baseValue = 0f;
        private readonly List<AttributeModifier> _attrModifiers = new List<AttributeModifier>();

        private float _value;
        private bool _isDirty = true;

        public float Value
        {
            get
            {
                if(_isDirty)
                {
                    _value = CalculateCurrentValue();
                    _isDirty = false;
                }

                return _value;
            }
        }

        public Attribute(float value) => _baseValue = value;

        public void AddModifier(AttributeModifier modifier)
        {
            _isDirty = true;
            
            _attrModifiers.Add(modifier);
            _attrModifiers.Sort((x, y) => x.CompareAttribute(y));
        }

        public void RemoveModifier(AttributeModifier modifier)
        {
            if(!_attrModifiers.Contains(modifier))
                return;

            _isDirty = true;
            _attrModifiers.Remove(modifier);
        }

        private float CalculateCurrentValue()
        {
            float currentValue = _baseValue;
            float sumPercentAdd = 0f;

            for (int index = 0; index < _attrModifiers.Count; index++)
            {
                AttributeModifier currentModifier = _attrModifiers[index];
                
                switch (currentModifier.ModifierType)
                {
                    case AttributeModifierType.Flat:
                        currentValue += currentModifier.Value;
                        break;
                    case AttributeModifierType.PercentAdditive:
                        sumPercentAdd += currentModifier.Value;
                        if(index + 1 >= _attrModifiers.Count || 
                        _attrModifiers[index + 1].ModifierType != AttributeModifierType.PercentAdditive)
                        {
                            currentValue *= 1 + sumPercentAdd;
                            sumPercentAdd = 0f;
                        }
                        break;
                    case AttributeModifierType.PercentMultiplicative:
                        currentValue *= 1 + currentModifier.Value;
                        break;
                    default:
                        break;
                }
            }

            return currentValue;
        }
    }
}