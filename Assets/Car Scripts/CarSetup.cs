using UnityEngine;

public class CarSetup : MonoBehaviour
{
    // Settings
    [SerializeField] public float motorForce, breakForce, maxSteerAngle;

    // Wheel Colliders
    [SerializeField] public WheelCollider frontLeftWheelCollider, frontRightWheelCollider;
    [SerializeField] public WheelCollider rearLeftWheelCollider, rearRightWheelCollider;

    // Wheels
    [SerializeField] public Transform frontLeftWheelTransform, frontRightWheelTransform;
    [SerializeField] public Transform rearLeftWheelTransform, rearRightWheelTransform;
}
