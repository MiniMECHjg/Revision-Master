using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound
{
    //adds properties to sound clips to be adjustable

    public string name;
    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume;
    [Range(0f, 1f)]
    public float pitch;

    public bool loop;

    [HideInInspector]
    public AudioSource source;
}