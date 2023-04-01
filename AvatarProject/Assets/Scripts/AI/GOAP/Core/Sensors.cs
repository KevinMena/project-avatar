using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using AvatarBA.Debugging;

namespace AvatarBA.AI.Core
{
    public class Sensors : MonoBehaviour
    {
        [SerializeField]
        private LayerMask _targetMask;
        [SerializeField]
        private float _visionCone;
        [SerializeField]
        private float _visionRange;
        [SerializeField]
        private float _hearingRange;

        private GOAPMemory _memory;
        private Collider[] _targets;

        private float _hearingTimer = 0;
        private float _visionTimer = 0;

        private const float HEARING_COOLDOWN = 1f;
        private const float VISION_COOLDOWN = 1f;
        private const string HEARING_TAG = "TargetInHearingRange";
        private const string HEARING_POS_TAG = "HearingTargetAt";
        private const string VISION_TAG = "TargetInVision";
        private const string VISION_POS_TAG = "InVisionTargetAt";


        private float _cosAngle = 0;

        public float CosAngle => _cosAngle;

        private void Awake()
        {
            _memory = GetComponent<GOAPMemory>();
        }

        private void Start()
        {
            _targets = new Collider[5];
            _cosAngle = Mathf.Cos(_visionCone * Mathf.Deg2Rad);
        }

        private void Update()
        {
            // Update timers
            _hearingTimer += Time.deltaTime;
            _visionTimer += Time.deltaTime;

            // Check if we are in the windows where we can check the sensors
            if (_hearingTimer > HEARING_COOLDOWN)
            {
                CheckHear();
                _hearingTimer = 0;
            }

            if (_visionTimer > VISION_COOLDOWN)
            {
                CheckVision();
                _visionTimer = 0;
            }
        }

        private void CheckHear()
        {
            // Check if targets in range of hearing
            Physics.OverlapSphereNonAlloc(transform.position, _hearingRange, _targets, _targetMask);

            for(int i = 0; i < _targets.Length; i++)
            {
                // Ignore self
                if (_targets[i] is null || _targets[i].gameObject == gameObject)
                    continue;

                // Register target in memory
                Vector2 noise = Random.insideUnitCircle * 1.5f;
                Vector3 approximatePosition = _targets[i].transform.position + new Vector3(noise.x, transform.position.y, noise.y);
                _memory.AddWorldState(HEARING_TAG, true);
                _memory.AddShortData(HEARING_POS_TAG, approximatePosition);
                GameDebug.Log($"Hearing target {_targets[i].name} around {approximatePosition}");

                // Clean collection
                _targets[i] = null;
            }
        }
        
        private void CheckVision()
        {
            // Check if targets in range 
            Physics.OverlapSphereNonAlloc(transform.position, _visionRange, _targets, _targetMask);

            for (int i = 0; i < _targets.Length; i++)
            {
                // Ignore self
                if (_targets[i] is null || _targets[i].gameObject == gameObject)
                    continue;

                Vector3 directionToTarget = _targets[i].transform.position - transform.position;

                // Calculating the dot product of the angle towards the entity and
                // our facing direction gives us if vectors overlaps
                float hitAngle = Vector3.Dot(directionToTarget.normalized, transform.forward);

                // If the angle if less than the Cosine of our cone angle then
                // is not inside
                if (hitAngle < CosAngle)
                    continue;

                // Target in vision cone, check if actually looking or behind something
                RaycastHit hit;
                if(Physics.Raycast(transform.position, transform.forward, out hit, _visionRange, _targetMask, QueryTriggerInteraction.Collide))
                {
                    // Target is in vision cone and range, register in memory
                    _memory.AddWorldState(VISION_TAG, true);
                    _memory.AddShortData(VISION_POS_TAG, _targets[i].transform.position);
                    GameDebug.Log($"Looking at {_targets[i].name}");
                }

                _targets[i] = null;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, _hearingRange);
            Gizmos.color = Color.yellow;
            GizmosExtensions.DrawWireArc(transform.position, transform.forward, _visionCone, _visionRange, 2);
        }
    }
}
