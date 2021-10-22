using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//A placer enfant de IAMovement

public class AIControllers : MonoBehaviour
{
    private IAMovement iAController = null;
    private int nextTarget = 1;
    private float timer = 0f;
    
    private List<NavMeshAgent> entitiesGroup = new List<NavMeshAgent>();
    private NavMeshAgent currentEntity;

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

        currentEntity = entitiesGroup[0];
    }

    private void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;

        if (timer >= nextTarget /*&& IAMovement.NavmeshReachedDestination(currentEntity,iAController.GetCurrentTarget())*/)
        {
            for (int i = 0; i < entitiesGroup.Count; i++)
            {
                nextTarget = Random.Range(iAController.nextTargetMin, iAController.nextTargetMax);
                    iAController.NewSubTarget(entitiesGroup[i]);
                    timer = 0;
            }

        }
    }
}

