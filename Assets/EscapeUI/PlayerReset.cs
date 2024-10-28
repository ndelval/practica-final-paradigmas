using UnityEngine;

public class PlayerReset : MonoBehaviour
{
    public PlayerPositionReset positionReset;  // Nueva referencia a PlayerPositionReset
    public LapTimerReset lapTimerReset;  // Nueva referencia a LapTimerReset

    // Método para reiniciar todo el estado del jugador
    public void ResetPlayer()
    {
        positionReset.ResetPlayerPosition();  // Reiniciar la posición del coche
        lapTimerReset.ResetLapData();  // Reiniciar los datos del temporizador de vueltas
    }
}
