using AvatarBA.Debugging;
using AvatarBA.Interfaces;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;
using UnityEngine.UI;

namespace AvatarBA.Combat
{
    [RequireComponent(typeof(Rigidbody))]
    public class Projectile : MonoBehaviour, IProjectile
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

        public LayerMask Mask => _mask;
        public float BaseDamage => _baseDamage;
        public float Speed => _speed;
        public float LifeTime => _lifeTime;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        private void Update()
        {
            _lifeTimer -= Time.deltaTime;
            if (_lifeTimer <= 0)
                Explode();

            Move();
        }

        public void Setup(float damage, LayerMask mask, GameObject owner)
        {
            _baseDamage = damage;
            _mask = mask;
            _owner = owner;
            _lifeTimer = _lifeTime;
        }

        public void Move()
        {
            Vector3 desiredVelocity = Vector3.forward * _speed * Time.deltaTime;
            transform.Translate(desiredVelocity);
        }

        public void Explode()
        {
            // Play VFX
            Destroy(gameObject);
        }

        public void OnEntityHit(Collider hit)
        {
            GameObject entity = hit.gameObject;
            if (entity == _owner)
                return;

            if (entity is not null && (_mask & (1 << entity.layer)) != 0)
            {
                GameDebug.Log($"Collisioned with {hit.name} using projectile");
                if (entity.TryGetComponent(out IDamageable damageable))
                {
                    damageable.TakeHit(_baseDamage, entity.transform.position - transform.position);
                }
            }

            Explode();
        }

        private void OnTriggerEnter(Collider other)
        {
            OnEntityHit(other);
        }
    }
}
