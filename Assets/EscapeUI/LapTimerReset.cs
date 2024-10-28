using UnityEngine;

public class LapTimerReset : MonoBehaviour
{
    public LapTimer lapTimer;  // Referencia al temporizador de vueltas

    // Método para reiniciar los datos del temporizador
    public void ResetLapData()
    {
        lapTimer.ResetLapData();
    }
}
