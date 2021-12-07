using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using JetBrains.Annotations;
//using MiscUtil.Xml.Linq.Extensions;
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
    [SerializeField] private AreaManager areaManager;
    private bool eventAreaIsActive = false;
    private List<Collider2D> currentArea;

    private /*Circle*/BoxCollider2D dogArea;
    GameObject dog;
    private BoxCollider2D dogCollider;

    public float delayMin = 1;
    private float delay = 0;
    public float delayMax = 2;

    bool onDelay = false;

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

    bool isDead = false;

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
        anim.SetBool("isWalking", false);
        color = GetComponentInChildren<SpriteRenderer>().color;
        deadColor = new Color(1, 1, 1, 0);
        aliveColor = new Color(1, 1, 1, 1);
        areaManager = GameObject.FindGameObjectWithTag("Area").GetComponent<AreaManager>();
        SetDefaultArea();
        if (GameObject.FindGameObjectWithTag("SecondGoal"))
        {
            dogArea = GameObject.FindGameObjectWithTag("SecondGoal").GetComponentInChildren<BoxCollider2D>();
            dog = GameObject.FindGameObjectWithTag("SecondGoal");
            dogCollider = GameObject.FindGameObjectWithTag("SecondGoal").GetComponent<BoxCollider2D>();
        }
        areaColliderIsOn = (areaManager != null);
        currentEntity = GetComponent<NavMeshAgent>();
        currentEntity.updateRotation = false;
        currentEntity.updateUpAxis = false;
        sprite = GetComponentInChildren<SpriteRenderer>();
        zonePoint = transform.position;
        randomRange = GetRandomRange();
        if (areaColliderIsOn)
        {
            zonePoint = GameManager.RandomNavmeshLocation(randomRange, transform.position,
            ref currentOrientation, currentArea);
        }
        else
        {
            zonePoint = GameManager.RandomNavmeshLocation(randomRange, transform.position, ref currentOrientation);
        }
        StartCoroutine(Delay());
        feel = GetComponent<EntityMoveFeel>();
        transform.rotation = new Quaternion(0, 0, 0, 0);
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
        anim.SetFloat("X", velocity.x);
        anim.SetFloat("Y", velocity.y);

        if ((velocity.x != 0 || velocity.y != 0)/* && !anim.GetBool("isWalking")*/)
        {
            //Debug.Log("WALKING");
            if (canMove) anim.SetBool("isWalking", true);
            if (velocity.x < 0)
                sprite.flipX = !isDog;
            else
                sprite.flipX = isDog;
            //sprite.flipX = velocity.x < 0;
        }
        else /*if ((velocity.x == 0 || velocity.y == 0) && anim.GetBool("isWalking"))*/
        {
            anim.SetBool("isWalking", false);
            if (!isDog)
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
        /*
        if (showDebug)
        {
            GUILayout.Label("hasArriveToLocalPoint : " + hasArriveToLocalPoint);
            GUILayout.Label("Is waiting : " + isWating);
            GUILayout.Label("Delay : " + delay);
            GUILayout.Label("Random Range : " + randomRange);
            GUILayout.Label("Direction : " + currentOrientation);
        }
        */
    }

    #endregion

    #region Kill

    public void OnKilled()
    {
        //Debug.Log("DIE");
        anim.SetTrigger("Death");
        anim.SetBool("IsDead", true);
        agent.speed = 0;
        //GetComponentInChildren<SpriteRenderer>().color = deadColor;
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
        float dist = Vector2.Distance(transform.position, dog.transform.position);
        if (dist < /*localMaxMoveRange*/2000)
        {
            if (isDead) return;
            anim.SetTrigger("Death");
            anim.SetBool("IsDead", true);
            dogArea.enabled = false;
            //Debug.Log("Bones");
            agent.speed = 0;
            //GetComponentInChildren<SpriteRenderer>().color = Color.red;
            isDead = true;
            isBones = true;
            GameObject.FindGameObjectWithTag("SecondGoal").GetComponent<AIController>().DogTarget(this.transform.position);
            StartCoroutine(OnEaten());
        }
    }

    private IEnumerator OnEaten()
    {
        float dist = Vector2.Distance(transform.position, dog.transform.position);
        while (!IAMovement.NavmeshReachedDestination(dog.GetComponent<NavMeshAgent>(), transform.position, 1))
        {
            Debug.Log("too far");
            yield return null;
        }
        Debug.Log("eat");
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponentInChildren<SpriteRenderer>().color = Color.black;
        //DogEat();
        GameObject.FindGameObjectWithTag("SecondGoal").GetComponent<AIController>().dogTargetIsOn = false;
        dogArea.enabled = true;

        //TODO: Update point

        Destroy(gameObject);
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
        anim.SetBool("IsDead", false);
        //GetComponentInChildren<SpriteRenderer>().color = aliveColor;
        isDead = false;
        GetComponent<BoxCollider2D>().enabled = true;
    }

    #endregion

    #region Function Movement

    //Vérifie si L'IA atteint le point demandé

    public void UpdateNav()
    {
        if (IAMovement.NavmeshReachedDestination(currentEntity, zonePoint, rangePoint)|| eventAreaIsActive)
        {
            if (anim) anim.SetBool("isWalking", false);
            previousPoint = transform.position;
            randomRange = GetRandomRange();
            if (CompareTag("SecondGoal") && dogTargetIsOn) { zonePoint = nextZonePoint; return;}
            else if (areaColliderIsOn)
            {
                if (eventAreaIsActive)
                {
                   int colliderIndex =  Random.Range(0, currentArea.Count);
                   float x = Random.Range(currentArea[colliderIndex].bounds.min.x,currentArea[colliderIndex].bounds.max.x);
                   float y = Random.Range(currentArea[colliderIndex].bounds.min.y,currentArea[colliderIndex].bounds.max.y);
                   zonePoint = new Vector2(x, y);
                }
                else
                {
                    zonePoint = GameManager.RandomNavmeshLocation(randomRange, transform.position,
                        ref currentOrientation, currentArea);
                }
            }
            else
            {
                zonePoint = GameManager.RandomNavmeshLocation(randomRange, transform.position, ref currentOrientation);
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

    //Retourne un nombre aléatoire entre localMaxMoveRange et localMinMoveRange

    private float GetRandomRange()
    {
        return Random.Range(localMinMoveRange, localMaxMoveRange);
    }

    #endregion

    #region Zone Area Function

    public void GoToEvent(List<Collider2D> newCollider)
    {
        currentArea = newCollider;
        eventAreaIsActive = true;
    }

    public void SetDefaultArea()
    {
        currentArea = areaManager.areaColliders[areaIndex].zonesColliders;
        eventAreaIsActive = false;
    }

    public void SetAreaIndex(int index)
    {
        areaIndex = index;
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
        if (anim && canMove) anim.SetBool("isWalking", true);
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
            DownRight,
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