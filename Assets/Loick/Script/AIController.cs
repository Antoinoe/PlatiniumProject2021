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

    private Vector2 zonePoint = Vector2.zero;

    private Vector2 previousPoint = Vector2.zero;

    //private Vector2 localMove = Vector2.zero;
    [Range(0.1f, 1)]
    public float rangePoint = 0.25f;

    //public Vector2 mid;

    private Vector2 endPos;

    public float delayMin = 1;
    private float delay = 0;
    public float delayMax = 2;

    public float localMinMoveRange = 1;
    private float randomRange = 0;
    public float localMaxMoveRange = 5;

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
        randomRange = GetRandomRange();
        zonePoint = GameManager.RandomNavmeshLocation(randomRange, transform.position);
        currentEntity.SetDestination(zonePoint);
    }

    private void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        UpdateNav();
    }

    //Debug

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(previousPoint, localMaxMoveRange);       
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(previousPoint, localMinMoveRange);
        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(previousPoint,0.1f);
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(zonePoint,0.1f);
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
            GUILayout.Label("Delay : "+ delay);
            GUILayout.Label("Random Range : " + randomRange);
        }
    }

    #endregion

    #region Function Movement

    //Vérifie si L'IA atteint le point demandé

    public void UpdateNav()
    {
        if (IAMovement.NavmeshReachedDestination(currentEntity, zonePoint, rangePoint))
        {
            previousPoint = transform.position;
            randomRange = GetRandomRange();
            zonePoint = GameManager.RandomNavmeshLocation(randomRange, transform.position);

            if (!isWating)
            {
                StartCoroutine(Delay());
                currentEntity.SetDestination(zonePoint);
            }
        }

    }

    //Retourne un nombre aléatoire entre localMaxMoveRange et localMinMoveRange

    private float GetRandomRange()
    {
        return Random.Range(localMinMoveRange, localMaxMoveRange);
    }

    #endregion

    // Definit un temps d'arret ou l'IA ne bouge pas

    #region  Coroutine
    public IEnumerator Delay()
    {
        canMove = false;
        isWating = true;
        delay = Random.Range(delayMin, delayMax);
        yield return new WaitForSeconds(delay);
        currentEntity.SetDestination(zonePoint);
        Debug.Log("fin de delay");
        canMove = true;
        isWating = false;
        yield return null;
    }

    #endregion

}