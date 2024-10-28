using UnityEngine;
using System.Collections.Generic;

public class PlayerCarController : CarController
{
    public float maxSpeedKmh = 180f;
    public float speed;
    public Vector3 startPosition;
    public Dictionary<string, KeyCode> keyBindings;

    void Start()
    {
        keyBindings = new Dictionary<string, KeyCode>
        {
            { "forward", KeyCode.W },
            { "backward", KeyCode.S },
            { "left", KeyCode.A },
            { "right", KeyCode.D },
            { "brake", KeyCode.Space }
        };
        startPosition = rb.transform.position;
    }
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
        
        if (!Input.GetKey(keyBindings["forward"]) && !Input.GetKey(keyBindings["backward"]))
        {
            verticalInput = 0; 
        }
        if (!Input.GetKey(keyBindings["left"]) && !Input.GetKey(keyBindings["right"]))
        {
            horizontalInput = 0; 
        }
        if (!Input.GetKey(keyBindings["brake"]))
        {
            isBreaking = false;
        }
        
        if (Input.GetKey(keyBindings["forward"])) verticalInput = 1;
        if (Input.GetKey(keyBindings["backward"])) verticalInput = -1;
        if (Input.GetKey(keyBindings["left"])) horizontalInput = -1;
        if (Input.GetKey(keyBindings["right"])) horizontalInput = 1;

        // Actualizar la variable de frenado
        isBreaking = Input.GetKey(keyBindings["brake"]);
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
