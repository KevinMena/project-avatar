using System.Collections;
using AvatarBA.Stats;
using UnityEngine;

namespace AvatarBA.Abilities.Effects
{
    [System.Serializable]
    public class StatModifierEffect : AbilityEffect
    {
        [SerializeField]
        protected string m_StatId;
        [SerializeField]
        protected StatModifierType m_Type;
        [SerializeField]
        protected float m_BaseModifier = 0;
        [SerializeField]
        protected float m_Duration = 0;

        public override void Execute(ExecutionContext context)
        {
            Core owner = context.Owner;

            owner.StartCoroutine(ApplyChange(owner));
        }

        protected IEnumerator ApplyChange(Core owner)
        {
            owner.Stats.ApplyChangeToStat(m_StatId, Id, m_BaseModifier, m_Type);
            yield return new WaitForSeconds(m_Duration);
            owner.Stats.RemoveChangeToStat(m_StatId, Id);
        }
    }
}


