using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ClipList
{
    public string name;
    public SoundList clip;
}
[System.Serializable]
public class SoundList
{
    public List<AudioClip> clips;
}

public class SoundHandler : MonoBehaviour
{
    AudioSource source;
    //public SoundClip[] soundClips;
    public List<ClipList> soundClipsList;

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
        //if(source.clip != soundClipsList.Find(x => x.name == clip).clip){
        //    source.clip = soundClipsList.Find(x => x.name == clip).clip;
        //}

        SoundList soundList = soundClipsList.Find(x => x.name == clip).clip;

        source.clip = soundList.clips[Random.Range(0, soundList.clips.Count)];
        
        source.Play();
    }
}
