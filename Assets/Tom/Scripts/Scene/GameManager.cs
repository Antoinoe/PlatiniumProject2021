using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using DG.Tweening;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    #region Variables
    static GameManager _instance;

    [Header("Managers")]
    public Rewired.InputManager inputManager;

    public AudioManager audioManager;

    [Header("Players")]
    public Player[] players;
    public int playerNbrs = 1;

    [SerializeField] private int iAPerPlayer;

    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject iAPrefab;

    private int[] teams;
    [HideInInspector] public List<IAIdentity[]> iATeams;

    [Header("Smoke")]
    [SerializeField] private GameObject protoSmoke;
    [SerializeField] private float smokeDuration;

    //Camera Shake
    public event Action OnCameraShake;

    public int IAPerPlayer
    {
        get { return iAPerPlayer; }
        set { iAPerPlayer = value; }
    }

    #endregion

    private void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(gameObject);    // Suppression d'une instance précédente (sécurité...sécurité...)

        _instance = this;
    }

    public static GameManager GetInstance()
    {
        return _instance;
    }

    private void Start()
    {
        teams = new int[players.Length];
        iATeams = new List<IAIdentity[]>();

        Instantiate(inputManager);
        foreach (Player player in players)
        {
            #region Player
            //instancie joueur
            Vector2 newLocation = RandomNavmeshLocation(10, transform.position);
            GameObject newPlayer = GameObject.Instantiate(playerPrefab, new Vector3(newLocation.x, newLocation.y, 0), playerPrefab.transform.rotation);

            //team
            PlayerController newPlayerController = newPlayer.GetComponent<PlayerController>();
            newPlayerController.playerNb = player.playerNb;
            newPlayerController.teamNb = player.playerNb;
            teams[player.playerNb] = 1;

            //skin
            newPlayer.GetComponentInChildren<SpriteRenderer>().sprite = player.playerSprite;
            #endregion

            #region IA
            //Create team
            IAIdentity[] iATeam = new IAIdentity[iAPerPlayer];

            //Create each AIs
            for (int i = 0; i < iAPerPlayer; i++)
            {
                Vector2 initPos2 = RandomNavmeshLocation(10, transform.position);
                GameObject newIA = GameObject.Instantiate(iAPrefab, new Vector3(initPos2.x, initPos2.y, 0), iAPrefab.transform.rotation);

                IAIdentity iAIdentity = newIA.GetComponent<IAIdentity>();
                iAIdentity.teamNb = player.playerNb;
                iAIdentity.spriteRend.sprite = player.playerSprite;

                iATeam[i] = iAIdentity;
            }

            //Allocate team to player
            iATeams.Add(iATeam);
            #endregion
        }

        /*for (int i = 0; i < playerNbrs; i++)
        {
            Vector2 initPos = RandomNavmeshLocation(10, transform.position);
            GameObject newPlayer = GameObject.Instantiate(playerPrefab, new Vector3(initPos.x, initPos.y, 0), playerPrefab.transform.rotation);
            newPlayer.GetComponent<PlayerController>().teamNb = i;

            for (int i2 = 0; i2 < iAPerPlayer; i2++)
            {
                Vector2 initPos2 = RandomNavmeshLocation(10, transform.position);
                GameObject newIA = GameObject.Instantiate(iAPrefab, new Vector3(initPos2.x, initPos2.y, 0), iAPrefab.transform.rotation);
                newIA.GetComponent<IAIdentity>().teamNb = i;
            }
        }*/
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Application.Quit();
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            SpawnSmoke(Vector2.zero);
        }
    }

    public void OnValuesChanged(int _pNbr, int _iaPerPlayer)
    {
        playerNbrs = _pNbr;
        iAPerPlayer = _iaPerPlayer;
    }

    #region Win
    public void WinCheck(int curTeam, int targetTeam)
    {
        teams[curTeam] += -1;
        teams[targetTeam] += 1;

        if (targetTeam == players.Length)
        {
            Win(targetTeam);
        }
    }

    public void Win(int teamNb)
    {
        Debug.Log("Team " + teamNb + " win !");
    }
    #endregion

    #region Random NavMesh Location
    public static Vector2 RandomNavmeshLocation(float radius, Vector2 origin, AIController.CircleOrientation.Orientation navmeshOrientation)
    {
        List<int> allOrientations = new List<int>() { 0, 1, 2, 3, 0, 1, 2, 3 };
        List<int> tempList = allOrientations;
        for (int i = 0; i < allOrientations.Count; i++)
        {
            if (tempList[i] == (int)navmeshOrientation)
            {
                tempList.Remove(tempList[i]);
                break;
            }
        }
        int randomIndex = Random.Range(0, allOrientations.Count);
        navmeshOrientation = (AIController.CircleOrientation.Orientation)tempList[randomIndex];
        AIController.CircleOrientation iAOrientation = new AIController.CircleOrientation(navmeshOrientation);
        float angle = UnityEngine.Random.Range(iAOrientation.angleMin, iAOrientation.angleMax);
        Vector2 randomPosition = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;
        randomPosition += origin;
        NavMeshHit hit;
        Vector2 finalPosition = Vector2.zero;
        if (NavMesh.SamplePosition(randomPosition, out hit, radius, 1))
        {
            finalPosition = hit.position;
        }
        return finalPosition;
    }

    public static Vector2 RandomNavmeshLocation(float radius, Vector2 origin, AIController.CircleOrientation.Orientation navmeshOrientation, Bounds areaBounds)
    {
        List<int> allOrientations = new List<int>() { 0, 1, 2, 3, 0, 1, 2, 3 };
        List<int> tempList = allOrientations;
        for (int i = 0; i < allOrientations.Count; i++)
        {
            if (tempList[i] == (int)navmeshOrientation)
            {
                tempList.Remove(tempList[i]);
                break;
            }
        }
        int randomIndex = Random.Range(0, allOrientations.Count);
        navmeshOrientation = (AIController.CircleOrientation.Orientation)tempList[randomIndex];
        AIController.CircleOrientation iAOrientation = new AIController.CircleOrientation(navmeshOrientation);
        float angle = Random.Range(iAOrientation.angleMin, iAOrientation.angleMax);
        Vector2 randomPosition = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;
        randomPosition += origin;
        if (randomPosition.x > areaBounds.max.x || randomPosition.y > areaBounds.max.y
            || randomPosition.y < areaBounds.min.y || randomPosition.x < areaBounds.min.x)
        {
            Debug.Log("Random Point Reset");
            randomPosition = new Vector2(Random.Range(radius, areaBounds.max.x), Random.Range(radius, areaBounds.max.y));
        }
        NavMeshHit hit;
        Vector2 finalPosition = Vector2.zero;
        if (NavMesh.SamplePosition(randomPosition, out hit, radius, 1))
        {
            finalPosition = hit.position;
        }
        return finalPosition;
    }

    public static Vector2 RandomNavmeshLocation(float radius, Vector2 origin)
    {
        float angle = UnityEngine.Random.Range(-180, Mathf.PI);
        Vector2 randomPosition = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;
        randomPosition += origin;
        NavMeshHit hit;
        Vector2 finalPosition = Vector2.zero;
        if (NavMesh.SamplePosition(randomPosition, out hit, radius, 1))
        {
            finalPosition = hit.position;
        }
        return finalPosition;
    }

    public static Vector2 RandomNavmeshLocation(float radius, Vector2 origin, AIController.CircleOrientation.Orientation navmeshOrientation, List<Collider2D> areaColliders)
    {
        List<int> allOrientations = new List<int>() { 0, 1, 2, 3, 0, 1, 2, 3 };
        List<int> tempList = allOrientations;
        for (int i = 0; i < allOrientations.Count; i++)
        {
            if (tempList[i] == (int)navmeshOrientation)
            {
                tempList.Remove(tempList[i]);
                break;
            }
        }

        int randomIndex = Random.Range(0, allOrientations.Count);
        navmeshOrientation = (AIController.CircleOrientation.Orientation)tempList[randomIndex];
        AIController.CircleOrientation iAOrientation = new AIController.CircleOrientation(navmeshOrientation);
        float angle = Random.Range(iAOrientation.angleMin, iAOrientation.angleMax);
        Vector2 randomPosition = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;
        randomPosition += origin;
        bool inArea = false;
        for (int i = 0; i < areaColliders.Count; i++)
        {
            if (areaColliders[i].bounds.Contains(randomPosition))
            {
                inArea = true;
                break;
            }
        }

        if (!inArea)
        {
            Debug.Log("Random Point Reset");
            int randomArea = Random.Range(0, areaColliders.Count);
            if (areaColliders.Count > 0)
            {
                randomPosition = areaColliders[randomArea].ClosestPoint(randomPosition);
            }
            else
            {
                Debug.LogError("Liste de collider vide");
            }
        }
        NavMeshHit hit;
        Vector2 finalPosition = Vector2.zero;
        if (NavMesh.SamplePosition(randomPosition, out hit, radius, 1))
        {
            finalPosition = hit.position;
        }
        return finalPosition;
    }
    #endregion

    #region Shake
    public void Shake()
    {
        OnCameraShake();
    }
    #endregion

    #region Smoke
    public void SpawnSmoke(Vector2 position)
    {
        GameObject smoke = GameObject.Instantiate(protoSmoke, position, protoSmoke.transform.rotation);
        StartCoroutine(DespawnSmoke(smoke, smokeDuration));

        /*List<GameObject> targets = new List<GameObject>();

        Collider2D[] collidersInRange = Physics2D.OverlapBoxAll(transform.position, new Vector2(5, 5), 0f);
        foreach (Collider2D c in collidersInRange)
        {
            GameObject collidingObject = c.gameObject;

            if (collidingObject && collidingObject.CompareTag("Player") || collidingObject.CompareTag("NPC"))
            {
                targets.Add(collidingObject);
            }
        }*/
    }

    public void SpawnSmoke(Vector2 position, int playerNb)
    {
        GameObject smoke = GameObject.Instantiate(players[playerNb].smokeSystem, position, protoSmoke.transform.rotation);
        StartCoroutine(DespawnSmoke(smoke, smokeDuration));

        /*List<GameObject> targets = new List<GameObject>();

        Collider2D[] collidersInRange = Physics2D.OverlapBoxAll(transform.position, new Vector2(5, 5), 0f);
        foreach (Collider2D c in collidersInRange)
        {
            GameObject collidingObject = c.gameObject;

            if (collidingObject && collidingObject.CompareTag("Player") || collidingObject.CompareTag("NPC"))
            {
                targets.Add(collidingObject);
            }
        }*/
    }

    private IEnumerator DespawnSmoke(GameObject smoke, float timer)
    {
        yield return new WaitForSeconds(timer);
        GameObject.Destroy(smoke);
        StopCoroutine(DespawnSmoke(smoke, timer));
    }
    #endregion
}
