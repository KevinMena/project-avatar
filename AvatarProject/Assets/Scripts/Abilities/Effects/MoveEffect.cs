using System.Collections;
using UnityEngine;

namespace AvatarBA.Abilities.Effects
{
    [System.Serializable]
    public class MoveEffect : AbilityEffect
    {
        [SerializeField]
        private float m_Speed = 0;

        [SerializeField]
        private float m_Distance = 0;

        public override void Execute(ExecutionContext context)
        {
            Core owner = context.Owner;

            owner.StartCoroutine(Trigger(owner));
        }

        public IEnumerator Trigger(Core owner)
        {
            // Calculate correct direction base on where the owner is looking
            Vector3 targetPosition = owner.transform.position + (owner.Movement.AimDirection * m_Distance);

            owner.Movement.DisableMovement();

            owner.Movement.Impulse(owner.Movement.AimDirection, m_Speed);

            float cSquared;

            do
            {
                Vector3 offset = targetPosition - owner.transform.position;
                offset.y = 0;
                cSquared = offset.Distance();
                yield return null;
            } while (cSquared > 0.1f);

            owner.Movement.EnableMovement();
            yield return null;
        }
    }
}