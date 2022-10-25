using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressedButton : MonoBehaviour
{
    #region isPressed
    private bool isPressed;
    public bool IsPressed => isPressed;
    #endregion

    public void OnPointerDown() // if we hold the button
    {
        isPressed = true;
    }

    public void OnPointerUp() // if we do not hold the button
    {
        isPressed = false;
    }
}

