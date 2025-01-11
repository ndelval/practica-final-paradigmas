using UnityEngine;

public interface ISteeringStrategy
{
    void Steer(AICarController carController, Transform targetNode);
}


public class NormalSteeringStrategy : ISteeringStrategy
{
    public void Steer(AICarController carController, Transform targetNode)
    {
        Vector3 relativeVector = carController.transform.InverseTransformPoint(targetNode.position);
        float targetSteer = (relativeVector.x / relativeVector.magnitude) * carController.carSetup.maxSteerAngle;
        float newSteer = Mathf.Lerp(carController.targetSteerAngle, targetSteer, 0.2f);
        carController.targetSteerAngle = newSteer;
        carController.horizontalInput = newSteer / carController.carSetup.maxSteerAngle;
        carController.carSetup.frontLeftWheelCollider.steerAngle = carController.targetSteerAngle;
        carController.carSetup.frontRightWheelCollider.steerAngle = carController.targetSteerAngle;
    }
}


public class AvoidObstacleSteeringStrategy : ISteeringStrategy
{
    public void Steer(AICarController carController, Transform targetNode)
    {
        float newSteer = carController.carSetup.maxSteerAngle * carController.overtakeManager.avoidMultiplier;
        carController.targetSteerAngle = newSteer;
        carController.horizontalInput = newSteer / carController.carSetup.maxSteerAngle;
        carController.carSetup.frontLeftWheelCollider.steerAngle = carController.targetSteerAngle;
        carController.carSetup.frontRightWheelCollider.steerAngle = carController.targetSteerAngle;
    }
}