using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class PopUpText : MonoBehaviour
{
    [SerializeField] private string text;
    [SerializeField] private TextMesh textMesh;
    private float fadeValue = 0;
    private bool isFading = false;

    private void Start()
    {
        textMesh.text = text;
        textMesh.color = new Color(textMesh.color.r, textMesh.color.g, textMesh.color.b, fadeValue);
        StartCoroutine(RaycastCheck());
    }

    private void Update()
    {            
        if (isFading)
            fadeValue += 0.01f;
        if (fadeValue >= 1)
            isFading = false;
        if (isFading == false && fadeValue > 0)
            fadeValue -= 0.01f;
        if (fadeValue == 0)
            return;
        textMesh.color = new Color(textMesh.color.r, textMesh.color.g, textMesh.color.b, fadeValue);
    }

    IEnumerator RaycastCheck()
    {
        
        while (true)
        {
#if UNITY_EDITOR
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);

                if (hit.collider != null && hit.transform.gameObject.CompareTag("Easter"))
                {
                    isFading = true;
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

                    if (hit.collider != null && hit.transform.gameObject.CompareTag("Easter"))
                    {
                        isFading = true;
                    }
                }
            }
            yield return new WaitForFixedUpdate();
        }

    }
}