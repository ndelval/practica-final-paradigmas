using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject needle;
    public PlayerCarController Car; 

    private float startPosition = 218.214f;
    private float endPosition = -40.64f;
    private float desiredPosition;

    public float vehicleSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    { 
        vehicleSpeed = Car.speed;
        UpdateNeedle();
    }

    public void UpdateNeedle()
    {
        desiredPosition = startPosition - endPosition;
        float temp = vehicleSpeed / 180;
        needle.transform.eulerAngles = new Vector3(0, 0, (startPosition - temp * desiredPosition));

    }
}
