using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DynamicFillAmount : MonoBehaviour
{
    public bool match, delayedMatch;
    public Image healthBar;

    public float delay;
    public float fillSpeed = 1f;

    Image img;
    bool doingMatch;

    // Start is called before the first frame update
    void Start()
    {
        img = this.GetComponent<Image>();

        if (match)
        {
            this.GetComponent<Image>().fillAmount = healthBar.fillAmount;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (match)
        {
            if (healthBar.fillAmount <= .5f)
            {
                img.enabled = true;
                img.fillAmount = healthBar.fillAmount;
            }
            else
            {
                img.enabled = false;
            }
        }
        else if (delayedMatch)
        {
            if ((img.fillAmount > healthBar.fillAmount) && !doingMatch)
            {
                StartCoroutine(DoMatch());
            }
        }
    }

    private IEnumerator DoMatch()
    {
        doingMatch = true;
        yield return new WaitForSeconds(delay);
        while(img.fillAmount > healthBar.fillAmount)
        {
            img.fillAmount -= (img.fillAmount - healthBar.fillAmount) * Time.deltaTime * fillSpeed;
            yield return null;
        }

        doingMatch = false;
    }
}
