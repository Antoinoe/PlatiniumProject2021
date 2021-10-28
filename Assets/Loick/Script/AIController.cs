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

    private Vector2 zonePoint = Vector2.zero;

    private Vector2 previousPoint = Vector2.zero;

    [Range(0.1f, 1)]
    public float rangePoint = 0.25f;

    public float delayMin = 1;
    private float delay = 0;
    public float delayMax = 2;

    public float localMinMoveRange = 1;
    private float randomRange = 0;
    public float localMaxMoveRange = 5;

    public bool showDebug = false;
    private bool hasArriveToLocalPoint = false;
    private bool isWating = false;
    private bool canMove = true;

    private NavMeshAgent currentEntity = null;

    public CircleOrientation.Orientation currentOrientation;

    EntityMoveFeel feel;

    //Code pour que les sprites passent deriere les autres éléments en fonction de leurs hauteur Y
    //Il faut le metrre dans les joueurs et les IA et crée un autre Sorting layer puis ajouter IA et Player dans le nouveau sorting layer
    //Déclaration Variable
    private SpriteRenderer sprite;


    #region  UnityFunction

    private void Start()
    {
        currentEntity = GetComponent<NavMeshAgent>();
        currentEntity.updateRotation = false;
        currentEntity.updateUpAxis = false;
        sprite = GetComponentInChildren<SpriteRenderer>();
        zonePoint = transform.position;
        randomRange = GetRandomRange();
        zonePoint = GameManager.RandomNavmeshLocation(randomRange, transform.position, currentOrientation);
        currentEntity.SetDestination(zonePoint);
        feel = GetComponent<EntityMoveFeel>();
    }

    private void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        sprite.sortingOrder = Mathf.RoundToInt(transform.position.y * -10f);
        if (feel.IsMoving != canMove) feel.IsMoving = canMove;
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
        Gizmos.DrawSphere(previousPoint, 0.1f);
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(zonePoint, rangePoint);
    }

    //Debug

    private void OnGUI()
    {
        if (showDebug)
        {
            GUILayout.Label("hasArriveToLocalPoint : " + hasArriveToLocalPoint);
            GUILayout.Label("Is waiting : " + isWating);
            GUILayout.Label("Delay : " + delay);
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
            zonePoint = GameManager.RandomNavmeshLocation(randomRange, transform.position, currentOrientation);

            if (!isWating)
            {
                StartCoroutine(Delay());
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
        canMove = true;
        isWating = false;
        yield return null;
    }

    #endregion

    public class CircleOrientation
    {
        public CircleOrientation(Orientation currentOrientation)
        {
            switch (currentOrientation)
            {
                case Orientation.UpRight:
                    angleMin = 0;
                    angleMax = Mathf.PI / 2;
                    break;
                case Orientation.UpLeft:
                    angleMin = Mathf.PI / 2;
                    angleMax = Mathf.PI;
                    break;
                case Orientation.DownLeft:
                    angleMin = Mathf.PI;
                    angleMax = 3 * Mathf.PI / 2;
                    break;
                case Orientation.DownRight:
                    angleMin = 3 * Mathf.PI / 2;
                    angleMax = Mathf.PI * 2;
                    break;
            }
        }
        public enum Orientation
        {
            UpLeft, UpRight, DownLeft, DownRight
        }

        public float angleMin;
        public float angleMax;
    }
}