using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICharacterController : MonoBehaviour
{
    #region left
    [SerializeField] private PressedButton left;
    public PressedButton Left => left;
    #endregion
    #region right
    [SerializeField] private PressedButton right;
    public PressedButton Right => right;
    #endregion
    #region fire
    [SerializeField] private Button fire;
    public Button Fire => fire;
    #endregion
    #region jump
    [SerializeField] private Button jump;
    public Button Jump => jump;
    #endregion

    void Start()
    {
        Player.Instance.InitUIController(this);
    }
}
