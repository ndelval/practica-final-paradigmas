using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OvertakeManager
{
    public SensorManager sensorManager;
    public AICarController carController;
    private IOvertakeState currentState;

    public float avoidMultiplier { get; private set; }
    public bool IsAvoiding { get; private set; }
    private bool isPassingComplete = false;
    public bool cantMove { get; private set; }
    private Transform targetCar; 
    private bool continua;

    public OvertakeManager(AICarController carController, SensorManager sensorManager)
    {
        this.carController = carController;
        this.sensorManager = sensorManager;
        currentState = new NoOvertakingState();
    }

    public void AttemptOvertake()
    {
        currentState.UpdateState(this);
    }

    public void SetState(IOvertakeState newState)
    {
        currentState.ExitState(this);
        currentState = newState;
        currentState.Enter(this);
    }

    public void StartOvertaking(float side)
    {
        cantMove = false;
        avoidMultiplier = side * 0.5f;
        IsAvoiding = true;
        targetCar = sensorManager.hit.transform;

        Vector3 directionToTarget = targetCar.position - carController.transform.position;
        Debug.DrawLine(carController.transform.position, targetCar.position, Color.red, 2f);

        float angleToTarget = Vector3.Angle(carController.transform.forward, directionToTarget);

        if (angleToTarget > 30f)
        {
            carController.horizontalInput = 0f;
            continua = false;
        }
        else
        {
            continua = true;
        }
    }

    public void ContinueOvertaking(bool rightAngularClear, bool leftAngularClear)
    {
        if ((avoidMultiplier > 0 && !rightAngularClear) || (avoidMultiplier < 0 && !leftAngularClear))
        {
            IsAvoiding = true;
        }
        else
        {
            isPassingComplete = true;
            SetState(new NoOvertakingState());
        }
    }

    public void ResetOvertake()
    {
        avoidMultiplier = 0;
        IsAvoiding = false;
        isPassingComplete = false;
        targetCar = null;
    }

    public void HandleLateralCollisionAdjustment()
    {
        bool leftCollision, rightCollision;
        if (sensorManager.CheckLateralCollision(out leftCollision, out rightCollision))
        {
            float adjustmentAngle = 2.5f;

            if (rightCollision)
            {
                carController.targetSteerAngle -= adjustmentAngle;
            }
            else if (leftCollision)
            {
                carController.targetSteerAngle += adjustmentAngle;
            }

            carController.turnManager.Steer(carController.targetSteerAngle);
        }
    }

    public void CheckForObstaclesBeforeReturn(out bool hasObstacle)
    {
        bool carAheadCenter, carAheadLeft, carAheadRight;
        bool rightSideClear, leftSideClear;

        sensorManager.CheckFrontAllObstacle(out carAheadCenter, out carAheadLeft, out carAheadRight);
        sensorManager.CheckSideClearance(out rightSideClear, out leftSideClear);

        hasObstacle = carAheadCenter || carAheadLeft || carAheadRight || !rightSideClear || !leftSideClear;
    }
}
