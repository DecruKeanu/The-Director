using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickUp : MonoBehaviour
{
    [SerializeField] private int m_HealthIncrease = 25;
    [SerializeField] private AudioSource m_PickUpSound = null;
    private float m_PickUpSoundLength = 1.241f;
    private float m_Timer;
    private bool m_PickUpActivated = false;


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
        if (other.tag != "Friendly")
        {
            return;
        }

        //finds the health of the object the PickUp collides with
        Health otherHealth = other.GetComponent<Health>();

        //if otherhealth exist increase it with the given value and destroy the pickUp
        if (otherHealth && m_PickUpSound)
        {
            m_PickUpSound.Play();
            otherHealth.heal(m_HealthIncrease);

            //yeet that pickUp to space
            gameObject.transform.position = gameObject.transform.position + (gameObject.transform.up * 15);
            m_PickUpActivated = true;
        }
    }
}
