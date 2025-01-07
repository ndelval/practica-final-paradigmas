using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpingCarController : JumpingCarController
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
            { "nitro", KeyCode.LeftShift },
            { "jump", KeyCode.E }
        };

        startPosition = rb.transform.position;
    }

    void Update()
    {
        GetInput();
    }

    private void FixedUpdate()
    {
        HandleMotor();
        HandleSteering();
        UpdateWheels();
        ClampSpeed();
        activateNitrus();
        UpdateNitroSlider();
        UpdateAudio();
        HandleJump();

        // No es necesario manejar el salto aquí si se maneja en Update
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
        if (Input.GetKey(keyBindings["jump"])) Jump();
        isBreaking = Input.GetKey(keyBindings["brake"]);

    }



}
