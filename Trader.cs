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

    private void Start()
    {
        StartCoroutine(CheckForClick());
    }

    public void OnPressExit()
    {
        shopPanel.gameObject.SetActive(false);
    }

    IEnumerator CheckForClick()
    {
        while (true)
        {
#if UNITY_EDITOR
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);

                if (hit.collider != null && hit.collider.isTrigger == false && TalkRange.Instance.isAllowedToTalk)
                {
                    if (hit.transform.gameObject.CompareTag("Trader"))
                    {
                        shopPanel.gameObject.SetActive(true);
                    }
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

                    if (hit.collider != null && hit.transform.gameObject.CompareTag("Trader") && TalkRange.Instance.isAllowedToTalk)
                    {
                        shopPanel.gameObject.SetActive(true);
                    }
                }
            }
            yield return new WaitForSeconds(0.005f);
        }
    }

}
