using UnityEngine;
using System.Collections.Generic;

public class PlayerCarController : CarController
{
    
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
        UpdateAudio();
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


}
