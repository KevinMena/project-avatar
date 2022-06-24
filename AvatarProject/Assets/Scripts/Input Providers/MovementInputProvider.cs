using UnityEngine;

namespace AvatarBA
{
    [CreateAssetMenu(fileName ="InputProvider_Movement_", menuName ="Providers/Movement Input Provider")]
    public class MovementInputProvider : IInputProvider
    {   
        public override InputState GetState()
        {
            currentState = new InputState();

            foreach (var middleware in middlewares)
            {
                middleware.Process(ref currentState);
            }
            
            return currentState;
        }
    }
    
}
