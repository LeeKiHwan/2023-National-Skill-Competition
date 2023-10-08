using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour
{
    public static TowerManager Instance;

    public int gold;
    public GameObject buildIndicator;
    public Transform towerRangeIndicator;
    public GameObject selectedTower;
    public LayerMask placableLayer;
    public bool onBuild;
    public GameObject buildEffect;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        BuildTower();

        buildIndicator.transform.position = GetMousePos();
    }

    public void GetGold(int getGold)
    {
        gold += getGold;
    }

    public Vector3 GetMousePos()
    {
        Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane);
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        RaycastHit hit;
        Physics.Raycast(ray, out hit, 100, placableLayer);
        return hit.point;
    }

    public void SelectTower(GameObject tower)
    {
        if (gold < tower.GetComponent<BaseTower>().price) return;
        buildIndicator.SetActive(true);
        selectedTower = tower;
        onBuild = true;
    }

    public void BuildTower()
    {
        if (selectedTower)
        {
            float scale = selectedTower.GetComponent<BaseTower>().atkRange * 2;
            towerRangeIndicator.localScale = new Vector3(scale, 0.5f, scale);
        }

        if (onBuild && Input.GetMouseButtonDown(0))
        {
            gold -= selectedTower.GetComponent<BaseTower>().price;
            Instantiate(selectedTower, GetMousePos(), Quaternion.identity);
            Instantiate(buildEffect, GetMousePos(), Quaternion.identity);
            buildIndicator.SetActive(false);
            onBuild = false;
        }
    }

    public void BuyItem()
    {
        if (gold > 50)
        {
            int rand = Random.Range(0, 3);
            switch (rand)
            {
                case 0:
                    foreach(BaseTower tower in FindObjectsOfType<BaseTower>())
                    {
                        tower.TakeHeal(20);
                    }
                    UIManager.Instance.ItemUI("TOWER HEAL", new Color(0.5f,1,0));
                    break;
                case 1:
                    foreach(BaseEnemy enemy in FindObjectsOfType<BaseEnemy>())
                    {
                        enemy.TakeDamage(20, null);
                    }
                    UIManager.Instance.ItemUI("ENEMY DAMAGE", new Color(1, 0.25f, 0));
                    break;
                case 2:
                    gold += 200;
                    UIManager.Instance.ItemUI("GOLD +200", Color.yellow);
                    break;
            }
            gold -= 50;
        }
    }
}
