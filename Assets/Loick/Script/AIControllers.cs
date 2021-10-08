using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//A placé enfant de l'IAMovement

public class AIControllers : MonoBehaviour
{
    private IAMovement iAController = null;
    private int nextTarget = 1;
    private float timer = 0f;
    private List<NavMeshAgent> entitiesGroup = new List<NavMeshAgent>();
    private int nextZone = 0;
    private void Start()
    {
        iAController = GetComponentInParent<IAMovement>();
        for (int i = 0; i < transform.childCount; i++)
        {
            NavMeshAgent agent = transform.GetChild(i).GetComponent<NavMeshAgent>();
            entitiesGroup.Add(agent);
            agent.updateRotation = false;
            agent.updateUpAxis = false;
        }
    }

    private void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;

        if (timer >= nextTarget)
        {
            nextTarget = Random.Range(iAController.nextTargetMin, iAController.nextTargetMax);
            iAController.NewTarget(entitiesGroup);
            timer = 0;
        }
    }
}

