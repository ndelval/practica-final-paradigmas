using System.Collections.Generic;
using UnityEngine;

public class AICarController : CarController
{
    public Transform path;
    private List<Transform> nodes;
    private int currentNode = 0;

    public float sensorLength = 3.5f;
    private float frontSensorZPos = 2.3f;
    private float frontSensorYPos = 0.5f;
    private float sideSensorXPos = 1f;
    private float frontSensorAngle = 30f;
    private RaycastHit hit;
    public LayerMask detectionLayers;
    public LayerMask carLayer; // Capa para detectar otros coches
    public LayerMask barrierLayer; // Capa para detectar barreras

    private bool avoiding = false;
    private float avoidMultiplier = 0f;
    private float targetSteerAngle = 0f;
    public float turnSpeed = 5f;

    public float maxCurveSpeedBase = 50f; // Velocidad base para curvas suaves
    public float minCurveSpeed = 20f; // Velocidad mínima para curvas muy cerradas
    public float maxCurveAngle = 90f; // Ángulo máximo para aplicar el freno
    public float currentSpeedKmh;

    private float minBarrierDistance = 0.1f; // Distancia mínima a la barrera para iniciar la corrección
    private float maxBarrierDistance = 1.0f; // Distancia a la barrera donde se deja de corregir
    private float maxAvoidanceStrength = 0.1f; // Ajuste de dirección máximo (para evitar giros bruscos)

    private void Start()
    {
        carSetup = GetComponent<CarSetup>();
        Transform[] pathTransforms = path.GetComponentsInChildren<Transform>();
        nodes = new List<Transform>();

        foreach (Transform pathTransform in pathTransforms)
        {
            if (pathTransform != path.transform)
            {
                nodes.Add(pathTransform);
            }
        }
    }

    private void FixedUpdate()
    {
        Sensors();
        ApplySteer();
        CheckAndAdjustForSharpTurns();
        ApplyAcceleration();

        LerpToSteerAngle();

        ApplyDirectSteering();

        HandleMotor();
        ClampSpeed();
        UpdateWheels();
        CheckWaypointDistance();
    }

    private void Sensors()
    {
        avoiding = false;
        avoidMultiplier = 0;

        // Obtener la posición de cada WheelCollider
        Vector3 frontLeftWheelPos = carSetup.frontLeftWheelCollider.transform.position;
        Vector3 frontRightWheelPos = carSetup.frontRightWheelCollider.transform.position;
        Vector3 rearLeftWheelPos = carSetup.rearLeftWheelCollider.transform.position;
        Vector3 rearRightWheelPos = carSetup.rearRightWheelCollider.transform.position;

        // Sensores laterales en cada WheelCollider
        CheckLateralSensor(frontLeftWheelPos, -1);  // Izquierda delantera
        CheckLateralSensor(frontRightWheelPos, 1);  // Derecha delantera
        CheckLateralSensor(rearLeftWheelPos, -1);   // Izquierda trasera
        CheckLateralSensor(rearRightWheelPos, 1);   // Derecha trasera

        // Sensores frontales existentes (puedes mantener tu lógica de sensores frontales aquí)
        Vector3 sensorStartingPos = transform.TransformPoint(new Vector3(0, frontSensorYPos, frontSensorZPos));

        // Right sensor
        Vector3 rightSensorPos = transform.TransformPoint(new Vector3(sideSensorXPos, frontSensorYPos, frontSensorZPos));
        SendFrontRay(rightSensorPos, hit, -1f, false);
        SendAngularRay(rightSensorPos, hit, frontSensorAngle, -0.5f);

        // Left sensor
        Vector3 leftSensorPos = transform.TransformPoint(new Vector3(-sideSensorXPos, frontSensorYPos, frontSensorZPos));
        SendFrontRay(leftSensorPos, hit, 1f, false);
        SendAngularRay(leftSensorPos, hit, -frontSensorAngle, 0.5f);

        // Center/front sensor
        if (avoidMultiplier == 0)
        {
            SendFrontRay(sensorStartingPos, hit, 1f, true);
        }
    }

    private void CheckLateralSensor(Vector3 wheelPosition, int directionMultiplier)
    {
        // Ajuste de dirección del rayo (-1 para izquierda, 1 para derecha)
        Vector3 lateralDirection = Quaternion.AngleAxis(90 * directionMultiplier, transform.up) * transform.forward;

        // Dibujar y lanzar el rayo lateral
        Debug.DrawRay(wheelPosition, lateralDirection * sensorLength, Color.blue);

        if (Physics.Raycast(wheelPosition, lateralDirection, out hit, sensorLength, barrierLayer))
        {
            float distanceToBarrier = hit.distance;

            // Si está dentro de la distancia mínima, aplicar una corrección progresiva
            if (distanceToBarrier < maxBarrierDistance)
            {
                avoiding = true;
                // Interpolar la fuerza de corrección según la distancia (más fuerte cuanto más cerca)
                float avoidanceStrength = Mathf.Lerp(maxAvoidanceStrength, 0, (distanceToBarrier - minBarrierDistance) / (maxBarrierDistance - minBarrierDistance));
                avoidMultiplier -= directionMultiplier * avoidanceStrength;
                Debug.Log($"Ajustando para mantener distancia con barrera a {distanceToBarrier}m en el lado {(directionMultiplier == 1 ? "derecho" : "izquierdo")}");
            }
        }
    }
    private void SendFrontRay(Vector3 sensorStartingPos, RaycastHit hit, float multiplier, bool center)
    {
        if (Physics.Raycast(sensorStartingPos, transform.forward, out hit, sensorLength, detectionLayers))
        {
            avoiding = true;
            avoidMultiplier += center ? (hit.normal.x < 0 ? -multiplier : multiplier) : multiplier;
        }
        Debug.DrawRay(sensorStartingPos, transform.forward * sensorLength, Color.blue);
    }

