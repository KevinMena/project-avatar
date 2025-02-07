using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AvatarBA
{    
    public class AbilityHolder : MonoBehaviour
    {
        public Ability sampleAbility;

        public void UseAbility()
        {
            sampleAbility.Perform();
            StartCoroutine(sampleAbility.CooldownCountdown());
        }
    }
}
