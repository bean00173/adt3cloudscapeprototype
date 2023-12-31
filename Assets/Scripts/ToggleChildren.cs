using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleChildren : MonoBehaviour
{
    int index = 0;

    public bool arrowKeys;
    public GameObject prev, next;

    // Start is called before the first frame update
    void Start()
    {
        foreach(Transform child in transform){
            child.gameObject.SetActive(false);
        }

        if (arrowKeys)
        {
            index = Random.Range(0, this.transform.childCount);
        }

        transform.GetChild(index).gameObject.SetActive(true);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (arrowKeys)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                CycleToNext();
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                CycleToPrevious();
            }
        }
    }

    public void CycleToNext()
    {
        if(index == transform.childCount - 1)
        {
            index = 0;
        }
        else
        {
            index++;
        }

        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }

        transform.GetChild(index).gameObject.SetActive(true);
    }

    public void CycleToPrevious()
    {
        if (index == 0)
        {
            index = transform.childCount - 1;
        }
        else
        {
            index--;
        }

        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }

        transform.GetChild(index).gameObject.SetActive(true);
    }
}
