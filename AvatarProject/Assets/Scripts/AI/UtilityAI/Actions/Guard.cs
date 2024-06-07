using UnityEngine;

using AvatarBA.AI.Considerations;
using AvatarBA.Common;

namespace AvatarBA.AI.Actions
{
    public class Guard : Action
    {
        [SerializeField]
        private float _guardTime;

        [SerializeField]
        private Action _previousAction;

        private Timer _timer;

        protected override void Start()
        {
            _considerations = new Consideration[1] { new PreviousAction(_previousAction.Name) };
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
