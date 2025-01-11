using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorManager
{
    private List<Sensor> sensors = new List<Sensor>();
    public RaycastHit hit;

    public void AddSensor(Sensor sensor)
    {
        sensors.Add(sensor);
    }

    public bool CheckSensors(out RaycastHit hit)
    {
        foreach (var sensor in sensors)
        {
            if (sensor.CheckSensor(out hit))
            {
                this.hit = hit;
                return true;
            }
        }
        hit = default;
        return false;
    }

    public bool CheckFrontAllObstacle(out bool carAheadCenter, out bool carAheadLeft, out bool carAheadRight)
    {
        carAheadCenter = sensors[0].CheckSensor(out hit);
        carAheadLeft = sensors[1].CheckSensor(out hit);
        carAheadRight = sensors[2].CheckSensor(out hit);
        return carAheadCenter || carAheadLeft || carAheadRight;
    }

    public bool CheckFrontObstacle(out bool carAheadCenter, out bool carAheadLeft, out bool carAheadRight)
    {
        carAheadCenter = sensors[0].CheckSensor(out hit);
        carAheadLeft = sensors[1].CheckSensor(out hit);
        carAheadRight = sensors[2].CheckSensor(out hit);
        return carAheadCenter || carAheadLeft || carAheadRight;
    }

    public bool CheckSideClearance(out bool rightSideClear, out bool leftSideClear)
    {
        rightSideClear = !sensors[3].CheckSensor(out hit);
        leftSideClear = !sensors[4].CheckSensor(out hit);
        return rightSideClear || leftSideClear;
    }

    public bool CheckLateralCollision(out bool leftCollision, out bool rightCollision)
    {
        leftCollision = sensors[1].CheckSensor(out hit);
        rightCollision = sensors[2].CheckSensor(out hit);
        return leftCollision || rightCollision;
    }
}