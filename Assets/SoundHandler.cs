using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SoundClip
{
    public string name;
    public AudioClip clip;
}

public class SoundHandler : MonoBehaviour
{
    AudioSource source;
    //public SoundClip[] soundClips;
    public List<SoundClip> soundClipsList;

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
        source.clip = soundClipsList.Find(x => x.name == clip).clip;
        
        source.Play();
    }
}
