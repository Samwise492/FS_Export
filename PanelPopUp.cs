using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelPopUp : MonoBehaviour
{
    [SerializeField] private GameObject canvas;
    [SerializeField] private string objectName;
    public GameObject additionalWindow;

    void Start()
    {
        StartCoroutine(CheckForClick(objectName));
    }

    public void ShutTheWindow()
    {
        canvas.SetActive(false);
        Time.timeScale = 1;
    }
    public void ShowAdditionalWindow()
    {
        StartCoroutine(AdditionalWindowPopUp());
    }
    IEnumerator CheckForClick(string objectName)
    {
        while (true)
        {
#if UNITY_EDITOR
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);

                if (hit.collider != null && hit.transform.gameObject.name == objectName)
                {
                    canvas.gameObject.SetActive(true);
                    if (hit.transform.gameObject.name != "Resurrection point (Tutorial)")
                    {
                        if (hit.transform.gameObject.name != "Resurrection point")
                        {
                            Time.timeScale = 0;
                        }
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

                    if (hit.collider != null && hit.transform.gameObject.name == objectName)
                    {
                        canvas.gameObject.SetActive(true);
                        if (hit.transform.gameObject.name != "Resurrection point (Tutorial)")
                        {
                            if (hit.transform.gameObject.name != "Resurrection point")
                            {
                                Time.timeScale = 0;
                            }
                        }
                    }
                }
            }
            
            yield return null;
        }
    }
    IEnumerator AdditionalWindowPopUp()
    {
        if(additionalWindow != null)
        {
            additionalWindow.SetActive(true);
            yield return new WaitForSeconds(1);
            additionalWindow.SetActive(false);
            canvas.SetActive(false);
            yield break;
        }
    }
}
