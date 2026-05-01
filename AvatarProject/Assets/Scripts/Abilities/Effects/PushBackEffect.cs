using UnityEngine;

namespace AvatarBA.Abilities.Effects
{
    [System.Serializable]
    public class PushBackEffect : AbilityEffect
    {
        [SerializeField]
        private float m_PushBack;

        public override void Execute(ExecutionContext context)
        {
            Core owner = context.Owner;
            owner.Movement.Impulse(owner.transform.forward.normalized, m_PushBack);
        }
    }
}

