using UnityEngine;

namespace AvatarBA
{
    public class ContextManager : MonoBehaviour
    {
        [SerializeField]
        private InputManager _inputManager = default;

        private void Start() 
        {
            _inputManager?.EnableInput();
        }

        private void OnDestroy()
        {
            _inputManager?.DisableInput();
        }
    }
}
