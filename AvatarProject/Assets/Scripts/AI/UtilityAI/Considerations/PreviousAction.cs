using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AvatarBA.AI.Considerations
{
    public class PreviousAction : Consideration
    {
        private string _action;

        public PreviousAction(string action)
        {
            _action = action;
        }

        public override float CalculateScore(GameObject owner)
        {
            if (owner.TryGetComponent(out AIBrain ownerBrain))
            {
                if (ownerBrain.LastAction != null && ownerBrain.LastAction.Name.Equals(_action))
                    return 0.9f;
            }

            return 0.1f;
        }
    }
}
