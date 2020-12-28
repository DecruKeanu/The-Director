using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]

public class NavMeshMovementBehaviour : MovementBehaviour
{
    //include the namespace UnityEngine.AI
    private NavMeshAgent m_NavMeshAgent;
    private Vector3 m_PreviousTargetPosition = Vector3.zero;
    private Health m_Health = null;

    protected override void Awake()
    {
        base.Awake();

        m_NavMeshAgent = GetComponent<NavMeshAgent>();
        if (m_NavMeshAgent)
            m_NavMeshAgent.speed = m_MovementSpeed;

        m_PreviousTargetPosition = transform.position;

        m_Health = GetComponent<Health>();
    }

    const float MOVEMENT_EPSILON = .25f;

    protected override void HandleMovement()
    {
        if (!m_NavMeshAgent)
            return;

        //if the target does not exist but our navmesh does stop the navMesh for that target
        if (!m_Target)
        {
            m_NavMeshAgent.isStopped = true;
            return;
        }


        //should the target move we should recalculate our path
        if ((m_Target.transform.position - m_PreviousTargetPosition).sqrMagnitude > MOVEMENT_EPSILON)
        {
            m_NavMeshAgent.SetDestination(m_Target.transform.position);
            m_NavMeshAgent.isStopped = false;
            m_PreviousTargetPosition = m_Target.transform.position;
        }

        transform.LookAt(m_DesiredLookatpoint, Vector3.up);
        if (m_Health.HeatlhPercentage > 0)
        {
            m_MovementSpeed = 0;
        }
    }
}
