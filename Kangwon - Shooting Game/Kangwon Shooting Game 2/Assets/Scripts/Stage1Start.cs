using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Stage1Start : MonoBehaviour
{
    private void Awake()
    {
        StartCoroutine(Co());
    }

    public IEnumerator Co()
    {
        yield return new WaitForSeconds(14);
        SceneManager.LoadScene("Stage1");
        yield break;
    }
}
