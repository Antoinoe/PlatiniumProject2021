using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DebugAgent : DebugScript
{
    private NavMeshAgent _navMesh;

    private void Start()
    {
        _navMesh = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        Debug.DrawLine(transform.position, _navMesh.destination, Color.green);
    }
}
