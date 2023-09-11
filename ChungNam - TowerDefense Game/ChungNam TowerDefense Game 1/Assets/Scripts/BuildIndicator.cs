using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BuildIndicator : MonoBehaviour
{
    public TowerManager towerManager;
    public LayerMask unableBuildLayer;
    public MeshRenderer mr;
    public Material ableBuildMat;
    public Material unableBuildMat;

    private void Update()
    {
        if (towerManager.canBuild) mr.material = ableBuildMat;
        else mr.material = unableBuildMat;

        transform.localScale = new Vector3(towerManager.selectTower.GetComponent<BaseTower>().size.x, 0.1f, towerManager.selectTower.GetComponent<BaseTower>().size.y) * 2;
    }

    private void OnTriggerStay(Collider other)
    {
        if ((1 << other.gameObject.layer & unableBuildLayer.value) == 1 << other.gameObject.layer)
        {
            towerManager.canBuild = false;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if ((1 << other.gameObject.layer & unableBuildLayer.value) == 1 << other.gameObject.layer)
        {
            towerManager.canBuild = true;
        }
    }
}
