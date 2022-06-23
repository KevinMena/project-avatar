using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AvatarBA
{
    [CreateAssetMenu(fileName ="Sample", menuName ="Abilities/Sample")]
    public class SampleAbility : Ability
    {
        public override void Initialize()
        {
            Debug.Log("Initiated");
        }

        public override IEnumerator Trigger(GameObject owner)
        {
            Debug.Log("This is a sample of ability");
            yield return null;
        }
    }
}
