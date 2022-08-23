using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using AvatarBA.NPC;

namespace AvatarBA.AI.Actions
{
    [CreateAssetMenu(fileName = "AI_Action_Wander", menuName = "AI/Actions/Wander")]
    public class Wander : Action
    {
        [SerializeField]
        private float _walkRange = 0;

        public override IEnumerator Execute(GameObject owner)
        {
            if (_walkRange <= 0)
                yield break;
            
            float distanceX = Random.Range(-_walkRange, _walkRange);
            float distanceZ = Random.Range(-_walkRange, _walkRange);

            Vector3 targetPosition = new Vector3(owner.transform.position.x + distanceX, owner.transform.position.y, owner.transform.position.z + distanceZ);

            if (owner.TryGetComponent(out NPCMiddleware movementMiddleware))
            {
                movementMiddleware.SetDestination(targetPosition);
            }
            Debug.Log($"Wandering to {targetPosition} ");
            yield return new WaitForSeconds(3f);
        }
    }
}