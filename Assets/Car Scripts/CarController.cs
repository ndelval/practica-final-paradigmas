using UnityEngine;

public class CarController : MonoBehaviour
{
    public float horizontalInput, verticalInput; // Cambia a public para permitir el acceso desde managers
    public float currentSteerAngle, currentBreakForce;
    public bool isBreaking { get; set; } // Cambia a public
    public Rigidbody rb; // Asegúrate de que sea public para que AICarController y managers puedan acceder

    public CarSetup carSetup;
    public float speed;

    public float nitrusValue;
    public bool nitrusFlag;

    private void Awake()
    {
        carSetup = GetComponent<CarSetup>();
        rb = GetComponent<Rigidbody>();
        nitrusValue = carSetup.initialNitroValue;
    }

    protected void HandleMotor()
    {
        carSetup.frontLeftWheelCollider.motorTorque = verticalInput * carSetup.motorForce;
        carSetup.frontRightWheelCollider.motorTorque = verticalInput * carSetup.motorForce;
        currentBreakForce = isBreaking ? carSetup.breakForce : 0f;
        ApplyBreaking();
    }

    protected void ApplyBreaking()
    {
        carSetup.frontRightWheelCollider.brakeTorque = currentBreakForce;
        carSetup.frontLeftWheelCollider.brakeTorque = currentBreakForce;
        carSetup.rearLeftWheelCollider.brakeTorque = currentBreakForce;
        carSetup.rearRightWheelCollider.brakeTorque = currentBreakForce;
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
    protected void ClampSpeed()
    {
        float speedKmh = rb.velocity.magnitude * 3.6f;
        float maxSpeedMs = carSetup.maxSpeedKmh / 3.6f;

        if (speedKmh > carSetup.maxSpeedKmh)
        {
            rb.velocity = rb.velocity.normalized * maxSpeedMs;
        }

        speed = speedKmh;
    }

    public void activateNitrus()
    {
        if (!nitrusFlag && nitrusValue <= 1)
        {
            nitrusValue += Time.deltaTime / 2;
        }
        else
        {
            nitrusValue -= (nitrusValue <= 0) ? 0 : Time.deltaTime / 3;
        }
        if (nitrusFlag && nitrusValue > 0)
        {
            startNitrusEmitter();
        }
    }

    public void startNitrusEmitter()
    {
        rb.AddForce(transform.forward * 10000);
    }
    protected void UpdateNitroSlider()
    {
        if (carSetup.nitroSlider != null)
        {
            carSetup.nitroSlider.value = nitrusValue;
        }
    }
}


    

