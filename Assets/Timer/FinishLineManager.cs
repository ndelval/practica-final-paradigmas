using UnityEngine;

public class FinishLine : MonoBehaviour
{
    public LapTimer lapTimer;
    private float lastLapTime = 0f;  
    public float lapCooldown = 2f;   

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            
            if (Time.time - lastLapTime >= lapCooldown)
            {
                
                if (!lapTimer.isTiming)
                {
                    lapTimer.StartTimer();
                }
                else
                {
                    lapTimer.EndLap();
                }

                
                lastLapTime = Time.time;
            }
        }
    }
}
