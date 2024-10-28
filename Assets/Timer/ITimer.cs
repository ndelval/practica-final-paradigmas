using System;

namespace Application
{
    public interface IRaceTimer
    {
        void StartTimer();
        void StopTimer();
        string GetFormattedTime();
    }
}
