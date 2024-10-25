using UnityEngine;

public class CarController : MonoBehaviour
{
    protected float horizontalInput, verticalInput;
    private float currentSteerAngle, currentbreakForce;
    protected bool isBreaking;

    private CarSetup carSetup;

    private void Awake()
    {
        // Obtener el componente CarSetup en el mismo GameObject
        carSetup = GetComponent<CarSetup>();
    }

    protected void HandleMotor()
    {
        carSetup.frontLeftWheelCollider.motorTorque = verticalInput * carSetup.motorForce;
        carSetup.frontRightWheelCollider.motorTorque = verticalInput * carSetup.motorForce;
        currentbreakForce = isBreaking ? carSetup.breakForce : 0f;
        ApplyBreaking();
    }

    protected void ApplyBreaking()
    {
        carSetup.frontRightWheelCollider.brakeTorque = currentbreakForce;
        carSetup.frontLeftWheelCollider.brakeTorque = currentbreakForce;
        carSetup.rearLeftWheelCollider.brakeTorque = currentbreakForce;
        carSetup.rearRightWheelCollider.brakeTorque = currentbreakForce;
    }

    protected void HandleSteering()
    {
        currentSteerAngle = carSetup.maxSteerAngle * horizontalInput;
        carSetup.frontLeftWheelCollider.steerAngle = currentSteerAngle;
        carSetup.frontRightWheelCollider.steerAngle = currentSteerAngle;
    }

    protected void UpdateWheels()
    {
        UpdateSingleWheel(carSetup.frontLeftWheelCollider, carSetup.frontLeftWheelTransform);
        UpdateSingleWheel(carSetup.frontRightWheelCollider, carSetup.frontRightWheelTransform);
        UpdateSingleWheel(carSetup.rearRightWheelCollider, carSetup.rearRightWheelTransform);
        UpdateSingleWheel(carSetup.rearLeftWheelCollider, carSetup.rearLeftWheelTransform);
    }

    protected void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.rotation = rot;
        wheelTransform.position = pos;
    }
}
