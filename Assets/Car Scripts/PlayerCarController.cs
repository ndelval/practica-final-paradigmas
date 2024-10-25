using UnityEngine;

public class PlayerCarController : CarController
{
    public float maxSpeedKmh = 180f;
    public float speed;

    private void FixedUpdate()
    {
        GetInput();
        HandleMotor();
        HandleSteering();
        UpdateWheels();
        ClampSpeed();
    }

    private void GetInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        isBreaking = Input.GetKey(KeyCode.Space);
    }

    private void ClampSpeed()
    {
        float speedKmh = rb.velocity.magnitude * 3.6f;

        if (speedKmh > maxSpeedKmh)
        {
            float maxSpeedMs = maxSpeedKmh / 3.6f;
            rb.velocity = rb.velocity.normalized * maxSpeedMs;
        }

        speed = speedKmh;
    }
}
