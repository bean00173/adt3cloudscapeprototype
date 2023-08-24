using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialSwitcher : MonoBehaviour
{

    public Material[] materials;
    Renderer rend;
    bool matGenerated;

    // Start is called before the first frame update
    void Start()
    {
        rend = this.GetComponent<Renderer>();

        if (materials.Length > 0)
        {
            rend.material = materials[Random.Range(0, materials.Length)];
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
