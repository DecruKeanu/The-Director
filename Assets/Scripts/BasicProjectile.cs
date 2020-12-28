using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class BasicProjectile : MonoBehaviour
{
    [SerializeField] private float m_Speed = 30.0f;
    [SerializeField] private float m_LifeTime = 10.0f;
    [SerializeField] private int m_Damage = 5;

    private void Awake()
    {
        //destroys the bullet if it reaches lifeTime
        Invoke("kill", m_LifeTime);
    }

    private void FixedUpdate()
    {
        //if the bullet does not hit a wall continue moving
        if (!WallDetection())
        {
            transform.position += transform.forward * Time.deltaTime * m_Speed;
        }
    }

    //when the bullet hits a wall destroy it
    static string[] RAYCAST_MASK = new string[] { "StaticLevel", "DynamicLevel" };
    private bool WallDetection()
    {
        Ray collisonRay = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(collisonRay,out RaycastHit hit,Time.deltaTime * m_Speed, LayerMask.GetMask(RAYCAST_MASK)))
        {
            kill();
            return true;
        }
        return false;
    }

    private void kill()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        //make sure we only hit friendly or enemies
        if (other.tag != "Friendly" && other.tag != "Enemy")
        {
            return;
        }

        //gets the health of the object the bullet interacts with
        Health otherHealth = other.GetComponent<Health>();

        //if otherhealth exist decrease it with a given parameter and destroy the bullet
        if (otherHealth)
        {
            //Debug.Log("hit");
            otherHealth.Damage(m_Damage);
            kill();
        }

    }
}
