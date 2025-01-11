using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    private AICarController carController;
    private CarSetup carSetup;
    private OvertakeManager overtakeManager;
    private ISteeringStrategy steeringStrategy;
    private float maxCurveSpeedBase = 90f;
    private float minCurveSpeed = 20f;
    private float maxCurveAngle = 90f;

    public TurnManager(AICarController carController, CarSetup carSetup, OvertakeManager overtakeManager)
    {
        this.carController = carController;
        this.carSetup = carSetup;
        this.overtakeManager = overtakeManager;
        this.steeringStrategy = new NormalSteeringStrategy(); // Estrategia por defecto
    }

    public void ApplySteering(Transform targetNode)
    {
        bool hasObstacle;
        overtakeManager.CheckForObstaclesBeforeReturn(out hasObstacle);

        if (hasObstacle)
        {
            if (overtakeManager.IsAvoiding)
            {
                if (!overtakeManager.cantMove)
                {
                    SetSteeringStrategy(new AvoidObstacleSteeringStrategy());
                }
            }
            else
            {
                Debug.Log("Esquivando obstaculo");
                SetSteeringStrategy(new AvoidObstacleSteeringStrategy());
            }
        }
        else
        {
            if (overtakeManager.IsAvoiding)
            {
                SetSteeringStrategy(new AvoidObstacleSteeringStrategy());
            }
            else
            {
                SetSteeringStrategy(new NormalSteeringStrategy());
            }
        }

        steeringStrategy.Steer(carController, targetNode);
    }

    public void SetSteeringStrategy(ISteeringStrategy strategy)
    {
        this.steeringStrategy = strategy;
    }

    public void Steer(float newSteer)
    {
        carController.targetSteerAngle = newSteer;
        carController.horizontalInput = newSteer / carSetup.maxSteerAngle;
        carSetup.frontLeftWheelCollider.steerAngle = carController.targetSteerAngle;
        carSetup.frontRightWheelCollider.steerAngle = carController.targetSteerAngle;
    }

    public void CheckForSharpTurns()
    {
        float currentSpeedKmh = carController.rb.velocity.magnitude * 3.6f;
        int lookaheadNodes = DetermineLookaheadNodes(currentSpeedKmh);

        if (lookaheadNodes > 1)
        {
            Vector3 currentDirection = carController.transform.forward.normalized;
            Vector3 nextNodeDirection = (carController.nodes[carController.currentNode + lookaheadNodes].position - carController.nodes[carController.currentNode].position).normalized;

            float turnAngle = Vector3.Angle(currentDirection, nextNodeDirection);
            float adjustedMaxCurveSpeed = CalculateMaxSpeedForAngle(turnAngle);

            if (currentSpeedKmh > adjustedMaxCurveSpeed)
            {
                float brakeFactor = Mathf.InverseLerp(0, maxCurveAngle, turnAngle);
                carController.verticalInput = Mathf.Lerp(0.8f, 1f, brakeFactor);
                carController.isBreaking = true;
            }
            else
            {
                carController.verticalInput = 1f;
                carController.isBreaking = false;
            }
        }
    }

    private int DetermineLookaheadNodes(float currentSpeedKmh)
    {
        int baseLookahead = 2;
        int maxLookahead = 5;
        int dynamicLookahead = baseLookahead + Mathf.FloorToInt(currentSpeedKmh / 50f);
        return Mathf.Clamp(dynamicLookahead, baseLookahead, maxLookahead);
    }

    private float CalculateMaxSpeedForAngle(float curveAngle)
    {
        float speedFactor = Mathf.Clamp01(1 - (curveAngle / maxCurveAngle));
        return Mathf.Lerp(minCurveSpeed, maxCurveSpeedBase, speedFactor * speedFactor);
    }

    public float GetCurrentSteerAngle()
    {
        return carController.targetSteerAngle;
    }
}