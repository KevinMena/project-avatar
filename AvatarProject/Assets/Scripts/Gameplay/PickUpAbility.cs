using UnityEngine;

using AvatarBA.Interfaces;
using AvatarBA.Abilities;

namespace AvatarBA
{
    public class PickUpAbility : MonoBehaviour, IInteractable
    {
        [SerializeField]
        private float _speed;

        [SerializeField]
        private float _range;

        [SerializeField]
        private Ability _holdAbility;

        private void Update()
        {
            Animation();
        }

        private void Animation()
        {
            Vector3 direction = new Vector3(0, Mathf.Sin(_speed * Time.time) * _range, 0) * Time.deltaTime;
            transform.Translate(direction);
        }

        public void Interact(GameObject entity)
        {
            if(entity.TryGetComponent(out PlayerAbilityController abilityController))
            {
                //TODO: Display Overlay to choose which ability slot to replace
                abilityController.ReplaceAbility(AbilitySlot.Left, _holdAbility);
                Disappear();
            }
        }

        private void Disappear()
        {
            Destroy(gameObject);
        }
    }
}
