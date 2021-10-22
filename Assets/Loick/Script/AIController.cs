using System.Collections;
using System.Collections.Generic;
using System.Net;
using MiscUtil.Xml.Linq.Extensions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;


public class AIController : MonoBehaviour
{

    public enum IndexNavigator
    {
        Next,
        Back
    }

    //public IAMovement iAController = null;

    public Transform[] zonePoint = new Transform[] { };

    private Vector2 localMove = Vector2.zero;

    public float rangePoint = 0.25f;

    public Vector2 mid;

    private Vector2 endPos;

    public float localMoveRange = 1;

    private int index = 0;

    public IndexNavigator indexNavigator;

    private bool hasArriveToLocalPoint = false;

    private bool hasArriveToPoint = false;

    public bool showDebug = false;

    private NavMeshAgent currentEntity = null;

    private void Start()
    {
        currentEntity = GetComponent<NavMeshAgent>();
        MoveSubPoint();
    }

    private void FixedUpdate()
    {
        UpdateNav();
        if (hasArriveToLocalPoint)
        {
            MoveSubPoint();
        }
    }

    private void MoveSubPoint()
    {
        hasArriveToLocalPoint = false;
        mid = TestDist.SetRandPos(transform, zonePoint[index], localMoveRange);
        if (Vector2.Distance(transform.position,zonePoint[index].position)< localMoveRange)
        {
            hasArriveToPoint = true;
            currentEntity.SetDestination(zonePoint[index].position);
        }
        else
        {
            endPos = mid + Random.insideUnitCircle * localMoveRange;
            currentEntity.SetDestination(endPos);
        }
    }

    private Transform GetNextTransform()
    {
        if (index >= (zonePoint.Length - 1))
        {
            indexNavigator = IndexNavigator.Back;
        }
        else if (index == 0)
        {
            indexNavigator = IndexNavigator.Next;
        }

        switch (indexNavigator)
        {
            case IndexNavigator.Next:
                return zonePoint[(index + 1)];
            case IndexNavigator.Back:
                return zonePoint[(index - 1)];
        }

        return zonePoint[0];
    }

    public void UpdateNav()
    {
        if (IAMovement.NavmeshReachedDestination(currentEntity, endPos, rangePoint))
        {
            hasArriveToLocalPoint = true;
        }
        if (IAMovement.NavmeshReachedDestination(currentEntity, zonePoint[index].position, rangePoint) && hasArriveToPoint)
        {
            switch (indexNavigator)
            {
                case IndexNavigator.Back:
                    if (index == 0)
                    {
                        indexNavigator = IndexNavigator.Next;
                    }
                    else
                    {
                        index--;
                    }

                    break;
                case IndexNavigator.Next:
                    if (index >= (zonePoint.Length - 1))
                    {
                        indexNavigator = IndexNavigator.Back;
                    }
                    else
                    {
                        index++;
                    }

                    break;
            }

            hasArriveToPoint = false;
            hasArriveToLocalPoint = true;
            currentEntity.SetDestination(zonePoint[index].position);
        }

    }

    private void OnGUI()
    {
        if (showDebug)
        {
            GUILayout.Label("NavPoint index : " + index);
            GUILayout.Label("indexNavigator : " + indexNavigator.ToString());
            GUILayout.Label("hasArriveToLocalPoint : " + hasArriveToLocalPoint);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(mid, localMoveRange);
    }
}


