using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class GrenadeProjectile : MonoBehaviour
{
    [SerializeField] private float m_Speed = 1.0f;
    [SerializeField] private float m_LifeTime = 10.0f;
    [SerializeField] private int m_Damage = 100;
    [SerializeField] private float m_ExplosionRadius = 0.01f;
    [SerializeField] private AudioSource m_ExplosionSound = null;
    private bool m_DoOnce = false;
    [SerializeField] private Rigidbody m_RigidBody= null;
    private bool m_Hit = false;
   
    private void Awake()
    {
        //destroys the bullet if it reaches lifeTime
        Invoke("kill", m_LifeTime);
        //ExplosionSound.Stop();
    }

    private void FixedUpdate()
    {
        //if the grenade doesnt hit anything continue moving
        if (!m_Hit)
        {
            transform.position += transform.forward * Time.deltaTime * m_Speed;
        }
    }

    //desroys the bullet
    private void kill()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        //make sure we only hit friendly or enemies (more performant)
        if (other.tag != "Friendly" && other.tag != "Enemy" && other.tag != "Level")
        {
            return;
        }

        //make sure we only hit the opossing team
        if (other.tag == tag)
        {
            return;
        }
        
        //if the grenade collides with an enemy get its healt and dame it with a value
        if (other.tag == "Enemy")
        {
            Health otherHealth = other.GetComponent<Health>();
            if(otherHealth)
            otherHealth.Damage(m_Damage);
        }
        
        //if the grenade collides with a levelObject increase it radius (like an explosion) and disables gravity
        if (other.tag == "Level")
        {
            if (m_DoOnce == false)
            {
                m_ExplosionSound.Play();
                transform.localScale = transform.localScale * m_ExplosionRadius;
                m_Hit = true;
                m_RigidBody.velocity = new Vector3(0, 0, 0);
                m_RigidBody.useGravity = false;
                m_DoOnce = true;
            }
        }
    }
}
