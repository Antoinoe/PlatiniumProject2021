using System.Collections;
using System.Collections.Generic;
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

    public float localMoveRange = 1;


    private int index = 0;

    public IndexNavigator indexNavigator;

    private bool hasArriveToPoint = true;

    public bool showDebug = false;

    private NavMeshAgent currentEntity = null;

    private void Start()
    {
        currentEntity = GetComponent<NavMeshAgent>();
    }

    private void FixedUpdate()
    {
        UpdateNav();
    }

    private Transform GetNextTransform()
    {
        if (index >= (zonePoint.Length - 1))
        {
            indexNavigator = IndexNavigator.Back;
        }
        else if (index == 0)
        {
            indexNavigator = IndexNavigator.Back;
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

    private float GetDistanceToNextAndCurrentTransform()
    {
        return Vector2.Distance(zonePoint[index].position, GetNextTransform().position);
    }

    public void RandomNav()
    {
        Vector2 middlepos = new Vector2((zonePoint[index].position.x + GetNextTransform().position.x) / 2,(zonePoint[index].position.y + GetNextTransform().position.y));
    }

    public void UpdateNav()
    {
                if (IAMovement.NavmeshReachedDestination(currentEntity, zonePoint[index].position, rangePoint))
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

                    currentEntity.SetDestination(zonePoint[index].position);
                }

    }

    private void OnGUI()
    {
        if (showDebug)
        {
            GUILayout.Label("NavPoint index : " + index);
            GUILayout.Label("indexNavigator : " + indexNavigator.ToString());
            GUILayout.Label("hasArriveToPoint : " + hasArriveToPoint);
        }
    }

void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, localMoveRange);
    }
}


