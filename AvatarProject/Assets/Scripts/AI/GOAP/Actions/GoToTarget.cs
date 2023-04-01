using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using AvatarBA.AI.Core;
using AvatarBA.Debugging;
using AvatarBA.Managers;

namespace AvatarBA.AI.Actions
{
    [CreateAssetMenu(fileName = "AI_Action_GoToTarget", menuName = "AI/GOAP/Actions/GoToTarget")]
    public class GoToTarget : Action
    {
        public override bool IsValid()
        {
            return true;
        }

        public override IEnumerator Perform(GameObject agent)
        {
            if (agent.TryGetComponent(out AvatarBA.Core agentCore))
            {
                Vector3 destination = agent.transform.position;

                if(agent.TryGetComponent(out GOAPMemory agentMemory))
                {
                    // Check if target in hearing range
                    var hearingPos = agentMemory.GetShortData("HearingTargetAt");

                    if(hearingPos is not null)
                        destination = (Vector3) hearingPos;

                    var visionPos = agentMemory.GetShortData("InVisionTargetAt");

                    if (visionPos is not null)
                        destination = (Vector3) visionPos;
                }

                // Move towards the destination
                Vector3 offset = agent.transform.position.TargetDirection(destination);
                float cSquared = offset.Distance();
                InputState movementState = new InputState();

                while (cSquared > 0.4f)
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
            }
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