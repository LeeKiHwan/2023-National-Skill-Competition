using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    public AudioSource bgm;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    public void PlaySFX(AudioClip sfx, bool isLoop)
    {
        GameObject obj = new GameObject(sfx.name);
        AudioSource source = obj.AddComponent<AudioSource>();
        source.loop = isLoop;
        source.clip = sfx;
        source.Play();

        if (!isLoop) Destroy(obj, sfx.length);
    }

    public void PlayBGM(AudioClip sfx)
    {
        bgm.clip = sfx;
        bgm.Play();
    }
}
