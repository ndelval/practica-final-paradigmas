using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Sensor
{
    protected Transform carTransform;
    protected AICarController carController;
    protected float sensorLength;

    public Sensor(Transform carTransform, AICarController carController, float sensorLength)
    {
        this.carTransform = carTransform;
        this.carController = carController;
        this.sensorLength = sensorLength;
    }

    public abstract bool CheckSensor(out RaycastHit hit);
    public abstract void DrawRay(bool detected);
}


public class FrontSensor : Sensor
{
    private float frontSensorZPos;
    private float frontSensorYPos;
    private float sideSensorXPos;

    public FrontSensor(Transform carTransform, AICarController carController, float sensorLength, float frontSensorZPos, float frontSensorYPos, float sideSensorXPos)
        : base(carTransform, carController, sensorLength)
    {
        this.frontSensorZPos = frontSensorZPos;
        this.frontSensorYPos = frontSensorYPos;
        this.sideSensorXPos = sideSensorXPos;
    }

    public override bool CheckSensor(out RaycastHit hit)
    {
        float carHeight = carController.GetComponent<Collider>().bounds.size.y;
        Vector3 centerSensorPos = carTransform.position + carTransform.forward * frontSensorZPos + carTransform.up * (carHeight / 2);
        bool detected = Physics.Raycast(centerSensorPos, carTransform.forward, out hit, sensorLength, carController.detectionLayers);
        DrawRay(detected);
        return detected;
    }

    public override void DrawRay(bool detected)
    {
        float carHeight = carController.GetComponent<Collider>().bounds.size.y;
        Vector3 centerSensorPos = carTransform.position + carTransform.forward * frontSensorZPos + carTransform.up * (carHeight / 2);
        Color rayColor = detected ? Color.red : Color.green;
        Debug.DrawRay(centerSensorPos, carTransform.forward * sensorLength, rayColor);
    }
}





public class SideSensor : Sensor
{
    private float sideSensorXPos;

    public SideSensor(Transform carTransform, AICarController carController, float sensorLength, float sideSensorXPos)
        : base(carTransform, carController, sensorLength)
    {
        this.sideSensorXPos = sideSensorXPos;
    }

    public override bool CheckSensor(out RaycastHit hit)
    {
        float carHeight = carController.GetComponent<Collider>().bounds.size.y;
        Vector3 sideSensorPos = carTransform.position + carTransform.right * sideSensorXPos + carTransform.up * (carHeight / 2);
        bool detected = Physics.Raycast(sideSensorPos, carTransform.right, out hit, sensorLength, carController.detectionLayers);
        DrawRay(detected);
        return detected;
    }

    public override void DrawRay(bool detected)
    {
        float carHeight = carController.GetComponent<Collider>().bounds.size.y;
        Vector3 sideSensorPos = carTransform.position + carTransform.right * sideSensorXPos + carTransform.up * (carHeight / 2);
        Color rayColor = detected ? Color.red : Color.green;
        Debug.DrawRay(sideSensorPos, carTransform.right * sensorLength, rayColor);
    }
}


public class AngularSensor : Sensor
{
    private float sensorAngle;

    public AngularSensor(Transform carTransform, AICarController carController, float sensorLength, float sensorAngle)
        : base(carTransform, carController, sensorLength)
    {
        this.sensorAngle = sensorAngle;
    }

    public override bool CheckSensor(out RaycastHit hit)
    {
        float carHeight = carController.GetComponent<Collider>().bounds.size.y;
        Vector3 sensorPos = carTransform.position + carTransform.up * (carHeight / 2);
        bool detected = Physics.Raycast(sensorPos, Quaternion.AngleAxis(sensorAngle, carTransform.up) * carTransform.forward, out hit, sensorLength, carController.detectionLayers);
        DrawRay(detected);
        return detected;
    }

    public override void DrawRay(bool detected)
    {
        float carHeight = carController.GetComponent<Collider>().bounds.size.y;
        Vector3 sensorPos = carTransform.position + carTransform.up * (carHeight / 2);
        Color rayColor = detected ? Color.red : Color.green;
        Debug.DrawRay(sensorPos, Quaternion.AngleAxis(sensorAngle, carTransform.up) * carTransform.forward * sensorLength, rayColor);
    }
}
