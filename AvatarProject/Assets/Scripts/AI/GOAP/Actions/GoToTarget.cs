using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using AvatarBA.AI.Core;
using AvatarBA.Debugging;

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
            GameDebug.Log("Am going to target");
            yield return null;
        }
    }
}