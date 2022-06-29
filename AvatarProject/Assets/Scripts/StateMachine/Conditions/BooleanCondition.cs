namespace AvatarBA.Patterns
{    
    public class BooleanCondition : Condition
    {
        private ConditionValue<bool> _targetValue;

        public BooleanCondition(ref ConditionValue<bool> currentValue)
        {
            _targetValue = currentValue;
        }

        public override bool Test()
        {
            return _targetValue.Value;
        }
    }
}
