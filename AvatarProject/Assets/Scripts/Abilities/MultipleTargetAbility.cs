using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AvatarBA
{
    public class MultipleTargetAbility : Ability
    {
        private float _range;
        private GameObject[] _targets;

        protected virtual GameObject[] GetTargets()
        {
            return new List<GameObject>().ToArray();
        }
    }
}