using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager am;
    public Sound[] sounds;

    private void Awake()
    {
        am = this;

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.outputAudioMixerGroup = s.output;
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.playOnAwake = s.playOnAwake;
            s.source.loop = s.loop;
        }
    }

    private void Start()
    {
        Invoke(nameof(BgMusicDelay), 8.5f);
    }

    private void BgMusicDelay()
    {
        //int rand = UnityEngine.Random.Range(0, 2);
        //if (rand == 0)
        //{
        //    Play("BGMusic");
        //}
        //else
        //{
        //    Play("BGPlay");
        //}
        Play(SoundBGM);
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, s => s.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sounds name: " + name + " not found!");
            return;
        }
        s.source.Play();
    }

    public void Pause(string name)
    {
        Sound s = Array.Find(sounds, s => s.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sounds name: " + name + " not found!");
            return;
        }
        s.source.Pause();
    }

    public void UnPause(string name)
    {
        Sound s = Array.Find(sounds, s => s.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sounds name: " + name + " not found!");
            return;
        }
        s.source.UnPause();
    }

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, s => s.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sounds name: " + name + " not found!");
            return;
        }
        s.source.Stop();
    }

    public static void ClickSound()
    {
        am.Play(SoundClick);
    }

    public const string SoundBGM = "BGM";
    public const string SoundClick = "Click";

    public const string MixerUI = "UI";
    public const string MixerBGM = "BGM";
    public const string MixerSFX = "SFX";
}

[Serializable]
public class Sound
{
    public string name = "Sound Name";
    public AudioClip clip;
    public AudioMixerGroup output;
    [Range(0f, 1f)]
    public float volume = 1f;
    [Range(0.1f, 3f)]
    public float pitch = 1f;
    [HideInInspector]
    public AudioSource source;
    public bool playOnAwake;
    public bool loop;
}
