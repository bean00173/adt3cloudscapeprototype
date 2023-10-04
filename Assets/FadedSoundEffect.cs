using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadedSoundEffect : MonoBehaviour
{
    protected AudioSource source;
    protected SoundHandler handler;

    public bool _fade;
    public float _fadeSpeed;
    public float _fadeTime;

    protected float _baseVol;

    protected bool doFade;
    // Start is called before the first frame update
    public virtual void Start()
    {
        
    }

    // Update is called once per frame
    public virtual void Update()
    {
        
    }

    public void LeavingMenu()
    {
        _fadeSpeed = 3f;
        StartCoroutine(Fade(0f));
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
        float startVol = handler.source.volume;

        float time = 0;

        while (time < _fadeSpeed)
        {
            handler.source.volume = Mathf.Lerp(startVol, _baseVol * val, time / _fadeSpeed);
            time += Time.deltaTime;
            yield return null;
        }

        handler.source.volume = val == 1f ? 1f : 0.01f;

        StartCoroutine(Timer());
    }

    protected IEnumerator Timer()
    {
        yield return new WaitForSeconds(_fadeTime);
        doFade = !doFade;
        DoFade(doFade);
    }
}
