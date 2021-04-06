using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trader : MonoBehaviour
{
    [SerializeField] private GameObject shopPanel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);

            if (hit.collider != null && hit.transform.gameObject.CompareTag("Trader"))
            {
                if (Time.timeScale > 0)
                {
                    shopPanel.gameObject.SetActive(true);
                    Time.timeScale = 0;
                }
                else
                {
                    shopPanel.gameObject.SetActive(false);
                    Time.timeScale = 1;
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

                if (hit.collider != null && hit.transform.gameObject.CompareTag("Trader"))
                {
                    if (Time.timeScale > 0)
                    {
                        shopPanel.gameObject.SetActive(true);
                        Time.timeScale = 0;
                    }
                    else
                    {
                        shopPanel.gameObject.SetActive(false);
                        Time.timeScale = 1;
                    }
                }
            }
        }
    }
}
