using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineHighlight : MonoBehaviour
{
    Material shader;
    public float maxValue;
    public float minValue;
    public float speed;
    bool isIncreasing;
    float value;

    // Start is called before the first frame update
    void Start()
    {
        shader = gameObject.GetComponent<Renderer>().sharedMaterial;
        StartCoroutine(Highlight());
    }

    IEnumerator Highlight()
    {
        while (true)
        {
            if (isIncreasing == false)
            {
                value -= speed;
                shader.SetFloat("_Thickness", value);
                if (value <= minValue)
                    isIncreasing = true;
                    
            }
            if (isIncreasing)
            {
                value += speed;
                shader.SetFloat("_Thickness", value);
                if (value >= maxValue)
                    isIncreasing = false;
                    
            }

            yield return new WaitForEndOfFrame();
        }
    }
}
