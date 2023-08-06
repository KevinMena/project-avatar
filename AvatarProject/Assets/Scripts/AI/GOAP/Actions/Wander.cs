using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using AvatarBA.Managers;

namespace AvatarBA.AI.GOAP.Actions
{
    [CreateAssetMenu(fileName = "AI_Action_Wander", menuName = "AI/GOAP/Actions/Wander")]
    public class Wander : Action
    {
        [SerializeField]
        private float _speed = -1;
        [SerializeField]
        private float _directionChangeInterval = 1f;
        [SerializeField]
        private float _wanderRadius = 3f;
        [SerializeField]
        private float _maxRadius = 5f;

        public override bool IsValid()
        {
            return true;
        }

        public override IEnumerator Perform(GameObject agent)
        {
            if (agent.TryGetComponent(out AvatarBA.Core agentCore))
            {
                if(agent.TryGetComponent(out GOAPMemory agentMemory))
                {
                    Vector3 spawnPoint = (Vector3) agentMemory.GetLongData("SpawnPoint");
                    Vector3 destination = agent.transform.position;

                    do
                    {
                        destination = RandomPoint(agent.transform.position);
                    }
                    while (Vector3.Distance(spawnPoint, destination) > _maxRadius);

                    Vector3 offset = agent.transform.position.TargetDirection(destination);
                    float cSquared = offset.Distance();
                    InputState movementState = new InputState();

                    while (cSquared > 0.1f)
                    {
                        movementState.MovementDirection = offset.normalized;
                        movementState.RotationDirection = movementState.MovementDirection;
                        movementState.Speed = -1;

                        agentCore.Movement.UpdateState(movementState);

                        yield return null;
                        offset = agent.transform.position.TargetDirection(destination);
                        cSquared = offset.Distance();
                    }

                    Reset(agentCore, movementState);
                    yield return new WaitForSeconds(_directionChangeInterval);
                }
            }
        }

        private Vector3 RandomPoint(Vector3 agentPosition)
        {
            Vector2 targetPoint = Random.insideUnitCircle * _wanderRadius;
            return new Vector3(targetPoint.x + agentPosition.x,
                                        agentPosition.y,
                                        targetPoint.y + agentPosition.z);
        }

        private void Reset(AvatarBA.Core agent, InputState movementState)
        {
            movementState.MovementDirection = Vector3.zero;
            movementState.RotationDirection = Vector3.zero;
            movementState.Speed = 0;
            agent.Movement.UpdateState(movementState);
        }
    }
}