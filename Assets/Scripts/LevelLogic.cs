using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelLogic : MonoBehaviour
{
    [SerializeField] protected Rigidbody m_Player = null;
    [SerializeField] private Collider m_Bomb1 = null;
    [SerializeField] private Collider m_TriggerArea1 = null;
    [SerializeField] private Collider m_Vault1 = null;
    [SerializeField] private Collider m_Bomb2 = null;
    [SerializeField] private Collider m_TriggerArea2 = null;
    [SerializeField] private Collider m_Vault2 = null;
    [SerializeField] private EnemyCharacter m_StartEnemy1 = null;
    [SerializeField] private EnemyCharacter m_StartEnemy2 = null;
    [SerializeField] private Collider m_TriggerBox1 = null;
    [SerializeField] private Collider m_TriggerBox2 = null;
    [SerializeField] private Collider m_TriggerBox3 = null;
    [SerializeField] private Collider m_TriggerBox4 = null;
    [SerializeField] private Collider m_BankDoor = null;
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
    private float m_ExplosionRadius = 20.0f;
    private float m_Second = 1.0f;
    private int m_Score = 0;
    private int m_MoneyPerSecond = 100;
    private bool m_IsBombDestroyed1 = false;
    private bool m_IsBombDestroyed2 = false;
    private bool m_DoOnce = false;
    private bool m_MoneyCollected = false;
    private bool m_CollectingMoney = false;
    private bool m_DeleteOnce = false;

    public bool m_GameWon = false;
    public float m_ExplosionTimer1 = 5.0f;
    public float m_ExplosionTimer2 = 20.0f;
    public bool m_IsBombPlaced1 = false;
    public bool m_IsBombPlaced2 = false;

    public int MoneyScore
    {
        get
        {
            return m_Score;
        }
    }
    public bool Bomb2Placed
    {
        get
        {
            return m_IsBombPlaced2;
        }
    }
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
        //Handles bomb 1 explosion and deletion when activated
        if (m_IsBombDestroyed1 == false)
        {
            if (m_IsBombPlaced1 == true)
            {
                m_ExplosionTimer1 -= Time.deltaTime;

                if (m_ExplosionTimer1 < 0.0f)
                {
                    if (m_DoOnce == false)
                    {
                        if (m_Bomb1)
                        {
                            m_ExplosionSound.Play();
                            m_Bomb1.transform.localScale = m_Bomb1.transform.localScale * m_ExplosionRadius;
                            m_Bomb1.isTrigger = true;
                            m_DoOnce = true;
                        }
                    }
                }
                if (m_ExplosionTimer1 < -1.0f)
                {
                    if (m_StartEnemy1)
                        Destroy(m_StartEnemy1.gameObject);
                    if (m_StartEnemy2)
                        Destroy(m_StartEnemy2.gameObject);
                    if (m_Bomb1)
                        Destroy(m_Bomb1.gameObject);
                    if (m_Vault1)
                        Destroy(m_Vault1.gameObject);
                    if (m_TriggerArea1)
                        Destroy(m_TriggerArea1.gameObject);

                    m_IsBombDestroyed1 = true;
                    m_DoOnce = false;
                }
            }
        }
        //Handles bomb 2 explosion and deletion when activated
        if (m_IsBombDestroyed2 == false)
        {
            if (m_IsBombPlaced2 == true)
            {
                m_ExplosionTimer2 -= Time.deltaTime;

                if (m_ExplosionTimer2 < 0.0f)
                {
                    if (m_DoOnce == false)
                    {
                        if (m_Bomb2)
                        {
                            m_ExplosionSound.Play();
                            m_Bomb2.transform.localScale = m_Bomb2.transform.localScale * m_ExplosionRadius;
                            m_Bomb2.isTrigger = true;
                            m_DoOnce = true;
                        }
                    }
                }
                if (m_ExplosionTimer2 < -1.0f)
                {
                    if (m_Bomb2)
                        Destroy(m_Bomb2.gameObject);
                    if (m_Vault2)
                        Destroy(m_Vault2.gameObject);
                    if (m_TriggerArea2)
                        Destroy(m_TriggerArea2.gameObject);

                    m_IsBombDestroyed2 = true;
                    m_DoOnce = false;
                }
            }
        }

        //when player collects money hes score increases by MoneyPerSecond for each second
        if (m_CollectingMoney == true)
        {
            m_Second -= Time.deltaTime;

            if (m_Second <= 0.0f)
            {
                m_Score += m_MoneyPerSecond;
                Debug.Log(m_Score);
                m_Second = 1.0f;
            }
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        //only check colliders we are interested in (more performant)
        if (other.tag != "LevelEnd")
        {
            return;
        }
        else
        {
            //if player has any money, he wins the game
            if (other.tag == "LevelEnd")
            {
                if (m_MoneyCollected == true)
                {
                    Debug.Log("Escaped");
                    m_GameWon = true;
                }
            }
        }
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
                        if (m_BankDoor)
                            Destroy(m_BankDoor.gameObject);

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
            //activates the bom
            if (m_Bomb1 && m_IsBombDestroyed1 == false)
            {
                if (Input.GetAxis("Interact") > 0.0f)
                {
                   if (m_IsBombPlaced1 == false)
                   {
                       m_IsBombPlaced1 = true;
                       m_Bomb1.transform.position += transform.up;
                   }
                }
            }
            //activates the second bom
            if (m_Bomb2 && m_IsBombDestroyed2 == false && m_IsBombDestroyed1 == true)
            {
                if (Input.GetAxis("Interact") > 0.0f)
                {
                   if (m_IsBombPlaced2 == false)
                   {
                        m_IsBombPlaced2 = true;
                        m_Bomb2.transform.position += transform.up;
                   }
                }
            }
            //toggle score increase when player is in the moneyArea and the bombs are destroyed
            if (m_IsBombDestroyed1 == true && m_IsBombDestroyed2 == true)
            {
                if (m_DoOnce == false)
                {
                    m_MoneyCollected = true;
                    m_CollectingMoney = true;
                    m_DoOnce = true;
                }
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

        //player will not collect money if he leaves the moneyArea
        if (m_CollectingMoney == true)
        {
            Debug.Log("Get inside the area and press E to collect money again");
            m_CollectingMoney = false;
            m_DoOnce = false;
        }
    }
}
