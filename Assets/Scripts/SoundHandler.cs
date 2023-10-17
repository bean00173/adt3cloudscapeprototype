using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

[RequireComponent(typeof(AudioSource))]
public class SoundHandler : MonoBehaviour
{
    public AudioType sourceType;
    public AudioSource source;

    public UnityEvent playOnAwake = new UnityEvent();

    // Start is called before the first frame update
    void Start()
    {
        playOnAwake?.Invoke();
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

    public void PlaySoundRandomChance(string clip, int probability)
    {
        try
        {
            if (GameManager.instance.RandomChance(probability))
            {
                DoPlay(AudioManager.instance.GetClip(sourceType, clip));
            }
        }
        catch (Exception e)
        {
            Debug.LogWarning($"{e.GetType()} : AudioClip with name => {clip} | Could Not Be Found. Try A Different Name?");
        }
    }

    private void DoPlay(AudioClip clip)
    {
        source.clip = clip;
        source.Play();
    }

}
