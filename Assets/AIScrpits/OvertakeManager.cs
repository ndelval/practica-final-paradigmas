using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OvertakeManager
{
    private AICarController carController;
    private SensorManager sensorManager;
    private TurnManager turnManager;
    private int overtakingSide = 0;

    public float avoidMultiplier { get; private set; }
    public bool IsAvoiding { get; private set; }
    private bool isPassingComplete = false;
    public bool cantMove { get; private set; }
    private Transform targetCar; // Reference to the car we are overtaking
    private bool continua;

    public OvertakeManager(AICarController carController, SensorManager sensorManager)
    {
        this.carController = carController;
        this.sensorManager = sensorManager;
        
    }

    public void AttemptOvertake()
    {
        IsAvoiding = false;
        cantMove = false;
        continua = false;


        // Car detection
        bool carAheadCenter, carAheadLeft, carAheadRight;
        sensorManager.CheckFrontObstacle(out carAheadCenter, out carAheadLeft, out carAheadRight);

        // Side clearance check
        bool rightSideClear, leftSideClear;
        sensorManager.CheckSideClearance(out rightSideClear, out leftSideClear);

        // Angular clearance check
        bool rightAngularClear, leftAngularClear;
        sensorManager.CheckAngularClearance(out rightAngularClear, out leftAngularClear);

        if (!isPassingComplete)
        {
            if (overtakingSide == 0) // Decide the side if not currently overtaking
            {
                if (carAheadCenter)
                {
                    if ((carAheadLeft && rightSideClear)) // || (carAheadRight && rightSideClear)
                    {
                        
                        StartOvertaking(1); // Overtake to the right
                    }
                    else if ((carAheadRight && leftSideClear)) // || (carAheadLeft && leftSideClear)
                    {
                        
                        StartOvertaking(-1); // Overtake to the left
                    }
                    else if (!rightSideClear && !leftSideClear)
                    {
                        
                        AlignBehindCar(sensorManager.hit.transform);
                    }
                }
                else if (!rightAngularClear)
                {
                    StartOvertaking(-0.5f);
                }
                else if (!leftAngularClear)
                {
                    StartOvertaking(0.5f);
                }
            }
            else // Already overtaking
            {
                ContinueOvertaking(rightAngularClear, leftAngularClear);
            }
        }
        else // If overtaking is complete, check if we can return to the path
        {
            bool hasObstacle;
            CheckForObstaclesBeforeReturn(out hasObstacle);

            if (!hasObstacle) // Check if no obstacles are detected
            {
                ResetOvertake(); // Return to the main path
            }
        }
    }

    public void HandleLateralCollisionAdjustment()
    {
        bool leftCollision, rightCollision;
        if (sensorManager.CheckLateralCollision(out leftCollision, out rightCollision))
        {
            float adjustmentAngle = 2.5f; // Ángulo de ajuste para separarse del otro coche

            if (rightCollision)
            {
                // Ajuste hacia la izquierda
                carController.targetSteerAngle -= adjustmentAngle;
            }
            else if (leftCollision)
            {
                // Ajuste hacia la derecha
                carController.targetSteerAngle += adjustmentAngle;
            }

            // Aplicar el nuevo ángulo de dirección
            turnManager.Steer(carController.targetSteerAngle);
        }
    }

    private void StartOvertaking(float side)
    {
        cantMove = false;
        avoidMultiplier = side * 0.5f; // Move left or right
        IsAvoiding = true;
        targetCar = sensorManager.hit.transform; // Store the overtaken car

        // Draw a ray towards the target car
        Vector3 directionToTarget = targetCar.position - carController.transform.position;
        Debug.DrawLine(carController.transform.position, targetCar.position, Color.red, 2f); // Red ray for 2 seconds

        // Calculate the angle to the target car
        float angleToTarget = Vector3.Angle(carController.transform.forward, directionToTarget);

        // Adjust the steering input based on the angle to the target car
        if (angleToTarget > 30f) // Continue steering if the angle is greater than 30 degrees
        {
            carController.horizontalInput = 0f;
            continua = false;
        }
        else
        {
            continua = true;
        }

    }


    private void ContinueOvertaking(bool rightAngularClear, bool leftAngularClear)
    {
        // Continue overtaking while detecting the car
        if ((overtakingSide == 1 && (!rightAngularClear || continua || !sensorManager.CheckSideClearance(out _, out _)))
            || (overtakingSide == -1 && (!leftAngularClear || continua || !sensorManager.CheckSideClearance(out _, out _))))
        {
            IsAvoiding = true; // Maintain the turn
        }
        else
        {
            isPassingComplete = true; // Overtaking completed
        }
    }

    public void CheckForObstaclesBeforeReturn(out bool hasObstacle)
    {
        bool carAheadCenter, carAheadLeft, carAheadRight;
        bool rightSideClear, leftSideClear;
        

        // Check all sensors
        sensorManager.CheckFrontAllObstacle(out carAheadCenter, out carAheadLeft, out carAheadRight);
        sensorManager.CheckSideClearance(out rightSideClear, out leftSideClear);
        

        // Determine if there is an obstacle detected
        hasObstacle = carAheadCenter || carAheadLeft || carAheadRight || !rightSideClear || !leftSideClear;
    }

    private void ResetOvertake()
    {
        overtakingSide = 0;
        avoidMultiplier = 0;
        IsAvoiding = false;
        isPassingComplete = false;
        targetCar = null; // Reset reference to the overtaken car
    }

    private void AlignBehindCar(Transform obstacle)
    {
        cantMove = true;
        Vector3 directionToObstacle = obstacle.position - carController.transform.position;
        directionToObstacle.y = 0;

        Quaternion targetRotation = Quaternion.LookRotation(directionToObstacle);
        carController.transform.rotation = Quaternion.Slerp(carController.transform.rotation, targetRotation, Time.deltaTime * carController.turnSpeed);
        carController.verticalInput = Mathf.Clamp(directionToObstacle.magnitude / sensorManager.sensorLength, 0.1f, 1f);

        avoidMultiplier = 0;
        IsAvoiding = true;
    }
}
