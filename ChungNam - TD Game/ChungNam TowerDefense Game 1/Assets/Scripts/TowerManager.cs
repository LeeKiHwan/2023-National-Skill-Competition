using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TowerManager : MonoBehaviour
{
    public static TowerManager Instance;

    public int gold;

    [Header("Buildable Tower")]
    public GameObject basicTower;
    public GameObject multiAttackTower;
    public GameObject focusAttackTower;
    public GameObject illusionTower;

    [Header("Buildable Data")]
    public bool isBuildOn;
    public bool canBuild;
    public Vector3 lastBuildPos;
    public LayerMask buildableLayer;
    public GameObject selectTower;
    public GameObject buildIndicator;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        BuildTower();
    }

    public Vector3 GetMousePosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;

        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100, buildableLayer)) lastBuildPos = hit.point;

        return lastBuildPos;
    }

    public void SelectBuildTower(GameObject tower)
    {
        if (tower.GetComponent<BaseTower>().price <= gold)
        {
            isBuildOn = true;
            canBuild = true;
            selectTower = tower;
        }
        else
        {
            Debug.Log("no gold");
        }
    }

    public void BuildTower()
    {
        if (isBuildOn) 
        {
            buildIndicator.SetActive(true);
            buildIndicator.transform.position = GetMousePosition();

            if (Input.GetMouseButtonDown(0) && canBuild && selectTower.GetComponent<BaseTower>().price <= gold)
            {
                Instantiate(selectTower, GetMousePosition(), Quaternion.identity);
                gold -= selectTower.GetComponent<BaseTower>().price;
                BuildEnd();
            }
        }
        else
        {
            buildIndicator.SetActive(false);
        }
    }

    public void BuildEnd()
    {
        isBuildOn = false;
    }

    public void GetGold(int getGold)
    {
        gold += getGold;
    }
}
