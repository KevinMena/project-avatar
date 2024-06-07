using UnityEngine;

namespace AvatarBA.AI.Considerations
{
    public class InRange : Consideration
    {
        private float _range;

        public InRange(float range)
        {
            _range = range;
        }

        public override float CalculateScore(GameObject owner)
        {
            if (owner.TryGetComponent(out Sensors ownerSensors))
            {
                if (Vector3.Distance(owner.transform.position, ownerSensors.TargetPosition) <= _range)
                {
                    return 0.7f;
                }
            }

            return 0;
        }
    }
}
