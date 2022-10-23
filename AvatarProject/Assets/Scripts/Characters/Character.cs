using System.Collections;
using UnityEngine;

using AvatarBA.Interfaces;

namespace AvatarBA
{
    public class Character : MonoBehaviour, IDamageable
    {
        [Header("References")]
        [SerializeField]
        private Transform _shootPosition;

        [SerializeField]
        protected float _invulnerableDuration = 0.5f;

        [SerializeField]
        protected float _invulnerableDelta = 0.15f;

        protected bool _isInvulnerable = false;

        public Transform ShootPosition => _shootPosition;

        public virtual void TakeDamage(float damage) { }

        public virtual void TakeHit(float damage, Vector2 hitDirection) { }

        public void BecomeInvulnerable()
        {
            if(!_isInvulnerable)
                StartCoroutine(Invulnerable());
        }

        protected IEnumerator Invulnerable()
        {
            _isInvulnerable = true;

            for(float i = 0; i < _invulnerableDuration; i += _invulnerableDelta)
            {
                yield return new WaitForSeconds(_invulnerableDelta);
            }

            _isInvulnerable = false;
        }
    }
}
