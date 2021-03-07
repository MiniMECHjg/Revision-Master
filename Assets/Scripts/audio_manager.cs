using UnityEngine.Audio;
using System;
using UnityEngine;


public class audio_manager : MonoBehaviour
{

    public Sound[] sounds;

    public static audio_manager instance;


    // Start is called before the first frame update
    void Awake()
    {
        
        if (instance == null)
        {
            instance = this;
        }else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);

        foreach(Sound s in sounds)
        {
            //gets the parameters of the sounds
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    //this sound it to be played if user gets a question right
    public void correct()
    {
        Play("Correct");
    }

    //sound played if user gets a question wrong
    public void wrong()
    {
        Play("Wrong");
    }

    //this function plays the sound with the name name
    public void Play(string name)
    {
        //finds the sound with the name name
        Sound s = Array.Find(sounds, sound => sound.name == name);
        //plays the sound
        s.source.Play();
    }
}
