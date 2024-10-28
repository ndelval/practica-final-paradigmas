using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerReset : MonoBehaviour
{
    public Rigidbody rb;
    public LapTimer lapTimer;

    private Vector3 playerStartPosition;
    private Quaternion playerStartRotation;

    void Start()
    {
        
        playerStartPosition = rb.transform.position;
        playerStartRotation = rb.transform.rotation;
    }

    public void ResetPlayer()
    {
        rb.transform.position = playerStartPosition;
        rb.transform.rotation = playerStartRotation;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        lapTimer.ResetLapData();  
    }
}
