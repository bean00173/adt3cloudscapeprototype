using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SoundHandler))]
public class MusicManager : MonoBehaviour
{
    private GameState currentState;
    private AudioSource source;
    private SoundHandler handler;

    // Start is called before the first frame update
    void Start()
    {
        source = this.GetComponent<AudioSource>();
        handler = this.GetComponent<SoundHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        currentState = AudioManager.currentState;

        if (source.clip == null || !source.clip.name.StartsWith(currentState.ToString()))
        {
            handler.PlayRandomSound(currentState.ToString());
        }
    }

}
