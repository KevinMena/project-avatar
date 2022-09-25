using AvatarBA.Debugging;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using AvatarBA.AI.Core;

namespace AvatarBA.AI.Actions
{
    [CreateAssetMenu(fileName = "AI_Action_RangeAttack", menuName = "AI/GOAP/Actions/RangeAttack")]
    public class RangeAttack : Action
    {
        public override bool IsValid()
        {
            return true;
        }

        public override IEnumerator Perform(GameObject agent)
        {
            GameDebug.Log("Attacking range");
            yield return null;
        }
    }
}
