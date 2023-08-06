using UnityEngine;

namespace AvatarBA.AI.Considerations
{
    public class TargetInRange : Consideration
    {
        public override float CalculateScore(GameObject owner)
        {
            if(owner.TryGetComponent(out Sensors ownerSensors))
            {
                if(ownerSensors.TargetPosition != Vector3.zero)
                {
                    return 1;
                }
            }

            return 0;
        }
    }
}