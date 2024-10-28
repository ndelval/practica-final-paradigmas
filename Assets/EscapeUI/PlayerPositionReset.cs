using UnityEngine;

public class PlayerPositionReset : MonoBehaviour
{
    public Rigidbody rb;  // Referencia al Rigidbody del coche
    private Vector3 playerStartPosition;
    private Quaternion playerStartRotation;

    void Start()
    {
        // Almacenar la posición inicial del coche
        playerStartPosition = rb.transform.position;
        playerStartRotation = rb.transform.rotation;
    }

    // Método para reiniciar la posición del coche
    public void ResetPlayerPosition()
    {
        rb.transform.position = playerStartPosition;
        rb.transform.rotation = playerStartRotation;
        rb.velocity = Vector3.zero;  // Resetear la velocidad
        rb.angularVelocity = Vector3.zero;  // Resetear la rotación angular
    }
}
