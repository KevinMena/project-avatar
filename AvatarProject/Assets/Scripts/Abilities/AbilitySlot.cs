using AvatarBA.Common;

namespace AvatarBA.Abilities
{
    public enum AbilityKey
    {
        Dash,
        Left,
        Right,
        Ultimate
    }

    [System.Serializable]
    public class AbilitySlot
    {
        public Ability Ability;
        public AbilityKey Key;
        public AbilityState State;
        public Timer CooldownTimer;
        public Timer ActiveTimer;

        public AbilitySlot(AbilityKey key, Ability ability = null)
        {
            Ability = ability;
            Key = key;
            CooldownTimer = new Timer(0);
            ActiveTimer = new Timer(0);
            State = AbilityState.Ready;
        }
    }
}
