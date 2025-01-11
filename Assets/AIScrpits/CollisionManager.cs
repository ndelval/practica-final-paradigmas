using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionManager
{
    private AICarController carController;

    public CollisionManager(AICarController carController)
    {
        this.carController = carController;
    }

    public void HandleCollision(GameObject collidingCar)
    {
        if (collidingCar != carController.gameObject)
        {
            Vector3 directionFromPlayer = carController.transform.position - collidingCar.transform.position;
            directionFromPlayer.Normalize();
            carController.rb.AddForce(directionFromPlayer * carController.collisionForce + Vector3.up * carController.collisionForce / 5, ForceMode.Impulse);
            Debug.Log($"Colisión detectada entre {collidingCar.name} y {carController.gameObject.name}");
        }
    }
}