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

    public void ResetPlayerPosition()
    {
        if (rb == null)
        {
            Debug.LogError("Rigidbody no asignado en PlayerPositionReset");
            return;
        }

        Debug.Log("Restableciendo la posición del coche...");
        rb.transform.position = playerStartPosition;
        rb.transform.rotation = playerStartRotation;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
}
