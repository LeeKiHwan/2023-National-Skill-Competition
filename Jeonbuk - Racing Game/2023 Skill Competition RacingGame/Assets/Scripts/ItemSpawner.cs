using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    enum SpawnDir
    {
        Horizontal,
        Vertical
    }

    [SerializeField] SpawnDir spawnDir;
    [SerializeField] float spawnCoolTime;
    [SerializeField] float spawnCurTime;

    private void Update()
    {
        SpawnItem();
    }

    void SpawnItem()
    {
        if (spawnCurTime <= 0)
        {
            float x = transform.position.x;
            float y = transform.position.y;

            foreach(RandomItem item in gameObject.GetComponentsInChildren<RandomItem>())
            {
                Destroy(item.gameObject);
            }

            if (spawnDir == SpawnDir.Horizontal)
            {
                Instantiate(ItemManager.Instance.RandomItem, gameObject.transform).GetComponent<Transform>().position = new Vector2(x-5,y);
                Instantiate(ItemManager.Instance.RandomItem, gameObject.transform).GetComponent<Transform>().position = new Vector2(x-2.5f,y);
                Instantiate(ItemManager.Instance.RandomItem, gameObject.transform).GetComponent<Transform>().position = new Vector2(x,y);
                Instantiate(ItemManager.Instance.RandomItem, gameObject.transform).GetComponent<Transform>().position = new Vector2(x+2.5f,y);
                Instantiate(ItemManager.Instance.RandomItem, gameObject.transform).GetComponent<Transform>().position = new Vector2(x+5,y);
            }
            else
            {
                Instantiate(ItemManager.Instance.RandomItem, gameObject.transform).GetComponent<Transform>().position = new Vector2(x,y-5);
                Instantiate(ItemManager.Instance.RandomItem, gameObject.transform).GetComponent<Transform>().position = new Vector2(x,y-2.5f);
                Instantiate(ItemManager.Instance.RandomItem, gameObject.transform).GetComponent<Transform>().position = new Vector2(x,y);
                Instantiate(ItemManager.Instance.RandomItem, gameObject.transform).GetComponent<Transform>().position = new Vector2(x,y+2.5f);
                Instantiate(ItemManager.Instance.RandomItem, gameObject.transform).GetComponent<Transform>().position = new Vector2(x,y+5);
            }
            spawnCurTime = spawnCoolTime;
        }
        else
        {
            spawnCurTime -= Time.deltaTime;
        }
    }
}
