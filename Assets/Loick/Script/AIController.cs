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
    IAIdentity iAIdentity;
    NavMeshAgent agent;

    float speed;

    float reviveTime = 3.0f;

    private Vector2 zonePoint = Vector2.zero;

    private Vector2 previousPoint = Vector2.zero;

    [Range(0.1f, 1)] public float rangePoint = 0.25f;

    [SerializeField] private int areaIndex = 0;
    [SerializeField] private AreaManager currentArea;

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
    public bool areaColliderIsOn = false;

    Color color;
    Color deadColor;
    Color aliveColor;

    private NavMeshAgent currentEntity = null;

    public CircleOrientation.Orientation currentOrientation;

    EntityMoveFeel feel;

    bool isDead;

    Animator anim;

    Vector3 previous;
    Vector3 velocity;

    //Code pour que les sprites passent deriere les autres �l�ments en fonction de leurs hauteur Y
    //Il faut le metrre dans les joueurs et les IA et cr�e un autre Sorting layer puis ajouter IA et Player dans le nouveau sorting layer
    //D�claration Variable
    private SpriteRenderer sprite;

    bool showGizmos = true;

    public bool ShowGizmos
    {
        get { return showGizmos; }
        set { showGizmos = value; }
    }

    public float Speed
    {
        get { return speed;/*GetComponent<NavMeshAgent>().speed;*/ }
        set { speed = value; /*GetComponent<NavMeshAgent>().speed = value;*/ }
    }

    public float ReviveTime
    {
        get { return reviveTime; }
        set { reviveTime = value; }
    }

    #region ChangeVariables
    public void OnValuesChanged
    (bool _showGizmo, float _speed,
    Vector2 _moveRange, Vector2 _moveTime)
    {
        ShowGizmos = _showGizmo;
        Speed = _speed;
        localMinMoveRange = _moveRange.x;
        localMaxMoveRange = _moveRange.y;
        delayMin = _moveTime.x;
        delayMax = _moveTime.y;

        agent.speed = Speed;
    }

    #endregion

    #region UnityFunction

    private void Start()
    {
        iAIdentity = GetComponent<IAIdentity>();
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        anim.SetFloat("playerNbr", iAIdentity.teamNb);
        color = GetComponentInChildren<SpriteRenderer>().color;
        deadColor = new Color(1, 1, 1, 0);
        aliveColor = new Color(1, 1, 1, 1);
        currentArea = GameObject.FindGameObjectWithTag("Area").GetComponent<AreaManager>();
        areaColliderIsOn = (currentArea != null);
        currentEntity = GetComponent<NavMeshAgent>();
        currentEntity.updateRotation = false;
        currentEntity.updateUpAxis = false;
        sprite = GetComponentInChildren<SpriteRenderer>();
        zonePoint = transform.position;
        randomRange = GetRandomRange();
        if (areaColliderIsOn)
        { 
            zonePoint = GameManager.RandomNavmeshLocation(randomRange, transform.position,
            currentOrientation, GetCurrentAreaCollider().zonesColliders);
        }
        else
        {
            zonePoint = GameManager.RandomNavmeshLocation(randomRange, transform.position, currentOrientation);
        }
        currentEntity.SetDestination(zonePoint);
        feel = GetComponent<EntityMoveFeel>();
    }

    private void Update()
    {
        sprite.sortingOrder = Mathf.RoundToInt(transform.position.y * -10f);
        if (feel.IsMoving != canMove) feel.IsMoving = canMove;
        UpdateNav();
    }

    private void FixedUpdate()
    {
        velocity = (transform.position - previous) / Time.deltaTime;
        previous = transform.position;
    }

    //Debug

    void OnDrawGizmos()
    {
        if (showGizmos)
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

    #region Kill

    public void OnKilled()
    {
        //Debug.Log("Dead");
        agent.speed = 0;
        GetComponentInChildren<SpriteRenderer>().color = deadColor;
        isDead = true;
        GetComponent<BoxCollider2D>().enabled = false;
        StartCoroutine(Revive());
    }

    IEnumerator Revive()
    {
        yield return new WaitForSeconds(reviveTime);
        OnRevive();
        yield return null;
    }

    void OnRevive()
    {
        //Debug.Log("Revive");
        agent.speed = Speed;
        GetComponentInChildren<SpriteRenderer>().color = aliveColor;
        isDead = false;
        GetComponent<BoxCollider2D>().enabled = true;
    }

    #endregion

    #region Function Movement

    //V�rifie si L'IA atteint le point demand�

    public void UpdateNav()
    {
        if (IAMovement.NavmeshReachedDestination(currentEntity, zonePoint, rangePoint))
        {
            if (anim) anim.SetBool("isWalking", false);
            previousPoint = transform.position;
            randomRange = GetRandomRange();
            if (areaColliderIsOn)
            {
                zonePoint = GameManager.RandomNavmeshLocation(randomRange, transform.position,
                    currentOrientation, GetCurrentAreaCollider().zonesColliders);
            }
            else
            {
                zonePoint = GameManager.RandomNavmeshLocation(randomRange, transform.position, currentOrientation);
            }
            if (!isWating)
            {
                StartCoroutine(Delay());
            }
        }

    }

    public void SetIndexArea(int newIndex)
    {
        areaIndex = newIndex;
    }

    //Retourne un nombre al�atoire entre localMaxMoveRange et localMinMoveRange

    private float GetRandomRange()
    {
        return Random.Range(localMinMoveRange, localMaxMoveRange);
    }

    #endregion

    #region Zone Area Function

    public AreaCollider GetCurrentAreaCollider()
    {
        return currentArea.areaColliders[areaIndex];
    }

    #endregion

    // Definit un temps d'arret ou l'IA ne bouge pas

    #region Function Delay

    public IEnumerator Delay()
    {
        canMove = false;
        isWating = true;
        delay = Random.Range(delayMin, delayMax);
        yield return new WaitForSeconds(delay);
        currentEntity.SetDestination(zonePoint);
        canMove = true;
        isWating = false;
        if (anim) anim.SetBool("isWalking", true);
        yield return null;
    }

    public float GetDelay()
    {
        return delay;
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
            UpLeft,
            UpRight,
            DownLeft,
            DownRight
        }

        public float angleMin;
        public float angleMax;
    }

    [System.Serializable]
    public class AreaCollider
    {
        public List<Collider2D> zonesColliders;
    }
}