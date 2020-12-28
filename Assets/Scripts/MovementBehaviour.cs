using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementBehaviour : MonoBehaviour
{
    [SerializeField] protected float m_MovementSpeed = 10.0f;
    protected Rigidbody m_RigidBody;
    protected Vector3 m_DesiredMovementDirection = Vector3.zero;
    protected Vector3 m_DesiredLookatpoint = Vector3.zero;
    protected GameObject m_Target;

    public Vector3 DesiredMovementDirection
    {
        get { return m_DesiredMovementDirection; }
        set { m_DesiredMovementDirection = value; }
    }

    public Vector3 DesiredLookatPoint
    {
        get { return m_DesiredLookatpoint; }
        set { m_DesiredLookatpoint = value; }
    }

    public GameObject Target
    {
        get { return m_Target; }
        set { m_Target = value; }
    }

    protected virtual void Awake()
    {
        m_RigidBody = GetComponent<Rigidbody>();
    }

    protected virtual void Update()
    {
        HandleRotation();
    }


    protected virtual void FixedUpdate()
    {
        HandleMovement();
    }

    protected virtual void HandleMovement()
    {
        //move the rigidBody of our object to the desiredMovementDirection
        Vector3 movement = m_DesiredMovementDirection.normalized;
        movement *= m_MovementSpeed * Time.deltaTime;

        if (m_RigidBody)
        m_RigidBody.MovePosition(m_RigidBody.position + movement);
    }

    protected virtual void HandleRotation()
    {
        //rotates the rigidBody of our object to the desiredMovementDirection
        transform.LookAt(DesiredLookatPoint, Vector3.up);
    }
}
