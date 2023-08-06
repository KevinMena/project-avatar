using System.Collections;
using UnityEngine;

using AvatarBA.Patterns;

namespace AvatarBA.AI
{
    public class Seek : Action
    {
        public IEnumerator Execute(StateMachine owner)
        {
            Debug.Log("Seeking");
            yield return null;
        }
    }
}
