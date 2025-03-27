using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesControl : MonoBehaviour
{
    [SerializeField] ParticleSystem snowParticle;
    [SerializeField] ParticleSystem rainParticle;
    public static ParticlesControl instance;
    public bool isParticleActive;
    private void Awake()
    {
        instance = this;
    }
    public void TurnOnSnow()
    {
        snowParticle.Play();
        isParticleActive = true;
    }

    public void TurnOffSnow()
    {
        snowParticle.Stop();
        isParticleActive = false;
    }

    public void TurnOnRain()
    {
        rainParticle.Play();
        isParticleActive = true;
    }

    public void TurnOffRain()
    {
        rainParticle.Stop();
        isParticleActive = false;
    }

    public bool IsParticleActive()
    {
        return isParticleActive;
    }

}
