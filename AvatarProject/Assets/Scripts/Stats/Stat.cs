using System;
using System.Collections.Generic;

namespace AvatarBA.Stats
{
    [Serializable]
    public class Stat: IStat
    {
        private float _baseValue = 0f;
        private readonly List<StatModifier> _statModifiers = new List<StatModifier>();

        private float _value;
        private bool _isDirty = true;

        /// <summary>
        /// Returns the current value of the stat with all the modifiers, 
        /// if is has to be updated first does that and the return.
        /// </summary>
        /// <value></value>
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

        public Stat(float value) => _baseValue = value;

        /// <summary>
        /// Add a modifier to the list and mark the value to be updated
        /// </summary>
        /// <param name="modifier">Modifier</param>
        public void AddModifier(StatModifier modifier)
        {
            _isDirty = true;
            
            _statModifiers.Add(modifier);
            _statModifiers.Sort((x, y) => x.CompareStat(y));
        }

        /// <summary>
        /// Removes a modifier from the list if exists and mark the value to be updated
        /// </summary>
        /// <param name="modifier">Modifier</param>
        public void RemoveModifier(StatModifier modifier)
        {
            if(!_statModifiers.Contains(modifier))
                return;

            _isDirty = true;
            _statModifiers.Remove(modifier);
        }

        /// <summary>
        /// Calculates and returns the current value after going through every modifier
        /// </summary>
        private float CalculateCurrentValue()
        {
            float currentValue = _baseValue;
            float sumPercentAdd = 0f;

            for (int index = 0; index < _statModifiers.Count; index++)
            {
                StatModifier currentModifier = _statModifiers[index];
                
                switch (currentModifier.ModifierType)
                {
                    // If is flat just add it
                    case StatModifierType.Flat:
                        currentValue += currentModifier.Value;
                        break;
                    
                    // If is additive percentage, accumulate every modifier of the same type
                    // then adds it
                    case StatModifierType.PercentAdditive:
                        sumPercentAdd += currentModifier.Value;
                        if(index + 1 >= _statModifiers.Count || 
                        _statModifiers[index + 1].ModifierType != StatModifierType.PercentAdditive)
                        {
                            currentValue *= 1 + sumPercentAdd;
                            sumPercentAdd = 0f;
                        }
                        break;
                    
                    // If is multiplicative, multiplicate with the current value
                    case StatModifierType.PercentMultiplicative:
                        currentValue *= 1 + currentModifier.Value;
                        break;
                    default:
                        break;
                }
            }
            // Returns rounded value to 4 digits for precision
            return (float) Math.Round(currentValue, 4);
        }
    }
}