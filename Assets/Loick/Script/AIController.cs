using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using MiscUtil.Xml.Linq.Extensions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class AIController : MonoBehaviour
{
    IAIdentity iAIdentity;
    NavMeshAgent agent;

    bool isDog;

    [SerializeField]
    float speed;

    float reviveTime = 3.0f;

    private Vector2 zonePoint = Vector2.zero;

    private Vector2 nextZonePoint = Vector2.zero;

    private Vector2 previousPoint = Vector2.zero;

    [Range(0.1f, 1)] public float rangePoint = 0.25f;

    [SerializeField] private int areaIndex = 0;
    [SerializeField] private AreaManager currentArea;
    private CircleCollider2D dogArea;
    private BoxCollider2D dogCollider;

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
    public bool dogTargetIsOn = false;
    private bool isBones = false;
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

    //Code pour que les sprites passent deriere les autres éléments en fonction de leurs hauteur Y
    //Il faut le metrre dans les joueurs et les IA et crée un autre Sorting layer puis ajouter IA et Player dans le nouveau sorting layer
    //Déclaration Variable
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
        GetComponent<NavMeshAgent>().speed = Speed;
        /*if (agent != null)
            agent.speed = Speed;
        else
            GetComponent<NavMeshAgent>().speed = Speed;*/
    }

    #endregion

    #region UnityFunction

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        isDog = gameObject.CompareTag("SecondGoal");
        iAIdentity = GetComponent<IAIdentity>();  
        anim = GetComponentInChildren<Animator>();
        anim.SetFloat("playerNbr", iAIdentity.teamNb);
        color = GetComponentInChildren<SpriteRenderer>().color;
        deadColor = new Color(1, 1, 1, 0);
        aliveColor = new Color(1, 1, 1, 1);
        currentArea = GameObject.FindGameObjectWithTag("Area").GetComponent<AreaManager>();
        dogArea = GameObject.FindGameObjectWithTag("SecondGoal").GetComponentInChildren<CircleCollider2D>();
        dogCollider = GameObject.FindGameObjectWithTag("SecondGoal").GetComponent<BoxCollider2D>();
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
        UpdateNav();
    }

    private void FixedUpdate()
    {
        velocity = (transform.position - previous) / Time.deltaTime;
        previous = transform.position;

        if ((velocity.x != 0 || velocity.y != 0)/* && !anim.GetBool("isWalking")*/)
        {
            anim.SetBool("isWalking", true);
            if (velocity.x < 0)
                sprite.flipX = !isDog;
            else
                sprite.flipX = isDog;
        }
        else /*if ((velocity.x == 0 || velocity.y == 0) && anim.GetBool("isWalking"))*/
        {
            anim.SetBool("isWalking", false);
            if(!isDog)
                sprite.flipX = false;
        }
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

    public void ChangeTeam()
    {
        anim.SetFloat("playerNbr", iAIdentity.teamNb);
    }

    public void OnBone()
    {
        if (Physics2D.Distance(dogArea, GetComponent<Collider2D>()).isOverlapped)
        {
            dogArea.enabled = false;
            Debug.Log("Bones");
            agent.speed = 0;
            GetComponentInChildren<SpriteRenderer>().color = Color.red;
            isDead = true;
            isBones = true;
            GameObject.FindGameObjectWithTag("SecondGoal").GetComponent<AIController>().DogTarget(this.transform.position);
            StartCoroutine(OnEaten());
        }
    }

    private IEnumerator OnEaten()
    {
        while (!Physics2D.Distance(dogCollider, GetComponent<Collider2D>()).isOverlapped)
        {
            yield return null;
        }
        Debug.Log("eat");
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponentInChildren<SpriteRenderer>().color = Color.black;
        //DogEat();
        GameObject.FindGameObjectWithTag("SecondGoal").GetComponent<AIController>().dogTargetIsOn = false; 
        dogArea.enabled = true;
        yield break;
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

    //Vérifie si L'IA atteint le point demandé

    public void UpdateNav()
    {
        if (IAMovement.NavmeshReachedDestination(currentEntity, zonePoint, rangePoint))
        {
            currentArea.ChangeCurrentAreaCollider(this);
            if (anim) anim.SetBool("isWalking", false);
            previousPoint = transform.position;
            randomRange = GetRandomRange();

            if (CompareTag("SecondGoal") && dogTargetIsOn) { zonePoint = nextZonePoint; }
            else if (areaColliderIsOn)
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

    public void DogTarget(Vector2 vector2)
    {
        dogTargetIsOn = true;
        nextZonePoint = vector2;
    }

    public void SetIndexArea(int newIndex)
    {
        areaIndex = newIndex;
    }

    //Retourne un nombre aléatoire entre localMaxMoveRange et localMinMoveRange

    private float GetRandomRange()
    {
        return Random.Range(localMinMoveRange, localMaxMoveRange);
    }

    #endregion

    #region Zone Area Function

    public int GetAreaIndex()
    {
        return areaIndex;
    }

    public void SetAreaIndex(int newIndex)
    {
        areaIndex = newIndex;
    }

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
                case Orientation.Right:
                    angleMin = 0;
                    angleMin = 0;
                    break;
                case Orientation.Left:
                    angleMax = 180;
                    angleMin = 180;
                    break;
                case Orientation.Up:
                    angleMin = 90;
                    angleMax = 90;
                    break;
                case Orientation.Down:
                    angleMin = 270;
                    angleMax = 270;
                    break;
            }
        }

        public enum Orientation
        {
            UpLeft,
            UpRight,
            DownLeft,
            DownRight,Up,Down,
            Left,Right
            
        }

        public float angleMin;
        public float angleMax;
    }

    [System.Serializable] 
    public class AreaCollider
    {
        public List<Collider2D> zonesColliders;

        public bool CheckPointIsInZonescollider(Vector2 point)
        {
            for (int i = 0; i < zonesColliders.Count; i++)
            {
                Bounds areaBounds = zonesColliders[i].bounds;
                if (point.x > areaBounds.max.x || point.y > areaBounds.max.y || point.y < areaBounds.min.y || point.x < areaBounds.min.x)
                {
                    return true;
                }
            }
            return false;
        }
    }
}