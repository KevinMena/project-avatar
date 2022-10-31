using System;
using System.Collections.Generic;
using UnityEngine.tvOS;

namespace AvatarBA.Stats
{
    [Serializable]
    public class Stat: IStat
    {
        private string _name;
        private float _baseValue = 0f;
        private readonly List<StatModifier> _statModifiers = new List<StatModifier>();

        private float _value;

        private float _lastBaseValue = float.MinValue;
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
                if(_isDirty || _lastBaseValue != BaseValue)
                {
                    _lastBaseValue = BaseValue;
                    _value = CalculateCurrentValue();
                    _isDirty = false;
                }

                return _value;
            }
        }

        public float BaseValue
        {
            get { return _baseValue; }
            set { _baseValue = value; }
        }

        public string Name => _name;

        public Stat(string name, float value)
        {
            _name = name; 
            _baseValue = value;
        }

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
        /// <returns> If managed to remove it </returns>
        public bool RemoveModifier(StatModifier modifier)
        {
            if(_statModifiers.Remove(modifier))
            {
                _isDirty = true;
                return true;
            }
            
            return false;
        }

        public bool RemoveModifier(string modifierId)
        {
            for (int i = _statModifiers.Count - 1; i >= 0; i--)
            {
                if (_statModifiers[i].Id == modifierId)
                {
                    _isDirty = true;
                    _statModifiers.RemoveAt(i);
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Remove all modifiers from one source
        /// </summary>
        /// <param name="source">Object source of modifiers</param>
        /// <returns> If removed something or not </returns>
        public bool RemoveModifiersFromSource(object source)
        {
            bool removed = false;

            for(int i = _statModifiers.Count - 1; i >= 0; i--)
            {
                if(_statModifiers[i].Source == source)
                {
                    _isDirty = true;
                    removed = true;
                    _statModifiers.RemoveAt(i);
                }
            }

            return removed;
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