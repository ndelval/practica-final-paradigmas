using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CarSelectionMenu : MonoBehaviour
{
    public Button crashingCarButton;
    public Button regularCarButton;
    public Button jumpingCarButton;

    private CarFactory carFactory;
    public Button startRaceButton;
    private GameObject car = null;


    void Start()
    {
        carFactory = GetComponent<CarFactory>();

        crashingCarButton.onClick.AddListener(() => OnCarButtonSelected(CarType.CrashingCar));
        regularCarButton.onClick.AddListener(() => OnCarButtonSelected(CarType.RegularCar));
        jumpingCarButton.onClick.AddListener(() => OnCarButtonSelected(CarType.JumpingCar));
        startRaceButton.onClick.AddListener(() => StartRace(car));


    }

    void OnCarButtonSelected(CarType selectedCarType)
    {
        if (car != null)
        {
            Destroy(car);
        }

        GameObject selectedCar = carFactory.CreateCar(selectedCarType);
        
        selectedCar.transform.position = new Vector3(170, 16, 315);
        selectedCar.transform.rotation = Quaternion.Euler(0, -90, 0); 

        car = selectedCar;

        Rigidbody rb = selectedCar.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }

        
        
    }
    void StartRace(GameObject selectedCar)
    {
        Destroy(crashingCarButton.gameObject);
        Destroy(regularCarButton.gameObject);
        Destroy(jumpingCarButton.gameObject);
        Destroy(startRaceButton.gameObject);

        // Asigna el coche a la cámara de Cinemachine para que lo siga
        Cinemachine.CinemachineVirtualCamera vCam = FindObjectOfType<Cinemachine.CinemachineVirtualCamera>();
        if (vCam != null)
        {
            Transform followPoint = selectedCar.transform.Find("FollowPoint");
            vCam.Follow = followPoint;
            vCam.LookAt = followPoint;
            selectedCar.transform.position = new Vector3(67, 32, 280);
            selectedCar.transform.rotation = Quaternion.Euler(0, -20, 0);

            // La cámara seguirá al coche seleccionado
        }
        for (int i = 0; i < 4; i++)  // Supongamos que tienes 4 AICars
        {
            GameObject aiCar = carFactory.CreateCar(CarType.AICar);  // O el tipo que desees
            aiCar.transform.position = new Vector3(66 + 2*i* (-1)^i, 33, 292 - 2*i*(-1) ^ i);
            aiCar.transform.rotation = Quaternion.Euler(0, -20, 0); ;  // Posiciona a los AI Cars
        }
    }

}
