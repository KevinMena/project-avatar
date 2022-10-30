using UnityEngine;
using UnityEngine.Events;

namespace AvatarBA.Managers
{
    [CreateAssetMenu(fileName = "Middleware_HealthDisplay", menuName = "Middlewares/Health Display")]
    public class HealthMiddleware : ScriptableObject
    {
        public event UnityAction<float> OnHealthChange;
        public event UnityAction<float> OnMaximumHealthChange;

        public void Setup(float baseValue)
        {
            OnHealthChange?.Invoke(baseValue);
            OnMaximumHealthChange?.Invoke(baseValue);
        }

        public void UpdateHealth(float value)
        {
            OnHealthChange?.Invoke(value);
        }

        public void UpdateMaxHealth(float value)
        {
            OnMaximumHealthChange?.Invoke(value);
        }
    }
}