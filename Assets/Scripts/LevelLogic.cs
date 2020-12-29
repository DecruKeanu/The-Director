using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelLogic : MonoBehaviour
{
    [SerializeField] protected Rigidbody m_Player = null;
    [SerializeField] private Collider m_TriggerBox1 = null;
    [SerializeField] private Collider m_TriggerBox2 = null;
    [SerializeField] private Collider m_TriggerBox3 = null;
    [SerializeField] private Collider m_TriggerBox4 = null;
    [SerializeField] private RectTransform m_Text1 = null;
    [SerializeField] private RectTransform m_Text2 = null;
    [SerializeField] private RectTransform m_Text3 = null;
    [SerializeField] private RectTransform m_Text4 = null;
    [SerializeField] private RectTransform m_Text5 = null;
    [SerializeField] private RectTransform m_Text6 = null;
    [SerializeField] private AudioSource m_ExplosionSound = null;

    private ShootingBehaviour m_PlayerShootingBehaviour;
    private MovementBehaviour m_PlayerMovementBehaviour;
    private PlayerCharacter m_PlayerBehaviour;
    private bool m_DeleteOnce = false;
    public bool m_GameStarted = false;
    // Start is called before the first frame update
    void Start()
    {
        //find objects we need
        m_PlayerShootingBehaviour = FindObjectOfType<ShootingBehaviour>();
        m_PlayerMovementBehaviour = FindObjectOfType<MovementBehaviour>();
        m_PlayerBehaviour = FindObjectOfType<PlayerCharacter>();
        //set time to 1 when the game starts 
        Time.timeScale = 1;
    }

    // Update is called once per frame
    private void Update()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
    }
    private void OnTriggerStay(Collider other)
    {
        //only check colliders we are interested in (more performant)
        if (other.tag != "VaultTrigger" && other.tag != "MoneyTrigger" && other.tag != "WeaponTriggerSMG" && other.tag != "WeaponTriggerShotGun" && other.tag != "WeaponTriggerAssault" && other.tag != "StartGame")
        {
            return;
        }
        else
        {
            //delete toturial assets when player starts game
           if (other.tag == "StartGame")
            {
                if (Input.GetAxis("Interact") > 0.0f)
                {
                    if (m_DeleteOnce == false)
                    {
                        if (m_TriggerBox1)
                            Destroy(m_TriggerBox1.gameObject);
                        if (m_TriggerBox2)
                            Destroy(m_TriggerBox2.gameObject);
                        if (m_TriggerBox3)
                            Destroy(m_TriggerBox3.gameObject);
                        if (m_TriggerBox4)
                            Destroy(m_TriggerBox4.gameObject);
                        if (m_Text1)
                            Destroy(m_Text1.gameObject);
                        if (m_Text2)
                            Destroy(m_Text2.gameObject);
                        if (m_Text3)
                            Destroy(m_Text3.gameObject);
                        if (m_Text4)
                            Destroy(m_Text4.gameObject);
                        if (m_Text5)
                            Destroy(m_Text5.gameObject);
                        if (m_Text6)
                            Destroy(m_Text6.gameObject);

                        m_GameStarted = false;
                        m_DeleteOnce = true;
                    }
                }
            }

            if (!m_PlayerBehaviour || !m_PlayerMovementBehaviour || !m_PlayerShootingBehaviour)
                return;
            if (other.tag == "WeaponTriggerSMG")
            {
                m_PlayerBehaviour.m_IsSettingWeapon = true;
                Vector3 pos = m_Player.transform.position + Vector3.forward;
                m_PlayerMovementBehaviour.DesiredLookatPoint = pos;

                if (Input.GetAxis("Interact") > 0.0f)
                {
                    if (m_PlayerShootingBehaviour.m_WhichWeapon != ShootingBehaviour.Weapon.none)
                    {
                        if (m_PlayerShootingBehaviour.m_WhichWeapon == ShootingBehaviour.Weapon.Shotgun)
                        {
                            Destroy(m_PlayerShootingBehaviour.getShotgun.gameObject);
                        }
                        if (m_PlayerShootingBehaviour.m_WhichWeapon == ShootingBehaviour.Weapon.assaultGun)
                        {
                            Destroy(m_PlayerShootingBehaviour.getAssaultGun.gameObject);
                        }
                    }
                    if (m_PlayerShootingBehaviour.m_WhichWeapon != ShootingBehaviour.Weapon.SMG)
                    {
                        m_PlayerShootingBehaviour.m_WhichWeapon = ShootingBehaviour.Weapon.SMG;
                        m_PlayerShootingBehaviour.SpawnWeapon();
                    }
                }
            }

            if (other.tag == "WeaponTriggerShotGun")
            {
                m_PlayerBehaviour.m_IsSettingWeapon = true;
                Vector3 pos = m_Player.transform.position + Vector3.forward;
                m_PlayerMovementBehaviour.DesiredLookatPoint = pos;

                if (Input.GetAxis("Interact") > 0.0f)
                {
                    if (m_PlayerShootingBehaviour.m_WhichWeapon != ShootingBehaviour.Weapon.none)
                    {
                        if (m_PlayerShootingBehaviour.m_WhichWeapon == ShootingBehaviour.Weapon.assaultGun)
                        {
                            Destroy(m_PlayerShootingBehaviour.getAssaultGun.gameObject);
                        }
                        if (m_PlayerShootingBehaviour.m_WhichWeapon == ShootingBehaviour.Weapon.SMG)
                        {
                            Destroy(m_PlayerShootingBehaviour.getSMG.gameObject);
                        }
                    }
                    if (m_PlayerShootingBehaviour.m_WhichWeapon != ShootingBehaviour.Weapon.Shotgun)
                    {
                        m_PlayerShootingBehaviour.m_WhichWeapon = ShootingBehaviour.Weapon.Shotgun;
                        m_PlayerShootingBehaviour.SpawnWeapon();
                    }
                }
            }

            if (other.tag == "WeaponTriggerAssault")
            {
                m_PlayerBehaviour.m_IsSettingWeapon = true;
                Vector3 pos = m_Player.transform.position + Vector3.forward;
                m_PlayerMovementBehaviour.DesiredLookatPoint = pos;

                 if (Input.GetAxis("Interact") > 0.0f)
                 {
                     if (m_PlayerShootingBehaviour.m_WhichWeapon != ShootingBehaviour.Weapon.none)
                     {
                         if (m_PlayerShootingBehaviour.m_WhichWeapon == ShootingBehaviour.Weapon.Shotgun)
                         {
                             Destroy(m_PlayerShootingBehaviour.getShotgun.gameObject);
                         }
                         if (m_PlayerShootingBehaviour.m_WhichWeapon == ShootingBehaviour.Weapon.SMG)
                         {
                             Destroy(m_PlayerShootingBehaviour.getSMG.gameObject);
                         }
                     }
                     if (m_PlayerShootingBehaviour.m_WhichWeapon != ShootingBehaviour.Weapon.assaultGun)
                     {
                         m_PlayerShootingBehaviour.m_WhichWeapon = ShootingBehaviour.Weapon.assaultGun;
                         m_PlayerShootingBehaviour.SpawnWeapon();
                     }
                 }
            }
            //if collider is not the vault or money trigger return
            if (other.tag != "VaultTrigger" && other.tag != "MoneyTrigger" )
            {
                return;
            }

        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        //only check colliders we are interested in (more performant)
        if (other.tag != "MoneyTrigger" && other.tag != "WeaponTriggerSMG" && other.tag != "WeaponTriggerShotGun" && other.tag != "WeaponTriggerAssault")
        {
            return;
        }

        //set the player rotation back to the mousePos when the player exist and leaves one of the weaponTriggers
        if (m_PlayerBehaviour && other.tag == "WeaponTriggerSMG" || other.tag == "WeaponTriggerShotGun" || other.tag == "WeaponTriggerAssault")
        {
            m_PlayerBehaviour.m_IsSettingWeapon = false;
        }
    }
}
