using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class CarCollision : MonoBehaviour
{
    // Definir un evento de colisión
    public static event Action<GameObject, Collision> OnCarCollision;

    private void OnCollisionEnter(Collision collision)
    {
        // Llamar al evento y pasar la colisión
        if (collision.gameObject.GetComponent<CarController>() != null)
        {
            OnCarCollision?.Invoke(gameObject, collision);
        }
    }
}
