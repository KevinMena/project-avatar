using UnityEngine;

using AvatarBA.AI.States;
using AvatarBA.AI.Considerations;

namespace AvatarBA.AI.Actions
{
    public class Wander : Action
    {
        [SerializeField]
        private float _maxDistance = 0;

        [SerializeField]
        private float _wanderDistance = 0;

        protected override void Start()
        {
            _considerations = new Consideration[1] { new TargetNotInRange() };
            _actionState = new WanderState(_maxDistance, _wanderDistance);
            base.Start();
        }
    }
}