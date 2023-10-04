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

        StartCoroutine(Timer());
    }

    // Update is called once per frame
    public override void Update()
    {
        currentState = AudioManager.currentState;

        if (source.clip == null || !source.clip.name.StartsWith(currentState.ToString()))
        {
            handler.PlayRandomSound(currentState.ToString());
        }
    }

}
