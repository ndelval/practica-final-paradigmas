using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICarSelectionState
{
    void EnterState(CarSelectionMenu carSelectionMenu);
    void UpdateState(CarSelectionMenu carSelectionMenu);
    void ExitState(CarSelectionMenu carSelectionMenu);
}
public class WaitingForSelectionState : ICarSelectionState
{
    public void EnterState(CarSelectionMenu carSelectionMenu)
    {
        Debug.Log("Estado: Esperando selección");
        // Puedes inicializar botones o interfaz aquí
    }

    public void UpdateState(CarSelectionMenu carSelectionMenu)
    {
        // Lógica mientras espera la selección
    }

    public void ExitState(CarSelectionMenu carSelectionMenu)
    {
        Debug.Log("Saliendo del estado: Esperando selección");
    }
}
public class CarSelectedState : ICarSelectionState
{
    private GameObject selectedCar;

    public CarSelectedState(GameObject selectedCar)
    {
        this.selectedCar = selectedCar;
    }

    public void EnterState(CarSelectionMenu carSelectionMenu)
    {
        Debug.Log("Estado: Coche seleccionado");
        // Aquí podrías mostrar el coche seleccionado
    }

    public void UpdateState(CarSelectionMenu carSelectionMenu)
    {
        // Lógica mientras el coche está seleccionado
    }

    public void ExitState(CarSelectionMenu carSelectionMenu)
    {
        Debug.Log("Saliendo del estado: Coche seleccionado");
    }
}
public class RaceStartedState : ICarSelectionState
{
    public void EnterState(CarSelectionMenu carSelectionMenu)
    {
        Debug.Log("Estado: Carrera iniciada");
        carSelectionMenu.StartRace(carSelectionMenu.car);
    }

    public void UpdateState(CarSelectionMenu carSelectionMenu)
    {
        // Lógica durante la carrera, si es necesaria
    }

    public void ExitState(CarSelectionMenu carSelectionMenu)
    {
        Debug.Log("Saliendo del estado: Carrera iniciada");
    }
}
