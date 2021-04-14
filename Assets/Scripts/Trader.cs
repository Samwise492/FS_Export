using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trader : MonoBehaviour
{
    [SerializeField] private GameObject shopPanel;
    [SerializeField] private Animator animator;
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        animator.SetTrigger("Greetings");
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        animator.SetTrigger("Greetings");
    }

    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);

            if (hit.collider != null && hit.collider.isTrigger == false)
                if (hit.transform.gameObject.CompareTag("Trader"))
                {
                    shopPanel.gameObject.SetActive(true);
                }
           

        }
#endif
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);

                if (hit.collider != null && hit.transform.gameObject.CompareTag("Trader"))
                {
                    shopPanel.gameObject.SetActive(true);
                }
            }
        }
    }

    public void OnPressedExit()
    {
        shopPanel.gameObject.SetActive(false);
    }
}
