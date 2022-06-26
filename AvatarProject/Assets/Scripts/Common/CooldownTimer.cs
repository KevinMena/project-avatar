using System;

namespace AvatarBA.Common
{    
    public class CooldownTimer 
    {
        private float _totalTime;
        private float _remainingTime;

        public float TotalTime { get => _totalTime; private set => _totalTime = value; }
        public float RemainingTime { get => _remainingTime; private set => _remainingTime = value; }

        public float TimeElapsed => TotalTime - RemainingTime;
        public float PercentElapsed => TimeElapsed / TotalTime;
        public bool IsComplete => RemainingTime < 0;

        public event Action OnTimerStarted;
        public event Action OnTimerCompleted;

        public CooldownTimer(float totalTime)
        {
            TotalTime = totalTime;
            RemainingTime = TotalTime;
        }

        public void Start()
        {
            RemainingTime = TotalTime;
            OnTimerStarted?.Invoke();
        }

        public void Update(float deltaTime)
        {
            if(RemainingTime > 0)
            {
                RemainingTime -= deltaTime;

                if(RemainingTime <= 0)
                    OnTimerCompleted?.Invoke();
            }
        }
    }
}
