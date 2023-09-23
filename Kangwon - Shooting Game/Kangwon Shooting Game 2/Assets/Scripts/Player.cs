using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public struct PlayerTextUI
{
    public string text;
    public Color color;

    public PlayerTextUI(string text, Color color)
    {
        this.text = text;
        this.color = color;
    }
}

public class Player : Unit
{
    [Header("Status")]
    Rigidbody2D rb;
    public int maxHp;
    public int mp;
    public int maxMp;

    [Header("Ability")]
    public float invcTime;
    public GameObject invcShield;
    public GameObject speedEffect;
    public Transform playerCamera;
    public GameObject hpLessScreen;
    public GameObject InvcScreen;

    [Header("Stage1")]
    public bool isClear;
    public Transform sight;
    public SpriteRenderer body;
    public Transform gun;
    public Transform muzzle;

    [Header("Text")]
    public Queue<PlayerTextUI> textQueue = new Queue<PlayerTextUI>();
    public RectTransform textParent;
    public GameObject textObj;



    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        StartCoroutine(GetMpCo());
        Cursor.visible = false;
        InstantiateTextUI();
    }

    private void Update()
    {
        if (invcTime > 0)
        {
            invcTime -= Time.deltaTime;
            invcShield.SetActive(true);
        }
        else
        {
            invcShield.SetActive(false);
        }

        if (invcTime > 0)
        {
            InvcScreen.SetActive(true);
        }
        else if (hp < 30)
        {
            hpLessScreen.SetActive(true);
        }
        else
        {
            InvcScreen.SetActive(false);
            hpLessScreen.SetActive(false);
        }

        Attack();
        Move();
    }

    public override void Attack()
    {
        Vector2 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        sight.transform.position = target;

        if (Input.GetMouseButton(0) && (InGameManager.Instance.curStage == 0 || InGameManager.Instance.curStage == 2))
        {
            body.flipX = target.x - transform.position.x > 0 ? false : true;
            gun.rotation = target.x - transform.position.x > 0 ? Quaternion.identity : Quaternion.Euler(new Vector2(0, 180));
        }
    }

    public override void Die()
    {
    }

    public override void Move()
    {
        float x = Input.GetAxisRaw("Horizontal") * speed;
        float y = Input.GetAxisRaw("Vertical") * speed;
        
        rb.velocity = new Vector2(x, y);
    }

    public override void TakeDamage(int damage)
    {
        if (invcTime <= 0)
        {
            base.TakeDamage(damage);

            if (mp - damage > 0) mp -= damage;
            else mp = 0;

            SetInvcTime(0.5f);
        }
    }

    public void TakeHeal(int heal)
    {
        if (hp + heal < maxHp) hp += heal;
        else hp = maxHp;
        OnTextUI("+ " + heal + "HP", Color.green);
    }

    public void SetInvcTime(float time)
    {
        if (time > invcTime)
        {
            invcTime = time;
            OnTextUI("Shield!", Color.yellow);
        }
    }

    public void SpeedUp(float speedValue, float time)
    {
        StartCoroutine(SpeedUpCo(speedValue, time));
        OnTextUI("Speed Up!", Color.yellow);
    }

    public IEnumerator SpeedUpCo(float speedValue, float time)
    {
        speed += speedValue;
        speedEffect.SetActive(true);
        yield return new WaitForSeconds(time);
        speed -= speedValue;
        speedEffect.SetActive(false);

        yield break;
    }

    public void GetMp(int getMp)
    {
        if (mp + getMp > maxMp) mp += getMp;
        else mp = maxMp;
        OnTextUI("+ " + getMp + "MP", Color.cyan);
    }

    public IEnumerator GetMpCo()
    {
        while (true)
        {
            if (mp + 5 < maxMp) mp += 5;
            else mp = maxMp;
            yield return new WaitForSeconds(1);
        }
    }

    public void OnTextUI(string text, Color color)
    {
        PlayerTextUI p = new PlayerTextUI(text, color);
        textQueue.Enqueue(p);
    }

    public void InstantiateTextUI()
    {
        if (textQueue.Count > 0)
        {
            GameObject o = Instantiate(textObj, textParent);

            TextMeshProUGUI t = o.GetComponent<TextMeshProUGUI>();
            PlayerTextUI p = textQueue.Peek();
            textQueue.Dequeue();
            t.text = p.text;
            t.color = p.color;
            Destroy(o, 1);
        }
        Invoke("InstantiateTextUI", 0.25f);
    }

    public void CameraShake()
    {
        StartCoroutine(CameraShakeCo());    
    }

    public IEnumerator CameraShakeCo()
    {
        for (int i = 0; i < 10; i++)
        {
            playerCamera.localPosition = Vector3.zero + new Vector3(Random.Range(-0.25f, 0.25f), Random.Range(-0.25f, 0.25f), -10);

            yield return new WaitForSeconds(0.025f);
        }

        playerCamera.localPosition = new Vector3(0,0,-10);

        yield break;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("NextStage") && !isClear)
        {
            InGameManager.Instance.StageClear();
            isClear = true;
        }
    }
}
