using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AvatarBA
{ 
    public class PlayerAbilityController : MonoBehaviour
    {
        // What do i want this to do?
        // 1. This has to be the container for all the core abilities of the player
        // Meaning: Dash ability (every character has one, but the effects it gains is different)
        // We have 4 core abilities that the player will be gaining through the stages
        // They will gain new effects, meaning, evolving with player choises
        // For now every character will have 3 base abilities at the beginning of the game and later on 
        // unlock the 4th one that is kinda like an ultimate ability. As said, every one of them will
        // evolve during gameplay or change drastically (another ability)
        // 2. This has to control the usage of every ability: trigger the abilities passing all the information
        // the abilities need and control the cooldown of every single one of them
        // 3. Change/add effect to the abilities or the abilities itself

        // FOR NOW
        [SerializeField]
        private InputManager _inputManager;
        //

        [SerializeField]
        private Ability _dashAbility;

        private AbilityState _dashState;

        [SerializeField]
        private Ability[] _coreAbilities;

        private AbilityState[] _coreStates;

        public void TriggerDash()
        {
            if(_dashState == AbilityState.cooldown || _dashState == AbilityState.active)
                return;
            
            StartCoroutine(_dashAbility.OnCooldown(_dashState));
            StartCoroutine(_dashAbility.Trigger(gameObject));
        }

    }
}