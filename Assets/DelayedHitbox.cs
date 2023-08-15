using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class DelayedHitbox : MonoBehaviour
{

    public Transform hitbox;

    // Start is called before the first frame update
    void Start()
    {
        hitbox.gameObject.SetActive(false);
        StartCoroutine(Timer());
    }

    // Update is called once per frame
    void Update()
    {

    }

    private IEnumerator Timer()
    {
        yield return new WaitForSeconds(3.0f);

        hitbox.gameObject.SetActive(true);
    }
}
