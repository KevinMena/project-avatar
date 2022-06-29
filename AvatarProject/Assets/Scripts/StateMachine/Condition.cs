namespace AvatarBA.Patterns
{
    public class ConditionValue<T> where T : struct
    {
        public T Value { get; set; }
        public ConditionValue(T value) { this.Value = value; }
    }

    public abstract class Condition
    {
        public abstract bool Test();
    }
}