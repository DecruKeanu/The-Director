using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingBehaviour : MonoBehaviour
{
    public enum Weapon
    {
        assaultGun,
        SMG,
        Shotgun,
        none
    }
    [SerializeField] public Weapon m_WhichWeapon;
    [SerializeField] private GameObject m_AssaultGunTemplate = null;
    [SerializeField] private GameObject m_ShotGunTemplate = null;
    [SerializeField] private GameObject m_SubMachineGunTemplate = null;
    [SerializeField] private GameObject m_PrimarySocket = null;
    private ShotGun m_ShotGun = null;
    private AssaultGun m_AssaultGun = null;
    private SubmachineGun m_SubmachineGun = null;
    public bool m_IsAssaultGun;

    public ShotGun getShotgun
    {
        get
        {
            if (m_ShotGun)
                return m_ShotGun;

            return null;
        }
    }
    public AssaultGun getAssaultGun
    {
        get
        {
            if (m_AssaultGun)
                return m_AssaultGun;

            return null;
        }
    }
    public SubmachineGun getSMG
    {
        get
        {
            if (m_SubmachineGun)
                return m_SubmachineGun;

            return null;
        }
    }

    //return the ammo in our currentClip depending on our weaponType
    public int PrimaryWeaponAmmo
    {
        get
        {
            if (m_WhichWeapon == Weapon.assaultGun)
            {
                if (m_AssaultGun)
                {
                    return m_AssaultGun.CurrentAmmo;
                }
            }
            else if (m_WhichWeapon == Weapon.SMG)
            {
                if (m_SubmachineGun)
                {
                    return m_SubmachineGun.CurrentAmmo;
                }
            }
            else if (m_WhichWeapon == Weapon.Shotgun)
            {
                if (m_ShotGun)
                {
                    return m_ShotGun.CurrentAmmo;
                }
            }

            return 0;
        }
    }
    //return the ammo in totalAmmo depending on our weaponType
    public int totalAmmo
    {
        get
        {
            if (m_WhichWeapon == Weapon.assaultGun)
            {
                if (m_AssaultGun)
                {
                    return m_AssaultGun.TotalAmmo;
                }
            }
            else if (m_WhichWeapon == Weapon.SMG)
            {
                if (m_SubmachineGun)
                {
                    return m_SubmachineGun.TotalAmmo;
                }
            }
            else if (m_WhichWeapon == Weapon.Shotgun)
            {
                if (m_ShotGun)
                {
                    return m_ShotGun.TotalAmmo;
                }
            }
         
             return 0;
        }
    }

    private void Awake()
    {
        SpawnWeapon();
    }

    public void SpawnWeapon()
    {
        //if our parameters exist spawn a weapon on our socket depending on the type of the weapon
        if (m_AssaultGunTemplate  && m_ShotGunTemplate && m_SubMachineGunTemplate && m_PrimarySocket)
        {
                if (m_WhichWeapon == Weapon.Shotgun)
                {
                    var gunObject = Instantiate(m_ShotGunTemplate, m_PrimarySocket.transform, true);
                    gunObject.transform.localPosition = Vector3.zero;
                    gunObject.transform.localRotation = Quaternion.identity;
                    m_ShotGun = gunObject.GetComponent<ShotGun>();
                }
                if (m_WhichWeapon == Weapon.assaultGun)
                {
                    var gunObject = Instantiate(m_AssaultGunTemplate, m_PrimarySocket.transform, true);
                    gunObject.transform.localPosition = Vector3.zero;
                    gunObject.transform.localRotation = Quaternion.identity;
                    m_AssaultGun = gunObject.GetComponent<AssaultGun>();
                    m_IsAssaultGun = true;
                }
                if (m_WhichWeapon == Weapon.SMG)
                {
                    var gunObject = Instantiate(m_SubMachineGunTemplate, m_PrimarySocket.transform, true);
                    gunObject.transform.localPosition = Vector3.zero;
                    gunObject.transform.localRotation = Quaternion.identity;
                    m_SubmachineGun = gunObject.GetComponent<SubmachineGun>();
                }
        }
    }
    public void PrimaryFire()
    {
        if (m_WhichWeapon == Weapon.Shotgun)
        {
            if (m_ShotGun)
            {
                m_ShotGun.PrimaryFire();
            }
        }
        else if (m_WhichWeapon == Weapon.assaultGun)
        {
            if (m_AssaultGun)
            {
                m_AssaultGun.PrimaryFire();
            }
        }
        else if (m_WhichWeapon == Weapon.SMG)
        {
            if (m_SubmachineGun)
            {
                m_SubmachineGun.PrimaryFire();
            }
        }
    }
    public void SecondaryFire()
    {
        if (m_WhichWeapon == Weapon.Shotgun)
        {
            if (m_ShotGun)
            {
                m_ShotGun.SecondaryFire();
            }
        }
        else if (m_WhichWeapon == Weapon.assaultGun)
        {
            if (m_AssaultGun)
            {
                m_AssaultGun.SecondaryFire();
            }
        }
        else if (m_WhichWeapon == Weapon.SMG)
        {
            if (m_SubmachineGun)
            {
                m_SubmachineGun.SecondaryFire();
            }
        }
    }

    public void Reload()
    {
        if (m_WhichWeapon == Weapon.Shotgun)
        {
            if (m_ShotGun)
            {
                m_ShotGun.Reload();
            }
        }
        else if (m_WhichWeapon == Weapon.assaultGun)
        {
            if (m_AssaultGun)
            {
                m_AssaultGun.Reload();
            }
        }
        else if (m_WhichWeapon == Weapon.SMG)
        {
            if (m_SubmachineGun)
            {
                m_SubmachineGun.Reload();
            }
        }
    }
}
