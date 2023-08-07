using UnityEngine;

namespace AvatarBA.AI.Considerations
{
    public class TargetInRange : Consideration
    {
        public override float CalculateScore(GameObject owner)
        {
            if(owner.TryGetComponent(out Sensors ownerSensors))
            {
                if(ownerSensors.TargetInRange)
                {
                    return 1;
                }
            }

            return 0;
        }
    }
}