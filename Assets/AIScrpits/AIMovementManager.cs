using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementManager : MonoBehaviour
{
    private AICarController carController;
    private CarSetup carSetup;

    public MovementManager(AICarController carController, CarSetup carSetup)
    {
        this.carController = carController;
        this.carSetup = carSetup;
    }

    public void ApplyAcceleration()
    {
        // Aplica aceleración solo si el coche no está en reversa
        if (!carController.reversing)
        {
            carController.verticalInput = 1f;
        }
    }

    public void CheckWaypointDistance(List<Transform> nodes, ref int currentNode)
    {
        // Calcula la distancia al nodo actual y avanza al siguiente cuando se alcanza
        float dynamicDistanceThreshold = Mathf.Lerp(3f, 8f, carController.currentSpeedKmh / carSetup.maxSpeedKmh);
        float distanceToNode = Vector3.Distance(carController.transform.position, nodes[currentNode].position);

        if (distanceToNode < dynamicDistanceThreshold)
        {
            currentNode++;
            if (currentNode >= nodes.Count)
            {
                currentNode = 0; // Vuelve al inicio si se llega al último nodo
            }
        }
    }
}