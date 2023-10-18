using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using System;

public class MenuManager : MonoBehaviour
{
    public Image selectedWeaponImg;
    public int curWeapon;
    public Sprite[] weaponImgs;
    public GameObject[] weaponStats;

    public TextMeshProUGUI[] names;
    public TextMeshProUGUI[] scores;

    public AudioClip bgm;
    public AudioClip gunSFX;
    public AudioClip uiInteractSFX;

    private void Awake()
    {
    }

    private void Start()
    {
        SoundManager.Instance.PlayBGM(bgm);
    }

    public void GameStart()
    {
        InGameManager.score = 0;
        Player.selectedGun = weaponImgs[curWeapon];
        SceneManager.LoadScene("Stage1Start");
    }

    //public void OnWeaponSelectUI()
    //{
    //    curWeapon = 0;
    //    selectedWeaponImg.sprite = weaponImgs[0];
        
    //    foreach(GameObject weapon in weaponStats)
    //    {
    //        weapon.SetActive(false);
    //    }
    //    weaponStats[0].SetActive(true);
    //}

    public void NextWeapon()
    {
        if (curWeapon < weaponImgs.Length-1)
        {
            SoundManager.Instance.PlaySFX(gunSFX);

            weaponStats[curWeapon].SetActive(false);

            curWeapon++;
            selectedWeaponImg.sprite = weaponImgs[curWeapon];
            weaponStats[curWeapon].SetActive(true);
        }
    }

    public void PrevWeapon()
    {
        if (curWeapon > 0)
        {
            SoundManager.Instance.PlaySFX(gunSFX);

            weaponStats[curWeapon].SetActive(false);

            curWeapon--;
            selectedWeaponImg.sprite = weaponImgs[curWeapon];
            weaponStats[curWeapon].SetActive(true);
        }
    }

    public void ShowRanking()
    {
        for (int i = 0; i < names.Length; i++)
        {
            names[i].text = "";
            scores[i].text = "";
        }

        for (int i = 0; i < RankingManager.Instance.ranking.Count; i++)
        {
            names[i].text = RankingManager.Instance.ranking[i].Name;
            scores[i].text = RankingManager.Instance.ranking[i].Score + " P";
        }
    }

    public void UIInteract()
    {
        SoundManager.Instance.PlaySFX(uiInteractSFX);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
