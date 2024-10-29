using UnityEngine;
using System.Collections.Generic;

public class PlayerCarController : CarController
{
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
            { "brake", KeyCode.Space },
            { "nitro", KeyCode.LeftShift }
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
        activateNitrus();
        UpdateNitroSlider();
    }

    private void GetInput()
    {
        verticalInput = 0;
        horizontalInput = 0;
        isBreaking = false;
        nitrusFlag = Input.GetKey(keyBindings["nitro"]);

        if (Input.GetKey(keyBindings["forward"])) verticalInput = 1;
        if (Input.GetKey(keyBindings["backward"])) verticalInput = -1;
        if (Input.GetKey(keyBindings["left"])) horizontalInput = -1;
        if (Input.GetKey(keyBindings["right"])) horizontalInput = 1;
        isBreaking = Input.GetKey(keyBindings["brake"]);
    }

    private void ClampSpeed()
    {
        float speedKmh = rb.velocity.magnitude * 3.6f;
        float maxSpeedMs = carSetup.maxSpeedKmh / 3.6f;

        if (speedKmh > carSetup.maxSpeedKmh)
        {
            rb.velocity = rb.velocity.normalized * maxSpeedMs;
        }

        speed = speedKmh;
    }
}
