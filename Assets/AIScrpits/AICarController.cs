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
    public OvertakeManager overtakeManager;
    public TurnManager turnManager;
    private MovementManager movementManager;
    private ReverseManager reverseManager;
    private CollisionManager collisionManager;

    public bool hasStartedMoving = false;
    private float startDelay = 1f;
    private float startTimer = 0f;
    private float collisionForce = 1000;

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
        sensorManager = new SensorManager();
        sensorManager.AddSensor(new FrontSensor(transform, this, 5f, 2.3f, 0.5f, 0.7f));
        sensorManager.AddSensor(new SideSensor(transform, this, 5f, 0.7f));
        sensorManager.AddSensor(new SideSensor(transform, this, 5f, -0.7f));
        sensorManager.AddSensor(new AngularSensor(transform, this, 5f, 30f));
        sensorManager.AddSensor(new AngularSensor(transform, this, 5f, -30f));
        overtakeManager = new OvertakeManager(this, sensorManager);
        turnManager = new TurnManager(this, carSetup, overtakeManager);
        movementManager = new MovementManager(this, carSetup);
        reverseManager = new ReverseManager(this);
        collisionManager = new CollisionManager(this, collisionForce);
        path = FindObjectOfType<Path>().gameObject.transform;
    }

    private void InitializeNodes()
    {
        nodes = new List<Transform>();
        float pathOffsetX = Random.Range(-0.5f, 0.5f);
        float pathOffsetZ = Random.Range(-0.5f, 0.5f);

        foreach (Transform pathTransform in path.GetComponentsInChildren<Transform>())
        {
            if (pathTransform != path.transform)
            {
                Vector3 offsetPosition = pathTransform.position + new Vector3(pathOffsetX, 0, pathOffsetZ);
                Transform offsetNode = new GameObject("OffsetNode").transform;
                offsetNode.position = offsetPosition;
                offsetNode.parent = path;

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

    private void HandleCollision(GameObject collidingCar, Collision collision)
    {
        if(collision.gameObject == gameObject)
        {
            float relativeImpactForce = collision.relativeVelocity.magnitude;

            collisionManager.HandleCollision(collidingCar, relativeImpactForce);
        }
            
    }
}