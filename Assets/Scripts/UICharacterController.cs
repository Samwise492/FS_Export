using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICharacterController : MonoBehaviour
{
    #region left button
    [SerializeField] private PressedButton leftButton;
    public PressedButton LeftButton => leftButton;
    #endregion
    #region right button
    [SerializeField] private PressedButton rightButton;
    public PressedButton RightButton => rightButton;
    #endregion
    #region fire button
    [SerializeField] private Button fireButton;
    public Button FireButton => fireButton;
    #endregion
    #region jump button
    [SerializeField] private Button jumpButton;
    public Button JumpButton => jumpButton;
    #endregion
    #region shadow bomb button
    [SerializeField] private Button shadowBombButton;
    public Button ShadowBombButton => jumpButton;
    #endregion

    void Start()
    {
        Player.Instance.InitUIController(this);
    }

    private void Update()
    {
        if (Player.Instance.BonusShadowBomb == 1)
            shadowBombButton.gameObject.SetActive(true);
    }
}
