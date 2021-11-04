using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    #region Singleton
    public static AudioManager instance;

    void Awake() {
        if(instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        foreach(Sound s in sounds) {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop; 
        }
    }
    #endregion

    //Play sounds you'd want to play at the start of the stage
    void Start() {
        Play("BackgroundMusic");
    }

    public void Play(string name) {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null) {
            Debug.Log("Tryna play a sound that aint there buddy. Check sound name: " + name + " !");
        } else {
            Debug.Log("Playing sound: " + name);
            s.source.Play();
        }
    }

    public static void StopMusic() {
        Debug.Log("stopping moosic");
        // Destroy(this);
    }
}
