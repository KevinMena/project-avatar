using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AvatarBA.AI.Considerations
{
    [CreateAssetMenu(fileName = "AI_Consideration_TargetInRange", menuName = "AI/Considerations/Target In Range")]
    public class TargetInRange : Consideration
    {
        [SerializeField]
        private string _targetTag;

        [SerializeField]
        private float _range;

        public override float CalculateScore(GameObject owner)
        {
            Collider[] hitColliders = Physics.OverlapSphere(owner.transform.position, _range);

            foreach (var hit in hitColliders)
            {
                if (hit.CompareTag(_targetTag))
                    return 1;
            }

            return 0;
        }
    }
}