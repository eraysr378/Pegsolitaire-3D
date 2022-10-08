using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class SoundManagerScript : MonoBehaviour
{
    public static float CurrentVolume = 0.5f;
    public static AudioClip audioClip;
    static AudioSource audioSource;
    public AudioMixer mixer;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        mixer.SetFloat("volume", Mathf.Log10(CurrentVolume) * 20);
    }
    //play the sound
    public static void PlaySound()
    {
        audioClip = Resources.Load<AudioClip>("BalloonExplosionSound");
        audioSource.clip = audioClip;
        audioSource.Play();
    }
    public static void PlayWrongMoveSound()
    {
        audioClip = Resources.Load<AudioClip>("WrongMoveSound");
        audioSource.clip = audioClip;
        audioSource.Play();
    }
}
