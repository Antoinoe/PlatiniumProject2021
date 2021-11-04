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
}

