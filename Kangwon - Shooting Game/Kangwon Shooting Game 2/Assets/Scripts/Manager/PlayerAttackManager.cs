using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    Shotgun,
    Sniper,
    RPG
}

public class PlayerAttackManager : MonoBehaviour
{
    public static PlayerAttackManager Instance;

    [Header("Player")]
    public Player player;
    public int xp;
    public int maxXp;

    [Header("all Atk")]
    public GameObject allAtkEffect;
    public int allAtkDamage;
    public float allAtkCool;
    public float allAtkCur;

    [Header("invc")]
    public float invcTime;
    public float invcCool;
    public float invcCur;

    [Header("speed")]
    public float speedValue;
    public float speedTime;
    public float speedCool;
    public float speedCur;

    [Header("heal")]
    public int healValue;
    public float healCool;
    public float healCur;

    [Header("basic bullet")]
    public GameObject flash;
    public GameObject basicBullet;
    public int basicBulletDamage;
    public float basicBulletSpeed;
    public float basicBulletSpread;
    public float basicBulletCool;
    public float basicBulletCur;

    [Header("Shotgun")]
    public bool onShotgun;
    public GameObject shotgunBullet;
    public int shotgunDamage;
    public float shotgunSpeed;

    [Header("Sniper")]
    public bool onSniper;
    public GameObject sniperBullet;
    public int sniperDamage;
    public float sniperSpeed;

    [Header("RPG")]
    public bool onRPG;
    public GameObject RPGBullet;
    public int RPGDamage;
    public float RPGSpeed;

    [Header("Sound")]
    public AudioClip attackSFX;
    public AudioClip shotgunSFX;
    public AudioClip sniperSFX;
    public AudioClip rpgSFX;

    private void Awake()
    {
        Instance = this;

        if (FindObjectOfType<Player>()) player = FindObjectOfType<Player>(); 
    }

    private void Update()
    {
        if (FindObjectOfType<Player>() && !player) player = FindObjectOfType<Player>();

        UseSkill();
        Attack();
        UseWeapon();
    }

    public void UseSkill()
    {
        if (allAtkCur > 0) allAtkCur -= Time.deltaTime;
        if (invcCur > 0) invcCur -= Time.deltaTime;
        if (speedCur > 0) speedCur -= Time.deltaTime;
        if (healCur > 0) healCur -= Time.deltaTime;

        if (allAtkCur <= 0 && Input.GetKeyDown(KeyCode.Alpha1) && player.mp > 50)
        {
            Instantiate(allAtkEffect, new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, 0), Quaternion.identity);

            Collider2D[] enemys = Physics2D.OverlapBoxAll(player.transform.position, new Vector2(20, 12), 0);

            foreach (Collider2D enemy in enemys)
            {
                if (enemy.GetComponent<BaseEnemy>())
                {
                    enemy.GetComponent<BaseEnemy>().TakeDamage(allAtkDamage);
                }
                if (enemy.GetComponent<Bullet>() && enemy.GetComponent<Bullet>().bulletType == BulletType.Enemy)
                {
                    Destroy(enemy.gameObject);
                }
            }
            player.CameraShake();
            player.mp -= 50;
            allAtkCur = allAtkCool;
        }
        if (invcCur <= 0 && Input.GetKeyDown(KeyCode.Alpha2) && player.mp > 30)
        {
            player.SetInvcTime(invcTime);
            player.mp -= 30;
            invcCur = invcCool;
        }
        if (speedCur <= 0 && Input.GetKeyDown(KeyCode.Alpha3) && player.mp > 20)
        {
            player.SpeedUp(speedValue, speedTime);
            player.mp -= 20;
            speedCur = speedCool;
        }
        if (healCur <= 0 && Input.GetKeyDown(KeyCode.Alpha4) && player.mp > 20)
        {
            player.TakeHeal(healValue);
            player.mp -= 20;
            healCur = healCool;
        }
    }

    public void Attack()
    {
        if (basicBulletCur > 0) basicBulletCur -= Time.deltaTime;

        if (basicBulletCur <= 0 && Input.GetMouseButton(0))
        {
            SoundManager.Instance.PlaySFX(attackSFX);

            Vector2 target = Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(Random.Range(-basicBulletSpread, basicBulletSpread), Random.Range(-basicBulletSpread, basicBulletSpread));
            Vector2 dir = target - (Vector2)player.muzzle.transform.position;

            GameObject b = Instantiate(basicBullet, player.muzzle.position, Quaternion.identity);
            b.transform.up = dir.normalized;
            b.GetComponent<Bullet>().SetBulletStatus(basicBulletDamage, basicBulletSpeed);
            basicBulletCur = basicBulletCool;

            Instantiate(flash, player.muzzle.position, Quaternion.identity);
        }
    }

    public void GetXp(int getXp)
    {
        if (xp + getXp < maxXp) xp += getXp;
        else
        {
            GetItem();
            xp = 0;
        }
    }

    public void GetItem()
    {
        int i = Random.Range(0, 5);

        switch(i)
        {
            case 0:
                player.SpeedUp(2, 2);
                break;
            case 1:
                player.SetInvcTime(2);
                break;
            case 2:
                player.TakeHeal(15);
                player.GetMp(15);
                break;
            case 3:
                GetXp(15);
                break;
        }
    }

    public void GetWeapon(WeaponType weaponType)
    {
        switch (weaponType)
        {
            case WeaponType.Shotgun:
                onShotgun = true;
                onSniper = false;
                onRPG = false;
                break;
            case WeaponType.Sniper:
                onSniper = true;
                onShotgun = false;
                onRPG = false;
                break;
            case WeaponType.RPG:
                onRPG = true;
                onShotgun = false;
                onSniper = false;
                break;
        }
    }

    public void UseWeapon()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (onShotgun)
            {
                SoundManager.Instance.PlaySFX(shotgunSFX);
                for (int i=0; i<20; i++)
                {
                    Vector2 target = Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f));
                    Vector2 dir = target - (Vector2)player.muzzle.transform.position;

                    GameObject b = Instantiate(shotgunBullet, player.muzzle.position, Quaternion.identity);
                    b.transform.up = dir.normalized;
                    b.GetComponent<Bullet>().SetBulletStatus(shotgunDamage, shotgunSpeed);
                    Instantiate(flash, player.muzzle.position, Quaternion.identity);
                }
            }
            if (onSniper)
            {
                SoundManager.Instance.PlaySFX(sniperSFX);
                Vector2 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 dir = target - (Vector2)player.muzzle.transform.position;

                GameObject b = Instantiate(sniperBullet, player.muzzle.position, Quaternion.identity);
                b.transform.up = dir.normalized;
                b.GetComponent<Bullet>().SetBulletStatus(sniperDamage, sniperSpeed, true);
                Instantiate(flash, player.muzzle.position, Quaternion.identity);
            }
            if (onRPG)
            {
                SoundManager.Instance.PlaySFX(rpgSFX);
                Vector2 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 dir = target - (Vector2)player.muzzle.transform.position;

                GameObject b = Instantiate(RPGBullet, player.muzzle.position, Quaternion.identity);
                b.transform.up = dir.normalized;
                b.GetComponent<Bullet>().SetBulletStatus(RPGDamage, RPGSpeed);
                Instantiate(flash, player.muzzle.position, Quaternion.identity);
            }

            onShotgun = false;
            onSniper = false;
            onRPG = false;
        }
    }
}
