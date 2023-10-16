using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlash : MonoBehaviour
{
    public float waitTime = .25f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Disable());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator Disable()
    {
        yield return new WaitForSeconds(waitTime);

        Destroy(this.gameObject);
    }
}
