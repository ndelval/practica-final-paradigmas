using UnityEngine;

public class CarSetup : MonoBehaviour
{
    // Settings
    [SerializeField] public float motorForce = 2000f;
    [SerializeField] public float breakForce = 3000f;
    [SerializeField] public float maxSteerAngle = 30f;

    // Wheel Colliders
    public WheelCollider frontLeftWheelCollider;
    public WheelCollider frontRightWheelCollider;
    public WheelCollider rearLeftWheelCollider;
    public WheelCollider rearRightWheelCollider;

    // Wheels (Visual Transforms)
    public Transform frontLeftWheelTransform;
    public Transform frontRightWheelTransform;
    public Transform rearLeftWheelTransform;
    public Transform rearRightWheelTransform;

    private void Awake()
    {
        // Buscar automáticamente los componentes usando nombres específicos
        frontLeftWheelCollider = transform.Find("Wheels/Colliders/FrontLeftWheelCollider")?.GetComponent<WheelCollider>();
        frontRightWheelCollider = transform.Find("Wheels/Colliders/FrontRightWheelCollider")?.GetComponent<WheelCollider>();
        rearLeftWheelCollider = transform.Find("Wheels/Colliders/RearLeftWheelCollider")?.GetComponent<WheelCollider>();
        rearRightWheelCollider = transform.Find("Wheels/Colliders/RearRightWheelCollider")?.GetComponent<WheelCollider>();

        frontLeftWheelTransform = transform.Find("Wheels/Meshes/FrontLeftWheelTransform");
        frontRightWheelTransform = transform.Find("Wheels/Meshes/FrontRightWheelTransform");
        rearLeftWheelTransform = transform.Find("Wheels/Meshes/RearLeftWheelTransform");
        rearRightWheelTransform = transform.Find("Wheels/Meshes/RearRightWheelTransform");

    }
}
