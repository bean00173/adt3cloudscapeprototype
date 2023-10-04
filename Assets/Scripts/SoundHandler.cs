using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(AudioSource))]
public class SoundHandler : MonoBehaviour
{
    public AudioType sourceType;
    AudioSource source;

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();

        //foreach(SoundClip clip in soundClips)
        //{
        //    soundClipsList.Add(clip);
        //}
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaySound(string clip)
    {
        try
        {
            DoPlay(AudioManager.instance.GetClip(sourceType, clip));
        }
        catch(Exception e)
        {
            Debug.LogWarning($"{e.GetType()} : AudioClip with name => {clip} | Could Not Be Found. Try A Different Name?");
        }
    }

    public void PlayRandomSound(string prefix)
    {
        try
        {
            DoPlay(AudioManager.instance.GetRandomClip(sourceType, prefix));
            
        }
        catch (Exception e)
        {
            Debug.LogWarning($"{e.GetType()} : AudioClip with prefix => {prefix} | Could Not Be Found. Try A Different Prefix?");
        }
    }

    private void DoPlay(AudioClip clip)
    {
        source.clip = clip;
        source.Play();
    }

}
