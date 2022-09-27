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

        private GameObject _owner;

        private Rigidbody rb;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        private void Update()
        {
            Move();
        }

        public void Setup(float damage, GameObject owner)
        {
            _baseDamage = damage;
            _owner = owner;
        }

        public void Move()
        {
            Vector3 desiredVelocity = Vector3.forward * _speed * Time.deltaTime;
            transform.Translate(desiredVelocity);
        }

        private void Destroy()
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

            Destroy();
        }
    }
}
