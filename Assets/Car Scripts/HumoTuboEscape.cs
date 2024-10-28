using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumoTuboEscape : MonoBehaviour
{
    public ParticleSystem leftExhaustParticles; 
    public ParticleSystem rightExhaustParticles; 
    public PlayerCarController carController; 

    void Update()
    {
        bool shouldPlayParticles = carController.speed > 1f;

        
        if (shouldPlayParticles && !leftExhaustParticles.isPlaying)
        {
            leftExhaustParticles.Play();
        }
        else if (!shouldPlayParticles && leftExhaustParticles.isPlaying)
        {
            leftExhaustParticles.Stop();
        }

        
        if (shouldPlayParticles && !rightExhaustParticles.isPlaying)
        {
            rightExhaustParticles.Play();
        }
        else if (!shouldPlayParticles && rightExhaustParticles.isPlaying)
        {
            rightExhaustParticles.Stop();
        }
    }
}

