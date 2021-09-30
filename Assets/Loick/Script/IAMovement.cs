using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class IAMovement : MonoBehaviour
{
    private NavMeshAgent navMeshAgent = null;
    private float timer = 0f;
    public bool patrolling = false;
    public float patrollingRange = 4f;
    public Vector3 target = Vector3.zero;
    public int nextTarget = 0;

    public Transform[] navpoint = null;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;

        if (timer >= nextTarget )
        {
            NewTarget();
            timer = 0;
        }
    }

    #region Gizmos

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, patrollingRange);
    }

    #endregion

    #region Moving
    void NewTarget()
    {
        float myX = gameObject.transform.position.x;
        float myZ = gameObject.transform.position.y;

        float xPos = myX + Random.Range(myX - patrollingRange, myX + patrollingRange);
        float zPos = myZ + Random.Range(myZ - patrollingRange, myZ + patrollingRange);

        target = new Vector3(xPos, transform.position.y, zPos);
        navMeshAgent.SetDestination(target);
    }

    #endregion
}
