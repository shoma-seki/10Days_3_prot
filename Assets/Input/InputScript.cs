using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputScript : MonoBehaviour
{
    //input
    PlayerInput input;

    //public
    //stick
    public Vector2 lStick;
    public Vector2 rStick;

    //aƒ{ƒ^ƒ“
    public bool aButtonTrigger;
    public bool aButtonHold;
    public bool aButtonRelease;

    // Start is called before the first frame update
    void Start()
    {
        input = new PlayerInput();

        //Move
        input.Player.LStick.started += OnMove;
        input.Player.LStick.performed += OnMove;
        input.Player.LStick.canceled += OnMove;

        //Aim
        input.Player.RStick.started += OnAim;
        input.Player.RStick.performed += OnAim;
        input.Player.RStick.canceled += OnAim;

        //Roll
        input.Player.aButton.started += OnAButton;
        input.Player.aButton.performed += OnAButton;
        input.Player.aButton.canceled += OnAButton;

        input.Enable();
    }

    private void Update()
    {
        //Debug.Log("lStick = " + lStick);
        //Debug.Log("rStick = " + rStick);
        //Debug.Log("aButtonTrigger = " + aButtonTrigger);
        //Debug.Log("aButtonHold = " + aButtonHold);
        //Debug.Log("aButtonRelease = " + aButtonRelease);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        aButtonTrigger = false;
        aButtonRelease = false;
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        lStick = context.ReadValue<Vector2>().normalized;
    }

    private void OnAim(InputAction.CallbackContext context)
    {
        rStick = context.ReadValue<Vector2>().normalized;
    }

    private void OnAButton(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            aButtonTrigger = true;

            aButtonHold = true;
        }

        if (context.canceled)
        {
            aButtonHold = false;
            aButtonRelease = true;
        }
    }
}
