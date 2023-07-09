using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;


public class VolumeMixing : MonoBehaviour
{
  
    public AudioMixer audioMixer;

   
    public void ChangeMasterVolume(float volume)
    {
        audioMixer.SetFloat("Master", volume);
    }
    public void ChangeMusicVolume(float volume)
    {
        audioMixer.SetFloat("BGM", volume);
    }
    public void ChangeSFXVolume(float volume)
    {
        audioMixer.SetFloat("SFX", volume);

    }
}
