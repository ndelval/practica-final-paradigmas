using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CarSelectionMenu : MonoBehaviour
{
    public Button crashingCarButton;
    public Button regularCarButton;
    public Button jumpingCarButton;
    private ICarSelectionState currentState;

    private CarFactory carFactory;
    public Button startRaceButton;
    public GameObject car = null;
    private List<Vector3> positions = new List<Vector3>() { new Vector3(66, 33, 292), new Vector3(58, 33, 294), new Vector3(62, 33, 300), new Vector3(63, 33, 289) };


    void Start()
    {
        carFactory = GetComponent<CarFactory>();

        crashingCarButton.onClick.AddListener(() => OnCarButtonSelected(CarType.CrashingCar));
        regularCarButton.onClick.AddListener(() => OnCarButtonSelected(CarType.RegularCar));
        jumpingCarButton.onClick.AddListener(() => OnCarButtonSelected(CarType.JumpingCar));
        startRaceButton.onClick.AddListener(() => StartRace(car));


    }

    void Update()
    {
        currentState?.UpdateState(this);
    }

    public void ChangeState(ICarSelectionState newState)
    {
        currentState?.ExitState(this);
        currentState = newState;
        currentState.EnterState(this);
    }

    void OnCarButtonSelected(CarType selectedCarType)
    {
        if (car != null) Destroy(car);
        car = carFactory.CreateCar(selectedCarType);
        car.transform.position = new Vector3(170, 16, 315);
        car.transform.rotation = Quaternion.Euler(0, -90, 0);

        Rigidbody rb = car.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false; // Evita que las fuerzas físicas afecten el coche
            rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ ; // Bloquea el movimiento
        }

        ChangeState(new CarSelectedState(car));
    }
    public void StartRace(GameObject selectedCar)
    {
        Destroy(crashingCarButton.gameObject);
        Destroy(regularCarButton.gameObject);
        Destroy(jumpingCarButton.gameObject);
        Destroy(startRaceButton.gameObject);

        // Libera el Rigidbody para permitir el movimiento
        Rigidbody rb = selectedCar.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false; // Permite que las fuerzas físicas afecten el coche
            rb.constraints = RigidbodyConstraints.None; // Quita todas las restricciones
        }

        // Asigna el coche a la cámara de Cinemachine para que lo siga
        Cinemachine.CinemachineVirtualCamera vCam = FindObjectOfType<Cinemachine.CinemachineVirtualCamera>();
        if (vCam != null)
        {
            Transform followPoint = selectedCar.transform.Find("FollowPoint");
            vCam.Follow = followPoint;
            vCam.LookAt = followPoint;
            selectedCar.transform.position = new Vector3(67, 32, 280);
            selectedCar.transform.rotation = Quaternion.Euler(0, -20, 0);
        }

        for (int i = 0; i < 4; i++) // Supongamos que tienes 4 AICars
        {
            GameObject aiCar = carFactory.CreateCar(CarType.AICar); // O el tipo que desees
            aiCar.transform.position = positions[i];
            aiCar.transform.rotation = Quaternion.Euler(0, -20, 0); // Posiciona a los AI Cars
        }
    }

}
