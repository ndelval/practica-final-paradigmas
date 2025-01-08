using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject needle;
    private PlayerCarController Car;


    private float needleStartPosition = 218.214f;
    private float needleEndPosition = -40.64f;
    private float desiredPosition;

    public float vehicleSpeed;

    public VolumeManager volumeManager;
    void Start() {
        Car = FindObjectOfType<PlayerCarController>();
    }
    // Update is called once per frame
    void FixedUpdate()
    {   
        
        if (Car == null) {
            Car = FindObjectOfType<PlayerCarController>();
        }
        else{
            vehicleSpeed = Car.speed;
            UpdateNeedle();
            volumeManager.changeVolume();
        }
            
        
    }

    public void UpdateNeedle()
    {
        desiredPosition = needleStartPosition - needleEndPosition;
        float temp = vehicleSpeed / 180;
        needle.transform.eulerAngles = new Vector3(0, 0, (needleStartPosition - temp * desiredPosition));

    }
}
