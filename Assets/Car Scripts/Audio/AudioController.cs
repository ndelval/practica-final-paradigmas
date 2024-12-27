using UnityEngine;
using System;

public class CarAudioController : MonoBehaviour
{
    public bool sparkWhileAcceleration;
    [Range(0f, 1f)] public float sparkRate;
    public EngineAudioSetup setup;

    private AudioSource lowAccel, medAccel, highAccel, limit, idleSource, spark1, spark2, spark3;
    public float rpm;
    private void Awake()
    {
        InitializeAudioSources();
        UpdateEngineSetup();
    }

    private void InitializeAudioSources()
    {
        lowAccel = gameObject.AddComponent<AudioSource>();
        medAccel = gameObject.AddComponent<AudioSource>();
        highAccel = gameObject.AddComponent<AudioSource>();
        limit = gameObject.AddComponent<AudioSource>();
        idleSource = gameObject.AddComponent<AudioSource>();
        spark1 = gameObject.AddComponent<AudioSource>();
        spark2 = gameObject.AddComponent<AudioSource>();
        spark3 = gameObject.AddComponent<AudioSource>();

        lowAccel.loop = true;
        medAccel.loop = true;
        highAccel.loop = true;
        limit.loop = true;
        idleSource.loop = true;
    }

    public void UpdateEngineSetup()
    {
        if (setup == null) return;

        sparkRate = setup.sparkRate;
        lowAccel.clip = setup.lowAcceleration.audio;
        medAccel.clip = setup.mediumAcceleration.audio;
        highAccel.clip = setup.highAcceleration.audio;
        limit.clip = setup.limiter.audio;
        idleSource.clip = setup.idle.audio;

        lowAccel.Play();
        medAccel.Play();
        highAccel.Play();
        limit.Play();
        idleSource.Play();
    }

    private void Update()
    {
        UpdateAudioLevels();
    }

    private void UpdateAudioLevels()
    {
        if (setup == null) return;

        lowAccel.volume = setup.lowAcceleration.volumeCurve.Evaluate(rpm);
        lowAccel.pitch = setup.lowAcceleration.pitchCurve.Evaluate(rpm);

        medAccel.volume = setup.mediumAcceleration.volumeCurve.Evaluate(rpm);
        medAccel.pitch = setup.mediumAcceleration.pitchCurve.Evaluate(rpm);

        highAccel.volume = setup.highAcceleration.volumeCurve.Evaluate(rpm);
        highAccel.pitch = setup.highAcceleration.pitchCurve.Evaluate(rpm);

        limit.volume = setup.limiter.volumeCurve.Evaluate(rpm);
        limit.pitch = setup.limiter.pitchCurve.Evaluate(rpm);

        idleSource.volume = setup.idle.volumeCurve.Evaluate(rpm);
        idleSource.pitch = setup.idle.pitchCurve.Evaluate(rpm);
    }
    public void PauseAudio()
    {
    // Pausa todos los audios
        foreach (AudioSource source in GetComponents<AudioSource>())
        {
            source.Pause();
        }
    }

    public void ResumeAudio()
    {
    // Reanuda todos los audios
        foreach (AudioSource source in GetComponents<AudioSource>())
        {
            source.UnPause();
        }
    }

}

