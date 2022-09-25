using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AvatarBA.AI.UtilityAI.Actions
{
    [CreateAssetMenu(fileName ="AI_Action_Seek", menuName ="AI/Actions/Seek")]
    public class Seek : Action
    {
        public override IEnumerator Execute(GameObject owner)
        {
            Debug.Log("Seeking");
            yield return null;
        }
    }
}
