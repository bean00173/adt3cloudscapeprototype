using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DoAudio : MonoBehaviour
{
    [SerializeField] private UnityEvent InvokeCase;
    private PlayerController pc;

    private void Start()
    {
        pc = GetComponentInParent<PlayerController>();   
    }

    public void DoInvoke()
    {
        InvokeCase?.Invoke();
    }
    public void DoAudioPlay(int index)
    {
        pc.PlayAudio(index);
    }

    public void PlayAudioThis()
    {
        this.GetComponent<AudioSource>().Play();
    }
}
