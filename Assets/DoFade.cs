using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoFade : MonoBehaviour
{
    Image image;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();

        StartCoroutine(FadeAlpha());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator FadeAlpha()
    {
        float alpha = 0;

        while (alpha != 1)
        {
            alpha += 0.01f;
            image.color += new Color(1, 1, 1, alpha);
            yield return new WaitForSeconds(0.015f);
        }

    }
}
