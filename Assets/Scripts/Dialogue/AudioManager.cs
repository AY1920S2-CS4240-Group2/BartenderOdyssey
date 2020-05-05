﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioSource audioSource;
    public AudioSource bgmSource;
    public float fadeTime = 1.0f;
    public bool StillSpeaking { get; set; }

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }

        DialogueRunner dialogueRunner = FindObjectOfType<DialogueRunner>();
        dialogueRunner.AddCommandHandler("fadeOutBgm", FadeOutBgm);
    }

    void Start()
    {
        audioSource = this.GetComponent<AudioSource>();
        StillSpeaking = false;
    }

    [YarnCommand("playBgm")]
    public void PlayBgm(string mood)
    {
        if (System.Enum.TryParse(mood, out SoundEffectType set))
        {
            PlayBgm(set);
        }
        else
        {
            Debug.LogError($"No matching BGM for {mood}");
        }
    }

    public void PlayBgm(SoundEffectType mood)
    {
        AudioClip clip = null;
        switch (mood)
        {
            case SoundEffectType.Normal:
                clip = Resources.Load<AudioClip>(SoundEffects.BgmNormal);
                break;
            case SoundEffectType.Happy:
                clip = Resources.Load<AudioClip>(SoundEffects.BgmHappy);
                break;
            case SoundEffectType.Sad:
                clip = Resources.Load<AudioClip>(SoundEffects.BgmSad);
                break;
            default:
                Debug.LogError($"Invalid BGM {mood.ToString()}");
                return;

        }

        if (clip != null)
        {
            bgmSource.Stop();
            bgmSource.clip = clip;
            bgmSource.loop = true;
            bgmSource.volume = 0.025f;
            bgmSource.Play();
        }
    }

    [YarnCommand("playSoundEffect")]
    public void PlaySoundEffect(string soundEffectString)
    {
        if (System.Enum.TryParse(soundEffectString, out SoundEffectType set))
        {
            PlaySoundEffect(set);
        }
        else
        {
            Debug.LogError($"No matching sound effect for {soundEffectString}");
        }
    }

    public void PlaySoundEffect(SoundEffectType type)
    {
        AudioClip clip = null;
        switch (type)
        {
            case SoundEffectType.SystemFailure:
                clip = Resources.Load<AudioClip>(SoundEffects.SystemFailureClip);
                break;
            case SoundEffectType.Shutdown:
                clip = Resources.Load<AudioClip>(SoundEffects.ShutdownClip);
                break;
            default:
                Debug.LogError($"Invalid sound effect {type.ToString()}");
                return;

        }

        if (clip != null)
        {
            audioSource.Stop();
            audioSource.clip = clip;
            audioSource.loop = true;
            audioSource.volume = 0.1f;
            audioSource.Play();
        }
    }

    public void SpeakWordsOnLoop(List<AudioClip> clips)
    {
        StartCoroutine(DoSpeakWordsOnLoop(clips));
    }

    private IEnumerator DoSpeakWordsOnLoop(List<AudioClip> clips)
    {
        if (clips.Count > 0)
        {
            audioSource.loop = false;
            StillSpeaking = true;
            while (StillSpeaking)
            {
                audioSource.Stop();
                int randIndex = Random.Range(0, clips.Count - 1);
                audioSource.clip = clips[randIndex];
                audioSource.Play();
                
                while (audioSource.isPlaying)
                {
                    yield return null;
                }
            }
        }
    }

    [YarnCommand("fadeOutSoundEffect")]
    public void FadeOutSoundEffect(string fadeTimeString)
    {
        if (float.TryParse(fadeTimeString, out float fade))
            AudioFadeOut(audioSource, fade);
            // StartCoroutine(DoAudioFadeOutYarn(audioSource, fade, onComplete));
        else
            Debug.LogError($"Invalid fade time {fadeTimeString[1]}");
    }

    public void FadeOutBgm(string[] parameters, System.Action onComplete)
    {
        if (float.TryParse(parameters[1], out float fade))
            StartCoroutine(DoAudioFadeOutYarn(bgmSource, fade, onComplete));
        else
            Debug.LogError($"Invalid fade time {parameters[1]}");
    }

    public void AudioFadeOut(AudioSource audio, float fade)
    {
        StartCoroutine(DoAudioFadeOut(audio, fade));
    }

    private IEnumerator DoAudioFadeOut(AudioSource audio, float fade)
    {
        float startVolume = audio.volume;

        while (audio.volume > 0)
        {
            audio.volume -= startVolume * Time.deltaTime / fade;
            yield return null;
        }

        audio.Stop();
        audio.volume = startVolume;
        audio.loop = false;
    }

    private IEnumerator DoAudioFadeOutYarn(AudioSource audio, float fade, System.Action onComplete)
    {
        yield return StartCoroutine(DoAudioFadeOut(audio, fade));
        onComplete();
    }

    // private IEnumerator DoAudioFadeOut(AudioSource audio, float fade, System.Action onComplete)
    // {
    //     DoAudioFadeOut(audio, fade);
    //     onComplete();
    // }
}
