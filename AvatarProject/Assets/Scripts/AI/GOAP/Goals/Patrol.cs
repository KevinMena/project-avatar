using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AvatarBA.AI.GOAP.Goals
{
    [CreateAssetMenu(fileName = "AI_Goal_Patrol", menuName = "AI/GOAP/Goals/Patrol")]
    public class Patrol : Goal
    {
        public override int CalculatePriority()
        {
            return 5;
        }

        public override bool IsValid(GameObject agent)
        {
            return true;
        }
    }
}