using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public AudioSource bgmSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySFX(AudioClip sfx, float volume = 1)
    {
        GameObject o = new GameObject(sfx.name);
        AudioSource source = o.AddComponent<AudioSource>();
        source.clip = sfx;
        source.volume = volume;
        source.Play();
        Destroy(o, sfx.length);
    }

    public void PlayBGM(AudioClip bgm)
    {
        bgmSource.Stop();
        bgmSource.clip = bgm;
        bgmSource.loop = true;
        bgmSource.Play();
    }
}
