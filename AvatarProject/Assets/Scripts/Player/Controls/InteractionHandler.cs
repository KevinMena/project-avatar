using UnityEngine;
using AvatarBA.Interfaces;
using AvatarBA.Debugging;

namespace AvatarBA
{
    public class InteractionHandler : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private InputManager _inputManager;

        [Header("Values")]
        [SerializeField]
        private LayerMask _layerMask;

        [SerializeField]
        private float _range;

        [SerializeField]
        private float _radius;

        public void Awake()
        {
            _inputManager.InteractEvent += OnInteract;
        }

        private void OnDisable()
        {
            _inputManager.InteractEvent -= OnInteract;
        }

        private void OnInteract()
        {
            GameDebug.Log("Interacting");
            Collider[] colliders = new Collider[5];
            Vector3 position = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z) + transform.forward * _range;
            if(Physics.OverlapSphereNonAlloc(position, _radius, colliders, _layerMask) > 0)
            {
                Collider closest = colliders[0];
                float distance = float.MaxValue;
                for(int i = 0; i < colliders.Length; i++)
                {
                    if(colliders[i] != null && Vector3.Distance(transform.position, colliders[i].transform.position) < distance)
                        closest = colliders[i];
                }

                if(closest.gameObject.TryGetComponent(out IInteractable interactable))
                {
                    interactable.Interact(gameObject);
                }
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(new Vector3(transform.position.x, transform.position.y + 1, transform.position.z) + transform.forward * _range, _radius);
        }
    }
}
