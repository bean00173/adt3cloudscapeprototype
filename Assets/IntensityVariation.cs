using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntensityVariation : MonoBehaviour
{
    private Renderer mr;
    public Color _emissionColor = Color.red;
    
    // Start is called before the first frame update
    void Start()
    {
        mr = this.GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {

        mr.material.SetColor("_EmissionColor", _emissionColor * Mathf.Lerp(5, 100, Time.deltaTime));
    }
}
