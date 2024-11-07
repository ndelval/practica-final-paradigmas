using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorManager
{
    private Transform carTransform;
    private AICarController carController;
    public float sensorLength = 5f;
    private float frontSensorZPos = 2.3f;
    private float frontSensorYPos = 0.5f;
    private float sideSensorXPos = 0.7f;
    private float frontSensorAngle = 30f;
    
    public RaycastHit hit;

    public SensorManager(Transform carTransform, AICarController carController)
    {
        this.carTransform = carTransform;
        this.carController = carController;
    }

    public bool CheckFrontAllObstacle(out bool carAheadCenter, out bool carAheadLeft, out bool carAheadRight)
    {
        Vector3 centerSensorPos = carTransform.position + carTransform.forward * frontSensorZPos + carTransform.up * frontSensorYPos;
        Vector3 leftSensorPos = carTransform.position + carTransform.forward * frontSensorZPos + carTransform.right * -sideSensorXPos + carTransform.up * frontSensorYPos;
        Vector3 rightSensorPos = carTransform.position + carTransform.forward * frontSensorZPos + carTransform.right * sideSensorXPos + carTransform.up * frontSensorYPos;

        carAheadCenter = SendFrontRay(centerSensorPos, out hit, 0.85f, true, carController.detectionLayers);
        carAheadLeft = SendFrontRay(leftSensorPos, out hit, 0.4f, false, carController.detectionLayers);
        carAheadRight = SendFrontRay(rightSensorPos, out hit, -0.4f, false, carController.detectionLayers);

        return carAheadCenter || carAheadLeft || carAheadRight;
    }

    public bool CheckFrontObstacle(out bool carAheadCenter, out bool carAheadLeft, out bool carAheadRight)
    {
        Vector3 centerSensorPos = carTransform.position + carTransform.forward * frontSensorZPos + carTransform.up * frontSensorYPos;
        Vector3 leftSensorPos = carTransform.position + carTransform.forward * frontSensorZPos + carTransform.right * -sideSensorXPos + carTransform.up * frontSensorYPos;
        Vector3 rightSensorPos = carTransform.position + carTransform.forward * frontSensorZPos + carTransform.right * sideSensorXPos + carTransform.up * frontSensorYPos;

        carAheadCenter = SendFrontRay(centerSensorPos, out hit, 0.85f, true, carController.carLayer);
        carAheadLeft = SendFrontRay(leftSensorPos, out hit, 0.4f, false, carController.carLayer);
        carAheadRight = SendFrontRay(rightSensorPos, out hit, -0.4f, false, carController.carLayer);

        return carAheadCenter || carAheadLeft || carAheadRight;
    }
    public bool CheckLateralCollision(out bool leftCollision, out bool rightCollision)
    {
        Vector3 frontRightWheel = carController.carSetup.frontRightWheelCollider.transform.position;
        Vector3 rearRightWheel = carController.carSetup.rearRightWheelCollider.transform.position;
        Vector3 frontLeftWheel = carController.carSetup.frontLeftWheelCollider.transform.position;
        Vector3 rearLeftWheel = carController.carSetup.rearLeftWheelCollider.transform.position;

        // Detectar colisiones laterales en el lado derecho e izquierdo
        rightCollision = CheckLateralSensorAll(frontRightWheel, 1, carController.carLayer) ||
                         CheckLateralSensorAll(rearRightWheel, 1, carController.carLayer);
        leftCollision = CheckLateralSensorAll(frontLeftWheel, -1, carController.carLayer) ||
                        CheckLateralSensorAll(rearLeftWheel, -1, carController.carLayer);

        return rightCollision || leftCollision;
    }

    public bool CheckSideClearance(out bool rightSideClear, out bool leftSideClear)
    {
        // Obtener la posición en el eje Y de las ruedas delanteras para la altura de los sensores laterales
        float sensorHeight = carController.carSetup.frontLeftWheelCollider.transform.position.y;

        // Posición de los sensores en la parte delantera y trasera de los laterales del coche
        Vector3 frontRightSensorPos = carTransform.position + carTransform.forward * (frontSensorZPos-0.3f) + carTransform.right * sideSensorXPos;
        //Vector3 rearRightSensorPos = carTransform.position - carTransform.forward * (frontSensorZPos - 0.3f) + carTransform.right * sideSensorXPos;
        Vector3 frontLeftSensorPos = carTransform.position + carTransform.forward * (frontSensorZPos - 0.3f) - carTransform.right * sideSensorXPos;
        //Vector3 rearLeftSensorPos = carTransform.position - carTransform.forward * (frontSensorZPos - 0.3f) - carTransform.right * sideSensorXPos;

        // Ajustar la altura en el eje Y para que esté al nivel de las ruedas
        frontRightSensorPos.y = sensorHeight;
        //rearRightSensorPos.y = sensorHeight;
        frontLeftSensorPos.y = sensorHeight;
        //rearLeftSensorPos.y = sensorHeight;

        // Verificar colisiones en los sensores laterales derecho e izquierdo
        rightSideClear = !CheckLateralSensorAll(frontRightSensorPos, 1, carController.detectionLayers); //&&
                                                                                                        //!CheckLateralSensorAll(rearRightSensorPos, 1, carController.detectionLayers);

        leftSideClear = !CheckLateralSensorAll(frontLeftSensorPos, -1, carController.detectionLayers); //&&
                        //!CheckLateralSensorAll(rearLeftSensorPos, -1, carController.detectionLayers);

        return rightSideClear || leftSideClear;
    }


    public bool CheckAngularClearance(out bool rightClear, out bool leftClear)
    {
        // Usar posiciones de las ruedas delanteras como puntos de partida para los rayos angulares
        Vector3 frontRightWheel = carController.carSetup.frontRightWheelCollider.transform.position;
        Vector3 frontLeftWheel = carController.carSetup.frontLeftWheelCollider.transform.position;

        // Verificar si el ángulo derecho está despejado
        rightClear = !SendAngularRay(frontRightWheel, out hit, frontSensorAngle, 1);

        // Verificar si el ángulo izquierdo está despejado
        leftClear = !SendAngularRay(frontLeftWheel, out hit, -frontSensorAngle, -1);

        return rightClear && leftClear;
    }
    // LO DE CENTER YA NO SE USA O LO IMPLEMENTAS O LO QUITAS
    public bool SendFrontRay(Vector3 sensorStartingPos, out RaycastHit hit, float multiplier, bool center, LayerMask layer)
    {
        bool detected = Physics.Raycast(sensorStartingPos, carTransform.forward, out hit, sensorLength, layer);
        Debug.DrawRay(sensorStartingPos, carTransform.forward);
        if (detected)
        {
            //Debug.Log($"Obstáculo detectado: {hit.transform.name} en el centro: {center}");
            Debug.DrawRay(sensorStartingPos, carTransform.forward * sensorLength, Color.red);
        }
        else
        {
            Debug.DrawRay(sensorStartingPos, carTransform.forward * sensorLength, Color.green);
        }
        return detected;
    }

    public bool SendAngularRay(Vector3 sensorStartingPos, out RaycastHit hit, float sensorAngle, float multiplier)
    {
        bool detected = Physics.Raycast(
            sensorStartingPos,
            Quaternion.AngleAxis(sensorAngle, carTransform.up) * carTransform.forward,
            out hit,
            sensorLength,
            carController.carLayer
        );

        if (detected)
        {
            
            Debug.DrawRay(sensorStartingPos, Quaternion.AngleAxis(sensorAngle, carTransform.up) * carTransform.forward * (sensorLength-1), Color.red);
        }
        else
        {
            Debug.DrawRay(sensorStartingPos, Quaternion.AngleAxis(sensorAngle, carTransform.up) * carTransform.forward * (sensorLength-1), Color.green);
        }
        //Debug.Log($"Angular {detected}");
        return detected;
    }

    public bool CheckLateralSensorAll(Vector3 wheelPosition, int directionMultiplier, LayerMask layer)
    {
        Vector3 lateralDirection = Quaternion.AngleAxis(90 * directionMultiplier, carTransform.up) * carTransform.forward;
        Debug.DrawRay(wheelPosition, lateralDirection * (sensorLength-3.5f), Color.blue);

        bool detected = Physics.Raycast(wheelPosition, lateralDirection, out hit, (sensorLength-3.5f), layer);
        return detected;
    }

    public bool CheckLateralSensor(Vector3 wheelPosition, int directionMultiplier)
    {
        return CheckLateralSensorAll(wheelPosition, directionMultiplier, carController.carLayer);
    }

}
