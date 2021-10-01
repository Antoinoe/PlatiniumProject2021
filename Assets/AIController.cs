using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//A placé enfant de l'IAMovement

public class AIController : MonoBehaviour
{
    private IAMovement iAController = null; 
    private int nextTarget = 1;
    private float timer = 0f;

    private void Start()
    {
        iAController = GetComponentInParent<IAMovement>();
        NavMeshAgent navAgent = GetComponent<NavMeshAgent>();
        navAgent.updateRotation = false;
        navAgent.updateUpAxis = false;
    }

    private void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;

        if (timer >= nextTarget)
        {
            nextTarget = Random.Range(1, iAController.nextTargetMax);
            iAController.NewTarget(gameObject);
            timer = 0;
        }
    }
}

