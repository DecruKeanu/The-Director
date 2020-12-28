using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class AmmoPickUp : MonoBehaviour
{
    private ShootingBehaviour m_Behaviour = null;
    [SerializeField] private AudioSource m_PickUpSound = null;
    private float m_PickUpSoundLength = 1.241f;
    private float m_Timer;
    private bool m_PickUpActivated = false;
    void Start()
    {
        //finds the shootingbehaviour attached to the player
        m_Behaviour = FindObjectOfType<PlayerCharacter>().gameObject.GetComponent<ShootingBehaviour>();
    }

    private void Update()
    {
        if (m_PickUpActivated)
        {
            m_Timer += Time.deltaTime;
            if (m_Timer > m_PickUpSoundLength)
            {
                Destroy(gameObject);
            }
        }
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        //makes sure only a friendly object can collide with pickUp
        if (other.tag != "Friendly" || m_Behaviour == null)
        {
            return;
        }

        //increases ammo depending on what weapon the player uses and destroys the object after that if it exists
        if (m_Behaviour && m_PickUpSound)
        {
            m_PickUpSound.Play();
            if (m_Behaviour.m_WhichWeapon == ShootingBehaviour.Weapon.Shotgun)
                m_Behaviour.getShotgun.increase(2);
            if (m_Behaviour.m_WhichWeapon == ShootingBehaviour.Weapon.assaultGun)
                m_Behaviour.getAssaultGun.increase(18);
            if (m_Behaviour.m_WhichWeapon == ShootingBehaviour.Weapon.SMG)
                m_Behaviour.getSMG.increase(40);

            //yeet that pickUp to space
            gameObject.transform.position = gameObject.transform.position + (gameObject.transform.up * 15);
            m_PickUpActivated = true;
        }
    }
}
