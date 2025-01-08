using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CarType
{
    CrashingCar,
    RegularCar,
    JumpingCar,
    AICar
}

public class CarFactory : MonoBehaviour
{
    [SerializeField] GameObject CrashingCar;
    [SerializeField] GameObject RegularCar;
    [SerializeField] GameObject JumpingCar;
    [SerializeField] GameObject AICar;

    public GameObject CreateCar(CarType carType)
    {
        GameObject car = null;

        switch (carType)
        {
            case CarType.CrashingCar:
                car = Instantiate(CrashingCar);
                break;

            case CarType.RegularCar:
                car = Instantiate(RegularCar);
                car.tag = "Player";
                break;

            case CarType.JumpingCar:
                car = Instantiate(JumpingCar);
                break;
            case CarType.AICar:
                car = Instantiate(AICar);
                break;
        }

        return car;
    }
}
