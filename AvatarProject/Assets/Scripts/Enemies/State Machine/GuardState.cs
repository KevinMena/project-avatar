using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using AvatarBA.Common;

namespace AvatarBA.AI.States
{
    public class GuardState : BaseState
    {
        private float _guardTime;
        private Timer _timer;

        public GuardState(float guardTime)
        {
            _guardTime = guardTime;
        }

        public override void Setup(GameObject owner)
        {
            _timer = new Timer(_guardTime);
        }

        public override void OnEnter()
        {
            _timer.Start();
        }

        public override void OnUpdate()
        {
            _timer.Update(Time.deltaTime);

            if(_timer.IsComplete)
            {
                _completed = true;
            }
        }

        public override void OnExit()
        {
            _completed = false;
        }
    }
}
