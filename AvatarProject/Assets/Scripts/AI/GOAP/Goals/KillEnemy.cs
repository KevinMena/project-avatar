using UnityEngine;

using AvatarBA.AI.Core;

namespace AvatarBA.AI.Goals
{
    [CreateAssetMenu(fileName = "AI_Goal_KillEnemy", menuName = "AI/GOAP/Goals/KillEnemy")]
    public class KillEnemy : Goal
    {
        public override int CalculatePriority()
        {
            return 1;
        }

        public override bool IsValid(GameObject agent)
        {
            if (agent.TryGetComponent(out GOAPMemory agentMemory))
            {
                return agentMemory.CheckWorldState(new WorldState("TargetInHearingRange", true));
            }

            return false;
        }
    }
}