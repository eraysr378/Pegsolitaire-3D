using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
public class VolumeManager : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider volumeSlider;
    private void Start()
    {
        volumeSlider.value = SoundManagerScript.CurrentVolume;
    }
    //when 
    public void SetVolume(float volume)
    {
        // decibels are different unit, to make that suitable for our slider some mathematical operations are made
        audioMixer.SetFloat("volume", Mathf.Log10(volume)*20);
        SoundManagerScript.CurrentVolume = volume;
    }
}
