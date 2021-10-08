using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

//A placer en parent de toutes les IA

public class IAMovement : MonoBehaviour
{
    public bool axisMovement = true;
    public float patrollingRange = 4f;
    public Vector3 posPatrollingCenter = Vector3.zero;
    private Vector3 target = Vector3.zero;
    public int nextTargetMax = 1;
    public int nextTargetMin = 0;

    public enum MovementAxis
    {
        X, Y
    }

    #region Gizmos

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(posPatrollingCenter, patrollingRange);

        Gizmos.color = Color.green;
        Gizmos.DrawSphere(target, 0.5f);
    }

    #endregion

    #region Moving
    public void NewTarget(List<NavMeshAgent> entities)
    {
      /*float myX = posPatrollingCenter.x;
        float myY = posPatrollingCenter.y; */
        for (int i = 0; i < entities.Count; i++)
        {
            Vector2 iaPos = entities[i].gameObject.transform.position;
            float myX = iaPos.x;
            float myY = iaPos.y;
            float yPos = myY + Random.Range(myY -  (patrollingRange*2), myY + patrollingRange);
        float xPos = myX + Random.Range(myX - patrollingRange, myX + patrollingRange);
                if (axisMovement)
                {
                    MovementAxis movement = (MovementAxis)Random.Range(0, 2);
                    switch (movement)
                    {
                        case MovementAxis.X:
                            target = new Vector3(xPos, entities[i].transform.position.y, entities[i].transform.position.z);
                            break;
                        case MovementAxis.Y:
                            target = new Vector3(entities[i].transform.position.x, yPos, entities[i].transform.position.z);
                            break;
                        default:
                            break;
                    }
                }
            
            NavMeshPath path = new NavMeshPath();
            if (entities[i].CalculatePath(target, path))
            {
                entities[i].SetDestination(target);
            }
        }
    }
    #endregion
}

