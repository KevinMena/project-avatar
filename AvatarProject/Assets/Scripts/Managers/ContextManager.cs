using UnityEngine;

namespace AvatarBA
{
    public class ContextManager : MonoBehaviour
    {
        [SerializeField]
        private InputManager _inputManager = default;

        private void Awake() 
        {
            _inputManager?.EnableInput();
        }

        private void OnDisable()
        {
            _inputManager?.DisableInput();
        }
    }
}
