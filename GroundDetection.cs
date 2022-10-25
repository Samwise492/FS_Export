using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundDetection : MonoBehaviour
{
    #region isGrounded
    [SerializeField] private bool isGrounded; //находимся мы на земле или нет
    public bool IsGrounded => isGrounded;
    #endregion isGrounded

    private void OnCollisionStay2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Platform"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Platform"))
        {
            isGrounded = false;
        }
    }
}
