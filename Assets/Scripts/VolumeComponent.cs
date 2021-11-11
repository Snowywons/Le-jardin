using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class VolumeComponent : MonoBehaviour
{
    public AudioMixer mixer;
    // Start is called before the first frame update
    public void SetMusicVolume(float volume)
    {
        mixer.SetFloat("MusicVolume", volume);
        Debug.Log("Changement musique: " + volume);
    }
    public void SetEffectsVolume(float volume)
    {
        mixer.SetFloat("EffectsVolume", volume);
        Debug.Log("Changement effets: " + volume);
    }
}
