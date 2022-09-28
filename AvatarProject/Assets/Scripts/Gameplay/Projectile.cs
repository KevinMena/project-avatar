using AvatarBA.Debugging;
using UnityEngine;

namespace AvatarBA.Combat
{
    [RequireComponent(typeof(Rigidbody))]
    public class Projectile : MonoBehaviour
    {
        [SerializeField]
        private LayerMask _mask;

        [SerializeField]
        private float _baseDamage = 0;

        [SerializeField]
        private float _speed = 0;

        [SerializeField]
        private float _lifeTime = 0;

        private GameObject _owner;

        private Rigidbody rb;

        private float _lifeTimer = 0;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        private void Update()
        {
            _lifeTime -= Time.deltaTime;
            if (_lifeTime <= 0)
                Explode();

            Move();
        }

        public void Setup(float damage, GameObject owner)
        {
            _baseDamage = damage;
            _owner = owner;
            _lifeTimer = _lifeTime;
        }

        public void Move()
        {
            Vector3 desiredVelocity = Vector3.forward * _speed * Time.deltaTime;
            transform.Translate(desiredVelocity);
        }

        private void Explode()
        {
            // Play VFX
            Destroy(gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
            GameObject entity = other.gameObject;

            if (entity == _owner)
                return;

            if (entity != null && entity.layer == _mask)
            {
                if (entity.TryGetComponent(out Character character))
                {
                    character.DoHit(_baseDamage, entity.transform.position, entity.transform.position - transform.position);
                }
            }

            Explode();
        }
    }
}
