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
    public Vector3 lStick;
    public Vector3 rStick;

    //aƒ{ƒ^ƒ“
    public bool aButtonTrigger;
    public bool aButtonHold;
    public bool aButtonRelease;

    //rTrigger
    public bool rTriggerTrigger;
    public bool rTriggerHold;
    public bool rTriggerRelease;

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

        //aButton
        input.Player.aButton.started += OnAButton;
        input.Player.aButton.performed += OnAButton;
        input.Player.aButton.canceled += OnAButton;

        //rTrigger
        input.Player.rTrigger.started += OnRTrigger;
        input.Player.rTrigger.performed += OnRTrigger;
        input.Player.rTrigger.canceled += OnRTrigger;

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

        rTriggerRelease = false;
        rTriggerTrigger = false;
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        lStick.x = context.ReadValue<Vector2>().normalized.x;
        lStick.z = context.ReadValue<Vector2>().normalized.y;
    }

    private void OnAim(InputAction.CallbackContext context)
    {
        rStick.x = context.ReadValue<Vector2>().normalized.x;
        rStick.z = context.ReadValue<Vector2>().normalized.y;
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

    private void OnRTrigger(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            rTriggerTrigger = true;

            rTriggerHold = true;
        }

        if (context.canceled)
        {
            rTriggerHold = false;
            rTriggerRelease = true;
        }
    }
}
