using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class ControlInputManager : MonoBehaviour
{
    public PlayerCarController playerCarController;
    public TMP_Text forwardKeyText, backwardKeyText, leftKeyText, rightKeyText, brakeKeyText;

    private string currentAction;
    private bool waitingForKey = false;

    void Start()
    {
        UpdateButtonLabels();
    }


    public void AssignKey(string action)
    {
        currentAction = action;
        waitingForKey = true;

    }

    void Update()
    {

        if (waitingForKey && Input.anyKeyDown)
        {


            // Recorre todas las teclas posibles para verificar cuál fue presionada
            foreach (KeyCode keyCode in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(keyCode))
                {

                    playerCarController.keyBindings[currentAction] = keyCode;
                    UpdateButtonLabels();
                    waitingForKey = false;
                    break;
                }
            }
        }
    }

    void UpdateButtonLabels()
    {

        forwardKeyText.text = playerCarController.keyBindings["forward"].ToString();
        backwardKeyText.text = playerCarController.keyBindings["backward"].ToString();
        leftKeyText.text = playerCarController.keyBindings["left"].ToString();
        rightKeyText.text = playerCarController.keyBindings["right"].ToString();
        brakeKeyText.text = playerCarController.keyBindings["brake"].ToString();
    }
}
