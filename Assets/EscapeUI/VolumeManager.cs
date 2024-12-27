using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeManager : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider;
    public void changeVolume()
    {
        AudioListener.volume = volumeSlider.value;
    }
}
