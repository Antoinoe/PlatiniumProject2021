using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DebugAgent : MonoBehaviour
{
    private NavMeshAgent navMesh;

    private void Start()
    {
        navMesh = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        Debug.DrawLine(transform.position, navMesh.destination, Color.green);
    }
}
