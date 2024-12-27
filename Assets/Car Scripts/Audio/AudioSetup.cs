using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Engine", menuName = "Car/EngineSound", order = 3)]
public class EngineAudioSetup : ScriptableObject
{
    public CarAudio idle, lowAcceleration, mediumAcceleration, highAcceleration, limiter, spark1, spark2, spark3;
    [Range(0f, 1f)] public float sparkRate = 0.8f;
}

