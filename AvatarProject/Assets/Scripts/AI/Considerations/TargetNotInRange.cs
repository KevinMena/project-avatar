using UnityEngine;

namespace AvatarBA.AI.Considerations
{
    [CreateAssetMenu(fileName = "AI_Consideration_TargetNotInRange", menuName = "AI/Considerations/Target Not In Range")]
    public class TargetNotInRange : Consideration
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
                    return 0;
            }

            return 1;
        }
    }
}