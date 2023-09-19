using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackUpgradeManager : MonoBehaviour
{
    public PlayerAttackManager pam;

    private void Start()
    {
        pam = PlayerAttackManager.Instance;
    }

    public void AllAttackUpgrade()
    {
        pam.allAttckDamage += 5;
    }

    public void InvcUpgrade()
    {
        pam.invcTime += 1;
    }

    public void HealUpgrade()
    {
        pam.healValue += 5;
    }

    public void SpeedUpUpgrade()
    {
        pam.speedUpTime += 0.5f;
        pam.speedUpValue += 0.5f;
    }

    public void NormalAttackUpgrade()
    {
        pam.normalAttackCoolTime *= 0.8f;
    }

    public void FireUpgrade()
    {
        if (!pam.canFire)
        {
            pam.fireCoolTime = 2;
            pam.canFire = true;
        }
        pam.fireCoolTime *= 0.8f;
    }

    public void ElecUpgrade()
    {
        if (!pam.canElec)
        {
            pam.elecCount = 1;
            pam.elecCoolTime = 5;
            pam.canElec = true;
        }
        pam.elecCount += 2;
    }

    public void LaserUpgrade()
    {
        if (!pam.canLaser)
        {
            pam.laserCoolTime = 5;
            pam.canLaser = true;
        }
        pam.laserCoolTime *= 0.8f;
    }

    public void IceUpgrade()
    {
        if (!pam.canIce)
        {
            pam.iceCoolTime = 5;
            pam.canIce = true;
        }
        pam.iceCoolTime *= 0.8f;
    }
}
