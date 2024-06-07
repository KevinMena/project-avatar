using UnityEngine;

namespace AvatarBA.AI.Considerations
{
    public class TargetInRange : Consideration
    {
        private bool _invert;

        public TargetInRange(bool invert = false)
        {
            _invert = invert;
        }

        public override float CalculateScore(GameObject owner)
        {
            if(owner.TryGetComponent(out Sensors ownerSensors))
            {
                if(ownerSensors.TargetInRange)
                {
                    return _invert ? 0f : 0.4f;
                }
            }

            return _invert ? 0.4f : 0f;
        }
    }
}