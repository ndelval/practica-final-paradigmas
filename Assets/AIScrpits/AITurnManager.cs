using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    private AICarController carController;
    private CarSetup carSetup;
    private OvertakeManager overtakeManager;
    private float maxCurveSpeedBase = 90f;
    private float minCurveSpeed = 20f;
    private float maxCurveAngle = 90f;
    public float newSteer;



    public TurnManager(AICarController carController, CarSetup carSetup, OvertakeManager overtakeManager)
    {
        this.carController = carController;
        this.carSetup = carSetup;
        this.overtakeManager = overtakeManager;
    }

    public void ApplySteering(Transform targetNode)
    {
        

        // Check for nearby obstacles using all sensors
        bool hasObstacle;
        overtakeManager.CheckForObstaclesBeforeReturn(out hasObstacle);
        
        if (hasObstacle)
        {
            // Adjust steering to avoid the obstacle
            if (overtakeManager.IsAvoiding)
            {
                if (!overtakeManager.cantMove)
                {
                    newSteer = carSetup.maxSteerAngle * overtakeManager.avoidMultiplier;
                    Steer(newSteer);

                }

            }
            else
            {
                Debug.Log("Esquivando obstaculo");
                
                newSteer = (carController.transform.position.x > 0 ? -0.1f : 0.1f) * carSetup.maxSteerAngle;
                Steer(newSteer);
            }
        }
        else
        {
            if (overtakeManager.IsAvoiding)
            {
                newSteer = carSetup.maxSteerAngle * overtakeManager.avoidMultiplier;
                Steer(newSteer);
            }
            else
            {
                
                Vector3 relativeVector = carController.transform.InverseTransformPoint(targetNode.position);
                float targetSteer = (relativeVector.x / relativeVector.magnitude) * carSetup.maxSteerAngle;
                newSteer = Mathf.Lerp(carController.targetSteerAngle, targetSteer, 0.2f);
                Steer(newSteer);
            }
        }

        
    }
    public void Steer(float newSteer)
    {
        
        carController.targetSteerAngle = newSteer;
        carController.horizontalInput = newSteer / carSetup.maxSteerAngle;
        // Apply the steering angle to the wheels
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
                float brakeFactor = Mathf.InverseLerp(0, maxCurveAngle, turnAngle); // Proporcional a la curva
                carController.verticalInput = Mathf.Lerp(0.8f, 1f, brakeFactor); // Rango ajustado de frenado
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
        return Mathf.Lerp(minCurveSpeed, maxCurveSpeedBase, speedFactor * speedFactor );
    }


}

