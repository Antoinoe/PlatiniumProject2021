using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using MiscUtil.Xml.Linq.Extensions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;


public class AIController : MonoBehaviour
{

    public enum IndexNavigator
    {
        Next,
        Back
    }

    //public IAMovement iAController = null;

    //public Transform[] zonePoint = new Transform[] { };

    private Vector3 zonePoint = Vector3.zero;

    //private Vector2 localMove = Vector2.zero;

    public float rangePoint = 0.25f;

    public Vector2 mid;

    private Vector2 endPos;

    [Range(0, 3)]
    public float delay;

    public float localMoveRange = 1;

    private int index = 0;

    public IndexNavigator indexNavigator;

    private bool hasArriveToLocalPoint = false;

    //private bool hasArriveToPoint = false;

    public bool showDebug = false;

    private bool isWating = false;

    private bool canMove = true;

    private NavMeshAgent currentEntity = null;

    #region  UnityFunction

    private void Start()
    {
        currentEntity = GetComponent<NavMeshAgent>();
        currentEntity.updateRotation = false;
        currentEntity.updateUpAxis = false;
        zonePoint = transform.position;
        zonePoint = GameManager.RandomNavmeshLocation(localMoveRange, transform.position);
        currentEntity.SetDestination(zonePoint);
        //MoveSubPoint();
    }

    private void FixedUpdate()
    {
        UpdateNav();
        //if (hasArriveToLocalPoint)
        //{
        //    MoveSubPoint();
        //}
    }

    //Debug

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(zonePoint, localMoveRange);
    }

    //Debug

    private void OnGUI()
    {
        if (showDebug)
        {
            GUILayout.Label("NavPoint index : " + index);
            GUILayout.Label("indexNavigator : " + indexNavigator.ToString());
            GUILayout.Label("hasArriveToLocalPoint : " + hasArriveToLocalPoint);
            GUILayout.Label("Is waiting : " + isWating);
        }
    }

    #endregion

    #region Function Movement

    //Vérifie si L'IA atteint le point demandé

    public void UpdateNav()
    {
        //if (IAMovement.NavmeshReachedDestination(currentEntity, endPos, rangePoint))
        //{
        //    hasArriveToLocalPoint = true;
        //}

        if (IAMovement.NavmeshReachedDestination(currentEntity, zonePoint, rangePoint))
        {
            //switch (indexNavigator)
            //{
            //    case IndexNavigator.Back:
            //        if (index == 0)
            //        {
            //            indexNavigator = IndexNavigator.Next;
            //        }
            //        else
            //        {
            //            index--;
            //        }

            //        break;
            //    case IndexNavigator.Next:
            //        if (index >= (zonePoint.Length - 1))
            //        {
            //            indexNavigator = IndexNavigator.Back;
            //        }
            //        else
            //        {
            //            index++;
            //        }

            //        break;
            //}
            zonePoint = GameManager.RandomNavmeshLocation(localMoveRange, transform.position);
            //hasArriveToPoint = false;
            //hasArriveToLocalPoint = true;

            if (!isWating)
            {
                StartCoroutine(Delay());
                currentEntity.SetDestination(zonePoint);
            }
        }

    }

    //Fonction de déplacement par sous point

    //private void MoveSubPoint()
    //{
    //    if (canMove)
    //    {
    //        hasArriveToLocalPoint = false;
    //        mid = TestDist.SetRandPos(transform, zonePoint, localMoveRange);
    //        if (Vector2.Distance(transform.position, zonePoint) < localMoveRange)
    //        {
    //            hasArriveToPoint = true;
    //            currentEntity.SetDestination(zonePoint);
    //        }
    //        else
    //        {
    //            endPos = mid + Random.insideUnitCircle * localMoveRange;
    //            currentEntity.SetDestination(endPos);
    //        }
    //    }

    //}


    #endregion


    #region  Coroutine
    public IEnumerator Delay()
    {
        canMove = false;
        isWating = true;
        yield return new WaitForSeconds(Random.Range(1, delay));
        currentEntity.SetDestination(zonePoint);
        Debug.Log("fin de delay");
        canMove = true;
        isWating = false;
        yield return null;
    }

    #endregion

}