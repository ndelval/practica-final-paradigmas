using UnityEngine;
using UnityEngine.UI;




public class CarSetup : MonoBehaviour
{
    // Car motor settings
    [SerializeField] public float motorForce;
    [SerializeField] public float breakForce = 3000f;
    [SerializeField] public float maxSteerAngle = 30f;
    [SerializeField] public float maxSpeedKmh = 180f;
    [SerializeField] public float initialNitroValue = 1f;
    [SerializeField] public float jumpForce = 1200;

    // Nitro settings
    public Slider nitroSlider;
    public Color normalSmokeColor = Color.gray;
    public Color nitroSmokeColor = Color.magenta;
    public float normalSmokeSpeed = 2f;
    public float nitroSmokeSpeed = 7f;
    public float normalEmissionRate = 10f;
    public float nitroEmissionRate = 50f;

    // Wheel Colliders and Visual Transforms
    public WheelCollider frontLeftWheelCollider;
    public WheelCollider frontRightWheelCollider;
    public WheelCollider rearLeftWheelCollider;
    public WheelCollider rearRightWheelCollider;

    public Transform frontLeftWheelTransform;
    public Transform frontRightWheelTransform;
    public Transform rearLeftWheelTransform;
    public Transform rearRightWheelTransform;

    // Exhaust particle systems
    public ParticleSystem smokeParticleSystem1;
    public ParticleSystem smokeParticleSystem2;

    private void Awake()
    {
        // Assign wheels
        frontLeftWheelCollider = transform.Find("Wheels/Colliders/FrontLeftWheelCollider")?.GetComponent<WheelCollider>();
        frontRightWheelCollider = transform.Find("Wheels/Colliders/FrontRightWheelCollider")?.GetComponent<WheelCollider>();
        rearLeftWheelCollider = transform.Find("Wheels/Colliders/RearLeftWheelCollider")?.GetComponent<WheelCollider>();
        rearRightWheelCollider = transform.Find("Wheels/Colliders/RearRightWheelCollider")?.GetComponent<WheelCollider>();

        frontLeftWheelTransform = transform.Find("Wheels/Meshes/FrontLeftWheelTransform");
        frontRightWheelTransform = transform.Find("Wheels/Meshes/FrontRightWheelTransform");
        rearLeftWheelTransform = transform.Find("Wheels/Meshes/RearLeftWheelTransform");
        rearRightWheelTransform = transform.Find("Wheels/Meshes/RearRightWheelTransform");
        nitroSlider = GameObject.FindGameObjectWithTag("NitroSlide").GetComponent<Slider>();
    }


}
