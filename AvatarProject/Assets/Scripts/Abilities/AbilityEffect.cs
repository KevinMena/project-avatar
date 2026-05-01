using UnityEngine;

namespace AvatarBA.Abilities.Effects
{
    public class ExecutionContext
    {
        public Core Owner;

        public ExecutionContext(Core owner)
        {
            Owner = owner;
        }
    }

    [System.Serializable]
    public abstract class AbilityEffect
    {
        [SerializeField]
        private string m_Id;

        public string Id => m_Id;

        public abstract void Execute(ExecutionContext context);
    }
}
