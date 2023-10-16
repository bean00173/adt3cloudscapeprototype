using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SoundHandler))]
public class MusicManager : FadedSoundEffect
{
    private GameState currentState;
    
    // Start is called before the first frame update
    public override void Start()
    {
        _baseVol = this.GetComponent<AudioSource>().volume;

        source = this.GetComponent<AudioSource>();
        handler = this.GetComponent<SoundHandler>();

        if (_fade)
        {
            StartCoroutine(Timer());
        }
    }

    // Update is called once per frame
    public override void Update()
    {
        currentState = AudioManager.currentState;

        if (source.clip == null || !source.isPlaying)
        {
            handler.PlayRandomSound(currentState.ToString());
        }
        else if (!source.clip.name.StartsWith(currentState.ToString()))
        {
            LeavingMenu();
            handler.PlayRandomSound(currentState.ToString());
        }
    }

}
