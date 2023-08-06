using UnityEngine;

namespace AvatarBA.AI.Considerations
{
    public class TargetNotInRange : Consideration
    {
        public override float CalculateScore(GameObject owner)
        {
            if (owner.TryGetComponent(out Sensors ownerSensors))
            {
                if (ownerSensors.TargetPosition != Vector3.zero)
                {
                    return 0;
                }
            }

            return 1;
        }
    }
}