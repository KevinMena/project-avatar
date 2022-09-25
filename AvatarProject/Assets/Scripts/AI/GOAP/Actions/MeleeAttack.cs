using AvatarBA.Debugging;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using AvatarBA.AI.Core;

namespace AvatarBA.AI.Actions
{
    [CreateAssetMenu(fileName = "AI_Action_MeleeAttack", menuName = "AI/GOAP/Actions/MeleeAttack")]
    public class MeleeAttack : Action
    {
        public override bool IsValid()
        {
            return true;
        }

        public override IEnumerator Perform(GameObject agent)
        {
            GameDebug.Log("Attacking melee");
            yield return null;
        }
    }
}
