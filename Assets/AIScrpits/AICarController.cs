using System.Collections.Generic;
using UnityEngine;

public class AICarController : CarController
{
    public Transform path;
    public LayerMask detectionLayers;
    public LayerMask carLayer;


    public List<Transform> nodes;
    public int currentNode = 0;
    public float targetSteerAngle;
    public float currentSpeedKmh;
    public bool reversing = false;
    public float turnSpeed = 5f; 


    public SensorManager sensorManager;
    private OvertakeManager overtakeManager;
    public TurnManager turnManager;
    private MovementManager movementManager;
    private ReverseManager reverseManager;

    public bool hasStartedMoving = false;
    private float startDelay = 1f;
    private float startTimer = 0f;

    private void Start()
    {
        InitializeComponents();
        InitializeNodes();
    }

    private void InitializeComponents()
    {
        carSetup = GetComponent<CarSetup>();
        sensorManager = new SensorManager(transform, this);
        overtakeManager = new OvertakeManager(this, sensorManager);
        turnManager = new TurnManager(this, carSetup, overtakeManager);
        movementManager = new MovementManager(this, carSetup);
        reverseManager = new ReverseManager(this);
    }

    private void InitializeNodes()
    {
        nodes = new List<Transform>();
        float pathOffsetX = Random.Range(-0.5f, 0.5f); // Rango de desplazamiento en X
        float pathOffsetZ = Random.Range(-0.5f, 0.5f); // Rango de desplazamiento en Z (o Y si es necesario)

        foreach (Transform pathTransform in path.GetComponentsInChildren<Transform>())
        {
            if (pathTransform != path.transform)
            {
                // Clonar la posición del nodo y añadir un desplazamiento aleatorio
                Vector3 offsetPosition = pathTransform.position + new Vector3(pathOffsetX, 0, pathOffsetZ);
                Transform offsetNode = new GameObject("OffsetNode").transform;
                offsetNode.position = offsetPosition;
                offsetNode.parent = path; // Opcional: asignar el padre para mantener la jerarquía

                nodes.Add(offsetNode);
            }
        }
    }


    private void FixedUpdate()
    {
        if (!hasStartedMoving)
        {
            startTimer += Time.deltaTime;
            if (startTimer >= startDelay) hasStartedMoving = true;
        }

        if (hasStartedMoving)
        {
            currentSpeedKmh = rb.velocity.magnitude * 3.6f;

            // Condiciones para decidir el comportamiento del coche en función de los obstáculos y el camino
            reverseManager.CheckIfStuck();
            overtakeManager.AttemptOvertake();
            

            if (reverseManager.IsReversing)
            {
                
                reverseManager.PerformReverseMovement();
            }
            else
            {

                // Solo seguir el path si no estamos evitando ni en reversa
                overtakeManager.HandleLateralCollisionAdjustment();
                turnManager.CheckForSharpTurns();
                movementManager.ApplyAcceleration();
                turnManager.ApplySteering(nodes[currentNode]);
            }

            HandleMotor();
            ClampSpeed();
            HandleSteering();
            UpdateWheels();
            movementManager.CheckWaypointDistance(nodes, ref currentNode);
        }
    }



}
