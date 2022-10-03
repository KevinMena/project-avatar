using UnityEngine;

namespace AvatarBA.Combat
{
    public class PushBoxAreaAttack : BoxAreaAttack
    {
        private float _pushBack;
        private Vector3 _pushBackDirection;

        public void Setup(string name, Vector3 size, float distance, float damage, float pushBack, LayerMask mask, GameObject owner)
        {
            _pushBack = pushBack;
            _pushBackDirection = owner.transform.forward;
            Setup(name, size, distance, damage, mask, owner);
        }

        protected override void HitEntity(Collider hit)
        {
            if (hit.TryGetComponent(out CharacterMovementController characterMovementController))
            {
                Vector3 velocity = _pushBackDirection * _pushBack;
                characterMovementController.AddForce(velocity, ForceMode.Force);
            }

            base.HitEntity(hit);
        }
    }
}