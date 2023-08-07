using System;

namespace AvatarBA.Common
{
    public class Timer
    {
        private float _totalTime;
        private float _remainingTime;
        private bool _running;

        public float TotalTime { get => _totalTime; set => _totalTime = value; }
        public float RemainingTime { get => _remainingTime; private set => _remainingTime = value; }

        public float TimeElapsed => TotalTime - RemainingTime;
        public float PercentElapsed => TimeElapsed / TotalTime;
        public bool IsComplete => RemainingTime < 0;
        public bool Running => _running;

        public event Action OnTimerStarted;
        public event Action OnTimerCompleted;

        public Timer(float totalTime)
        {
            TotalTime = totalTime;
            RemainingTime = TotalTime;
        }

        public void Start()
        {
            RemainingTime = TotalTime;
            OnTimerStarted?.Invoke();
            _running = true;
        }

        public void Update(float deltaTime)
        {
            if (RemainingTime > 0)
            {
                RemainingTime -= deltaTime;

                if (RemainingTime <= 0)
                {
                    OnTimerCompleted?.Invoke();
                    _running = false;
                }
            }
        }
    }
}
