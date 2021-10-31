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

        public override void Trigger()
        {
            if(state == AbilityState.cooldown)
                return;
            
            Debug.Log("This is a sample of ability");
        }
    }
}
