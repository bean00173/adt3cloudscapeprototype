using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ParticleEvents : MonoBehaviour
{
    private ParticleSystem _system;
    private int _currentNumberOfParticles = 0;

    public UnityEvent onBirth = new UnityEvent();
    public UnityEvent onBurst = new UnityEvent();
    public UnityEvent onDeath = new UnityEvent();

    [Tooltip("Set this to be the same as the time between particle start and max size from SizeOverLifetime")]
    public float sfxDelay;

    // Start is called before the first frame update
    void Start()
    {
        _system = this.GetComponent<ParticleSystem>();
        if (_system == null)
        {
            Debug.LogError("Missing Particle System!", this);
        }

    }

    //Update is called once per frame
    void Update()
    {
        Debug.LogWarning(_system.particleCount + " : " + _currentNumberOfParticles);
        
        if (_system.particleCount < _currentNumberOfParticles)
        {
            Timer(sfxDelay);
            onDeath.Invoke();
        }

        if (_system.particleCount > _currentNumberOfParticles + 5)
        {
            Timer(sfxDelay);
            onBurst.Invoke();
        }
        else if (_system.particleCount > _currentNumberOfParticles)
        {
            Timer(sfxDelay);
            onBirth.Invoke();
        }

        _currentNumberOfParticles = _system.particleCount;
    }

    private IEnumerator Timer(float time)
    {
        yield return new WaitForSeconds(time);
    }
}
