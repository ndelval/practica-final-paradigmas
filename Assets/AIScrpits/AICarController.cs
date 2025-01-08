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
    public float collisionForce = 80000f;  // Fuerza de retroceso al recibir una colisión

    private void Start()
    {
        InitializeComponents();
        InitializeNodes();
    }
    

    private void OnEnable()
    {
        
        CarCollision.OnCarCollision += HandleCollision;
    }

    private void OnDisable()
    {
        
        CarCollision.OnCarCollision -= HandleCollision;
    }




    private void InitializeComponents()
    {
        carSetup = GetComponent<CarSetup>();
        sensorManager = new SensorManager(transform, this);
        overtakeManager = new OvertakeManager(this, sensorManager);
        turnManager = new TurnManager(this, carSetup, overtakeManager);
        movementManager = new MovementManager(this, carSetup);
        reverseManager = new ReverseManager(this);
        path = FindObjectOfType<Path>().gameObject.transform;
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
    private void HandleCollision(GameObject collidingCar)
    {
        
        if (collidingCar != gameObject)
        {
            // Calcula el vector desde el coche que colisionó (PlayerCar) hacia este AICar
            Vector3 directionFromPlayer = transform.position - collidingCar.transform.position;

            
            directionFromPlayer.Normalize();

             
            rb.AddForce(directionFromPlayer * collisionForce + Vector3.up * collisionForce/ 5, ForceMode.Impulse);

            Debug.Log($"Colisión detectada entre {collidingCar.name} y {gameObject.name}");
        }
    }



}
