using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

//A placer en parent de toutes les IA

public class IAMovement : MonoBehaviour
{
    public bool patrolling = false;
    public bool axisMovement = true;
    public float patrollingRange = 4f;
    public Vector3 posPatrollingCenter = Vector3.zero;
    private Vector3 target = Vector3.zero;
    public int nextTargetMax = 1;

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
    public void NewTarget(GameObject entitie)
    {
        float myX = posPatrollingCenter.x;
        float myY = posPatrollingCenter.y;
        float yPos = myY + Random.Range(myY - patrollingRange, myY + patrollingRange);
        float xPos = myX + Random.Range(myX - patrollingRange, myX + patrollingRange);
        if (axisMovement)
        {
            MovementAxis movement = (MovementAxis)Random.Range(0, 2);
            switch (movement)
            {
                case MovementAxis.X:
                    target = new Vector3(xPos, entitie.transform.position.y, entitie.transform.position.z);
                    break;
                case MovementAxis.Y:
                    target = new Vector3(entitie.transform.position.x, yPos, entitie.transform.position.z);
                    break;
                default:
                    break;
            }
        }
        else
        {
            target = new Vector3(xPos, yPos, entitie.transform.position.y);
        }
            entitie.GetComponent<NavMeshAgent>().SetDestination(target);
    }
    #endregion
}
