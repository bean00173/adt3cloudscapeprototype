using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SoundHandler))]
public class AmbienceSFX : MonoBehaviour
{
    public string clipName;

    public bool _fade;
    public float _fadeSpeed;
    public float _fadeTime;

    private float _baseVol;

    bool doFade;

    SoundHandler _soundHandler;

    // Start is called before the first frame update
    void Start()
    {
        _baseVol = this.GetComponent<AudioSource>().volume;

        StartCoroutine(WaitForLoad());

        _soundHandler = this.GetComponent<SoundHandler>();
        while(_soundHandler.source.clip == null)
        {
            _soundHandler.PlaySound(clipName);
        }

        if (_fade)
        {
            StartCoroutine(Timer());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator WaitForLoad()
    {
        yield return new WaitUntil(() => AudioManager.instance.loaded == true);
    }

    private void DoFade(bool fade)
    {
        switch (fade)
        {
            case true:
                StartCoroutine(Fade(0.01f));
                break;
            case false:
                StartCoroutine(Fade(1f));
                break;
        }
    }

    private IEnumerator Fade(float val)
    {
        float startVol = _soundHandler.source.volume;

        float time = 0;

        while(time < _fadeSpeed)
        {
            _soundHandler.source.volume = Mathf.Lerp(startVol, _baseVol * val, time / _fadeSpeed);
            time += Time.deltaTime;
            yield return null;
        }
    
        _soundHandler.source.volume = val == 1f ? 1f : 0.01f;

        StartCoroutine(Timer());
    }

    private IEnumerator Timer()
    {
        yield return new WaitForSeconds(_fadeTime);
        doFade = !doFade;
        DoFade(doFade);
    }
}
