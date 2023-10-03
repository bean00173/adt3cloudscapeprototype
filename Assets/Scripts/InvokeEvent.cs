using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InvokeEvent : MonoBehaviour
{
    public UnityEvent playableEvent;
    public UnityEvent alternateEvent;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayEvent()
    {
        playableEvent.Invoke();
    }

    public void PlayAlt()
    {
        alternateEvent.Invoke();
    }
}
