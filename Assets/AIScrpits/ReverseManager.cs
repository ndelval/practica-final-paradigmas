using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReverseManager
{
    private AICarController carController;
    private float reverseTimer = 2f;
    private bool isReversing;

    public ReverseManager(AICarController carController)
    {
        this.carController = carController;
    }

    public bool IsReversing => isReversing;

    public void CheckIfStuck()
    {
        // Check for obstacles in front
        bool carAheadCenter, carAheadLeft, carAheadRight;
        bool obstacleDetected = carController.sensorManager.CheckFrontAllObstacle(out carAheadCenter, out carAheadLeft, out carAheadRight);

        // Only activate reverse if not moving, has started moving, and detects an obstacle
        if (carController.currentSpeedKmh < 0.1f && !isReversing && carController.hasStartedMoving && obstacleDetected)
        {
            isReversing = true;
            reverseTimer = 2f; // Set the reverse duration
            Debug.Log("Activando reversa para desatascar el coche.");
        }
        else if (isReversing)
        {
            // Count down the reverse timer
            reverseTimer -= Time.deltaTime;

            // If the reverse timer has finished, stop reversing
            if (reverseTimer <= 0f)
            {
                isReversing = false;
                Debug.Log("Desactivando reversa y retomando camino.");
            }
            else
            {
                PerformReverseMovement();
            }
        }
    }

    public void PerformReverseMovement()
    {
        Vector3 relativeVector = carController.transform.InverseTransformPoint(carController.nodes[carController.currentNode].position);
        float targetSteer = (relativeVector.x / relativeVector.magnitude) * carController.carSetup.maxSteerAngle;

        // Invert the steering angle for reverse movement
        carController.targetSteerAngle = Mathf.Lerp(carController.targetSteerAngle, -targetSteer, Time.deltaTime * 2f);
        carController.horizontalInput = carController.targetSteerAngle / carController.carSetup.maxSteerAngle;
        carController.verticalInput = -1f;

        // Apply the steering angle to the wheels
        carController.carSetup.frontLeftWheelCollider.steerAngle = carController.targetSteerAngle;
        carController.carSetup.frontRightWheelCollider.steerAngle = carController.targetSteerAngle;
    }
}