using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingCarController : CarControllerBase
{

    

    public void Jump()
    {
        if (IsCarGrounded()) // Verifica si el coche está en el suelo
        {
            Debug.Log("Jump");

            // Aplica la fuerza de salto
            rb.AddForce(transform.up * carSetup.jumpForce, ForceMode.Impulse);
        }
    }


    private bool IsCarGrounded()
    {
        // Distancia mínima para detectar el suelo desde el centro del Rigidbody
        float rayLength = 0.1f; // Ajusta según sea necesario

        // Realiza un raycast desde el centro del Rigidbody hacia abajo
        bool isGrounded = RaycastFromRigidbody(rayLength);

        return isGrounded;
    }

    private bool RaycastFromRigidbody(float rayLength)
    {
        // Posición del Rigidbody (su centro de masa)
        Vector3 rbPosition = rb.position;

        // Realiza un raycast desde el centro del Rigidbody, buscando el suelo
        Ray ray = new Ray(rbPosition, Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, rayLength))
        {
            Debug.DrawRay(rbPosition, Vector3.down * rayLength, Color.green);  // Dibuja el raycast en la escena
            return true;  // Si el rayo golpea algo, estamos tocando el suelo
        }

        Debug.DrawRay(rbPosition, Vector3.down * rayLength, Color.red);  // Dibuja el rayo en rojo si no golpea nada
        return false;
    }

    protected void HandleJump()
    {
        // Limitación de la rotación cuando el coche está en el aire
        if (!IsCarGrounded())
        {
            // Evitar que el coche gire en los ejes X y Z en el aire

            // También, si el coche está saltando, puedes amortiguar el ángulo de inclinación
            Vector3 currentRotation = transform.eulerAngles;
            currentRotation.x = Mathf.LerpAngle(currentRotation.x, 0, Time.deltaTime * 5f); // Reducir la rotación en X
            currentRotation.z = Mathf.LerpAngle(currentRotation.z, 0, Time.deltaTime * 5f); // Reducir la rotación en Z
            transform.eulerAngles = currentRotation;
        }
    }
}









