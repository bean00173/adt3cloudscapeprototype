using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(SoundHandler))]
public class AmbienceSFX : FadedSoundEffect
{
    public string clipName;

    // Start is called before the first frame update
    public override void Start()
    {
        _baseVol = this.GetComponent<AudioSource>().volume;

        StartCoroutine(WaitForLoad());

        handler = this.GetComponent<SoundHandler>();
        while(handler.source.clip == null)
        {
            try
            {
                handler.PlaySound(clipName);
            }
            catch(Exception e)
            {
                Debug.LogError($"{e.GetType()} : Incorrect Clip Name?");
                break;
            }
        }

        if (_fade)
        {
            StartCoroutine(Timer());
        }
    }

    // Update is called once per frame
    public override void Update()
    {
        
    }

    private IEnumerator WaitForLoad()
    {
        yield return new WaitUntil(() => AudioManager.instance.loaded == true);
    }
}
