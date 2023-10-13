using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Stage1Start : MonoBehaviour
{
    public AudioClip bgm;

    private void Awake()
    {
        StartCoroutine(Co());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene("Stage1");
        }
    }

    private void Start()
    {
        SoundManager.Instance.PlayBGM(bgm);
    }

    public IEnumerator Co()
    {
        yield return new WaitForSeconds(14);
        SceneManager.LoadScene("Stage1");
        yield break;
    }
}
