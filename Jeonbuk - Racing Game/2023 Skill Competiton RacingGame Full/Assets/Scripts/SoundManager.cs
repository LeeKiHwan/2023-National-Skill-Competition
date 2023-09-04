using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    public AudioSource bgm;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    public void PlaySound(AudioClip sfx, bool isLoop)
    {
        GameObject sfxObj = new GameObject(sfx.name);
        AudioSource audioSource = sfxObj.AddComponent<AudioSource>();
        audioSource.clip = sfx;
        audioSource.loop = isLoop;

        audioSource.Play();
        if (!isLoop) Destroy(sfxObj, sfx.length);
    }

    public void PlayBGM(AudioClip sfx)
    {
        bgm.clip = sfx;
        bgm.loop = true;
        bgm.volume = 0.25f;
        bgm.Play();
    }
}
