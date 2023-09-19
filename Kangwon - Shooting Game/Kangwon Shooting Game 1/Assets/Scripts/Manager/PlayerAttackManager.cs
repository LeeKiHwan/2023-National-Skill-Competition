using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerAttackManager : MonoBehaviour
{
    public static PlayerAttackManager Instance;

    [Header("Player")]
    public Player player;
    public int maxXp;
    public int xp;
    public float maxXpUpValue;

    [Header("All Attack Skill")]
    public int allAttckDamage;
    public float allAttckCoolTime;
    public float allAttckCurTime;

    [Header("Invc Skill")]
    public float invcTime;
    public float invcCoolTime;
    public float invcCurTime;

    [Header("Heal Skill")]
    public int healValue;
    public float healCoolTime;
    public float healCurTime;

    [Header("Speed Up Skill")]
    public float speedUpValue;
    public float speedUpTime;
    public float speedUpCoolTime;
    public float speedUpCurTime;

    [Header("NormalAttack")]
    public GameObject normalAttackBullet;
    public float normalAttackCoolTime;
    public float normalAttackCurTime;

    [Header("Fire Attack")]
    public bool canFire;
    public GameObject fire;
    public float fireCoolTime;
    public float fireCurTime;

    [Header("Elec Attack")]
    public bool canElec;
    public GameObject elec;
    public int elecCount;
    public float elecCoolTime;
    public float elecCurTime;

    [Header("Laser Attack")]
    public bool canLaser;
    public GameObject laser;
    public float laserCoolTime;
    public float laserCurTime;

    [Header("Ice Attack")]
    public bool canIce;
    public GameObject ice;
    public float iceCoolTime;
    public float iceCurTime;

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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Test");
        }

        if (player == null && FindObjectOfType<Player>())
        {
            player = FindObjectOfType<Player>();
        }
        
        if (player)
        {
            NormalAttack();
            UseSkill();
            FireAttack();
            ElecAttack();
            LaserAttack();
            IceAttack();
        }
    }

    public void GetXp(int getXp)
    {
        if (xp + getXp < maxXp) xp += getXp;
        else
        {
            maxXp = (int)(maxXp * maxXpUpValue);
            xp = 0;
            UIManager.Instance.LevelUpUI();
        }
    }

    public void UseSkill()
    {
        if (allAttckCurTime > 0) allAttckCurTime -= Time.deltaTime;
        if (invcCurTime > 0) invcCurTime -= Time.deltaTime;
        if (speedUpCurTime > 0) speedUpCurTime -= Time.deltaTime;
        if (healCurTime > 0) healCurTime -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Q) && allAttckCurTime <= 0 && player.mp >= 50)
        {
            Unit[] enemys = FindObjectsOfType<BaseEnemy>();
            foreach(BaseEnemy enemy in enemys)
            {
                if (enemy != null)
                {
                    enemy.GetComponent<Unit>().TakeDamage(allAttckDamage);
                }
            }
            player.mp -= 50;
            allAttckCurTime = allAttckCoolTime;
        }
        if (Input.GetKeyDown(KeyCode.E) && invcCurTime <= 0 && player.mp >= 30)
        {
            player.SetInvcTime(invcTime);
            player.mp -= 30;
            invcCurTime = invcCoolTime;
        }
        if (Input.GetKeyDown(KeyCode.R) && speedUpCurTime <= 0 && player.mp >= 20)
        {
            player.SpeedUp(speedUpValue, speedUpTime);
            player.mp -= 20;
            speedUpCurTime = speedUpCoolTime;
        }
        if (Input.GetKeyDown(KeyCode.F) && healCurTime <= 0 && player.mp >= 20)
        {
            player.TakeHeal(healValue);
            player.mp -= 20;
            healCurTime = healCoolTime;
        }
    }

    public void NormalAttack()
    {
        if (normalAttackCurTime > 0) normalAttackCurTime -= Time.deltaTime;

        if (normalAttackCurTime <= 0)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 dir = mousePos - (Vector2)player.transform.position;

            GameObject bullet = Instantiate(normalAttackBullet, player.transform.position, Quaternion.identity);
            bullet.transform.up = dir.normalized;

            normalAttackCurTime = normalAttackCoolTime;
        }
    }

    public void FireAttack()
    {
        if (fireCurTime > 0) fireCurTime -= Time.deltaTime;

        if (fireCurTime <= 0 && canFire)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 dir = mousePos - (Vector2)player.transform.position;

            GameObject bullet = Instantiate(fire, player.transform.position, Quaternion.identity);
            bullet.transform.up = dir.normalized;

            fireCurTime = fireCoolTime;
        }
    }

    public void ElecAttack()
    {
        if (elecCurTime > 0) elecCurTime -= Time.deltaTime;

        if (elecCurTime <= 0 && canElec)
        {
            for (int i = 0; i < elecCount; i++)
            {
                Vector2 target = new Vector2(Random.Range(-5, 5), Random.Range(-5, 5));
                Vector2 dir = target - (Vector2)transform.position;

                GameObject bullet = Instantiate(elec, player.transform.position, Quaternion.identity);
                bullet.transform.up = dir.normalized;
            }
            elecCurTime = elecCoolTime;
        }
    }

    public void LaserAttack()
    {
        if (laserCurTime > 0) laserCurTime -= Time.deltaTime;

        if (laserCurTime <= 0 && canLaser)
        {
            int xDir = Random.Range(-1f, 1f) > 0 ? 1 : -1;
            int yDir = Random.Range(-1f, 1f) > 0 ? 1 : -1;

            float x = xDir * 20;
            float y = yDir * 10;

            Vector2 target = player.transform.position;
            Vector2 dir = target - (new Vector2(x, y) + (Vector2)player.transform.position);

            GameObject bullet = Instantiate(laser, (new Vector2(x, y) + (Vector2)player.transform.position), Quaternion.identity);
            bullet.transform.up = dir.normalized;
            laserCurTime = laserCoolTime;
        }
    }

    public void IceAttack()
    {
        if (iceCurTime > 0) iceCurTime -= Time.deltaTime;

        if (iceCurTime <= 0 && canIce)
        {
            Collider2D[] enemys = Physics2D.OverlapBoxAll(player.transform.position, new Vector2(7, 7), 0);
            foreach(Collider2D enemy in enemys)
            {
                if (enemy.CompareTag("Enemy"))
                {
                    enemy.GetComponent<BaseEnemy>().SlowSpeed(2, 3);
                }
            }

            StartCoroutine(IceAttackCo(Instantiate(ice, player.transform.position, Quaternion.identity).GetComponent<SpriteRenderer>()));
            iceCurTime = iceCoolTime;
        }
    }

    public IEnumerator IceAttackCo(SpriteRenderer sr)
    {
        while (sr.color.a > 0)
        {
            sr.color = new Color(1, 1, 1, sr.color.a - 0.1f);
            yield return new WaitForSeconds(0.1f);
        }

        yield break;
    }
}
