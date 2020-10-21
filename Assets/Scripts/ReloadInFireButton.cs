using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReloadInFireButton : MonoBehaviour
{
    [SerializeField] private Button fireButton;
    [SerializeField] private Player player;
    [SerializeField] private Sprite empty_shell;
    [SerializeField] private Sprite shell_1;
    [SerializeField] private Sprite shell_2;
    [SerializeField] private Sprite shell_3;

    // Update is called once per frame
    void Update()
    {
        if (player.shellPool.Count == 3)
        {
            fireButton.image.sprite = shell_3;
        }

        if (player.shellPool.Count == 2)
        {
            fireButton.image.sprite = shell_2;
        }

        if (player.shellPool.Count == 1)
        {
            fireButton.image.sprite = shell_1;
        }

        if (player.shellPool.Count == 0)
        {
            fireButton.image.sprite = empty_shell;
        }
    }
}
