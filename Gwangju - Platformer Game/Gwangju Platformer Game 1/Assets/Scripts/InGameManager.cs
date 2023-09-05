using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameManager : MonoBehaviour
{
    public static InGameManager Instance;
    public GameObject map;
    public Vector3 targetRotation;
    public float rotateTime;

    [Header("Player")]
    public static int life;
    public static int score;

    private void Awake()
    {
        Instance = this;
        map = GameObject.Find("Map");
    }

    private void Update()
    {
        RotateMap();
    }

    void RotateMap()
    {
        map.transform.rotation = Quaternion.Lerp(map.transform.rotation, Quaternion.Euler(targetRotation), rotateTime);
        rotateTime += Time.deltaTime / 5;
    }

    public void SetTargetRotation(Vector3 rotateValue)
    {
        rotateTime = 0;
        targetRotation = rotateValue;
    }

    public void AddScore(int score)
    {
        InGameManager.score += score;
    }

    public void Die()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
