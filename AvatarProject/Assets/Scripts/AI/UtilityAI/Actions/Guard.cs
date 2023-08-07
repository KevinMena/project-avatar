using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using AvatarBA.AI.Considerations;
using AvatarBA.AI.States;

namespace AvatarBA.AI.Actions
{
    public class Guard : Action
    {
        [SerializeField]
        private float _guardTime;

        [SerializeField]
        private Action _previousAction;

        protected override void Start()
        {
            _considerations = new Consideration[1] { new PreviousAction(_previousAction.Name) };
            _actionState = new GuardState(_guardTime);
            base.Start();
        }
    }
}
