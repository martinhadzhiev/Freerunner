using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    void Awake()
    {
        foreach (var s in sounds)
        {
            var source = gameObject.AddComponent<AudioSource>();
            source.clip = s.clip;

            source.volume = s.volume;
            source.pitch = s.pitch;
            source.loop = s.loop;
            source.playOnAwake = s.playOnAwake;
            source.outputAudioMixerGroup = s.audioMixer;

            s.source = source;
        }
    }

    public void Play(string name)
    {
        var sound = Array.Find(sounds, s => s.name == name);

        if (sound != null)
        {
            sound.source.pitch = UnityEngine.Random.Range(sound.pitch - 0.1f, sound.pitch + 0.2f);
            sound.source.Play();
        }
    }
}