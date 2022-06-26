using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AvatarBA
{
    public class SingleTargetAbility : Ability
    {
        private float _range;
        private GameObject _target;

        protected virtual GameObject GetTarget()
        {
            return new GameObject();
        }
    }
}