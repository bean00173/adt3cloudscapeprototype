using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ParticleEvents : MonoBehaviour
{
    private ParticleSystem _system;
    private int _currentNumberOfParticles = 0;

    public UnityEvent onBirth = new UnityEvent();
    public UnityEvent onDeath = new UnityEvent();

    // Start is called before the first frame update
    void Start()
    {
        _system = this.GetComponent<ParticleSystem>();
        if( _system == null)
        {
            Debug.LogError("Missing Particle System!", this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        int amount = Mathf.Abs(_currentNumberOfParticles - _system.particleCount);

        if(_system.particleCount < _currentNumberOfParticles)
        {
            onDeath.Invoke();
            Debug.Log("Playing Birth Sound/s");
        }

        if(_system.particleCount > _currentNumberOfParticles)
        {
            onBirth.Invoke();
            Debug.Log("Playing Death Sound/s");
        }

        _currentNumberOfParticles = _system.particleCount;
    }


}