    private void SendAngularRay(Vector3 sensorStartingPos, RaycastHit hit, float sensorAngle, float multiplier)
    {
        if (Physics.Raycast(sensorStartingPos, Quaternion.AngleAxis(sensorAngle, transform.up) * transform.forward, out hit, sensorLength, detectionLayers))
        {
            avoiding = true;
            avoidMultiplier += multiplier;
        }
        Debug.DrawRay(sensorStartingPos, Quaternion.AngleAxis(sensorAngle, transform.up) * transform.forward * sensorLength, Color.blue);
    }

    private void ApplySteer()
    {
        if (avoiding)
        {
            targetSteerAngle = carSetup.maxSteerAngle * avoidMultiplier;
            horizontalInput = avoidMultiplier;
        }
        else
        {
            Vector3 relativeVector = transform.InverseTransformPoint(nodes[currentNode].position);
            float newSteer = (relativeVector.x / relativeVector.magnitude) * carSetup.maxSteerAngle;
            targetSteerAngle = newSteer;
            Debug.Log(newSteer);
            horizontalInput = newSteer / carSetup.maxSteerAngle;
        }
    }

    private void LerpToSteerAngle()
    {
        currentSteerAngle = Mathf.Lerp(currentSteerAngle, targetSteerAngle, Time.deltaTime * turnSpeed);
        carSetup.frontLeftWheelCollider.steerAngle = currentSteerAngle;
        carSetup.frontRightWheelCollider.steerAngle = currentSteerAngle;
    }

    private void ApplyDirectSteering()
    {
        carSetup.frontLeftWheelCollider.steerAngle = targetSteerAngle;
        carSetup.frontRightWheelCollider.steerAngle = targetSteerAngle;
    }

    private void ApplyAcceleration()
    {
        verticalInput = 1f;
    }

    private float CalculateMaxSpeedForAngle(float curveAngle)
    {
        float minSpeed = 30f;
        float maxSpeed = carSetup.maxSpeedKmh;

        float speedFactor = Mathf.Clamp01(1 - (curveAngle / 90f));
        return Mathf.Lerp(minSpeed, maxSpeed, speedFactor * speedFactor);
    }

    private void CheckAndAdjustForSharpTurns()
    {
        currentSpeedKmh = rb.velocity.magnitude * 3.6f;

        int baseLookahead = 2;
        int maxLookahead = 5;
        int dynamicLookahead = baseLookahead + Mathf.FloorToInt(currentSpeedKmh / 50f);
        int lookahead = Mathf.Clamp(dynamicLookahead, baseLookahead, maxLookahead);

        int nodesToConsider = Mathf.Min(lookahead, nodes.Count - currentNode - 1);

        if (nodesToConsider > 1)
        {
            Vector3 currentDirection = transform.forward.normalized;
            Vector3 nextNodeDirection = (nodes[currentNode + nodesToConsider].position - nodes[currentNode].position).normalized;

            float turnAngle = Vector3.Angle(currentDirection, nextNodeDirection);
            float adjustedMaxCurveSpeed = CalculateMaxSpeedForAngle(turnAngle);

            if (currentSpeedKmh > adjustedMaxCurveSpeed)
            {
                verticalInput = Mathf.Clamp(adjustedMaxCurveSpeed / currentSpeedKmh, 0.5f, 1f);
                isBreaking = true;
            }
            else
            {
                verticalInput = 1f;
                isBreaking = false;
            }

            // Debug.Log($"Nodo actual: {nodes[currentNode]}, Nodo objetivo para el steering: {nodes[currentNode + nodesToConsider]}");


        }
    }

    private void CheckWaypointDistance()
    {
        float dynamicDistanceThreshold = Mathf.Lerp(3f, 8f, currentSpeedKmh / carSetup.maxSpeedKmh); // Ajusta 3 y 8 según tus necesidades
        float distanceToNode = Vector3.Distance(transform.position, nodes[currentNode].position);
        if (distanceToNode < dynamicDistanceThreshold)
        {
            currentNode++;
            if (currentNode >= nodes.Count)
            {
                currentNode = 0;
            }
        }
    }
}
