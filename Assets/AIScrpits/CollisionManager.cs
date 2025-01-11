using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionManager
{
    private AICarController carController;
    private float collisionFactor;

    public CollisionManager(AICarController carController, float collisionFactor)
    {
        this.carController = carController;
        this.collisionFactor = collisionFactor;
    }

    public void HandleCollision(GameObject collidingCar, float concreteCollisionForce)
    {
        if (collidingCar != carController.gameObject)
        {
            Vector3 directionFromPlayer = carController.transform.position - collidingCar.transform.position;
            directionFromPlayer.Normalize();
            carController.rb.AddForce(directionFromPlayer * collisionFactor * concreteCollisionForce + Vector3.up * collisionFactor * concreteCollisionForce / 5, ForceMode.Impulse);
            Debug.Log($"Colisión detectada entre {collidingCar.name} y {carController.gameObject.name}");
        }
    }
}