using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using DG.Tweening;
using UnityEngine.AI;
using Random = UnityEngine.Random;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region Variables
    static GameManager _instance;

    public GameObject winPanel;

    [Header("Managers")]
    public Rewired.InputManager inputManager;

    public AudioManager audioManager;

    [Header("Players")]
    public Player[] players;
    public int playerNbrs = 1;
    public GameObject[] playersOnBoard;

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

    [Header("Bushes")]
    public Sprite invisibleSprite;

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
        for (int i = 0; i < ReInput.players.allPlayerCount - 1; i++)
        {
            //Debug.Log(ReInput.players.GetPlayer(i).id);
        }
        teams = new int[players.Length];
        iATeams = new List<IAIdentity[]>();
        playersOnBoard = new GameObject[playerNbrs];
        Instantiate(inputManager);
        for (int i = 0; i < playerNbrs; i++)
        {
            #region Player
            //instancie joueur
            /*Vector2 newLocation = RandomNavmeshLocation(10, transform.position);*/
            Vector2 newLocation = RandomRectangleLocation();
            GameObject newPlayer = GameObject.Instantiate(playerPrefab, new Vector3(newLocation.x, newLocation.y, 0), playerPrefab.transform.rotation);
            playersOnBoard[i] = newPlayer;
            //team
            PlayerController newPlayerController = newPlayer.GetComponent<PlayerController>();
            newPlayerController.playerNb = i;
            newPlayerController.teamNb = i;
            teams[i] = i;

            //skin
            newPlayer.GetComponentInChildren<SpriteRenderer>().sprite = players[i].playerSprite;
            #endregion

            #region IA
            //Create team
            IAIdentity[] iATeam = new IAIdentity[iAPerPlayer];

            //Create each AIs
            for (int j = 0; j < iAPerPlayer; j++)
            {
                /*Vector2 initPos2 = RandomNavmeshLocation(10, transform.position);*/
                Vector2 initPos2 = RandomRectangleLocation();
                GameObject newIA = GameObject.Instantiate(iAPrefab, new Vector3(initPos2.x, initPos2.y, 0), Quaternion.identity);
                //newIA.transform.rotation = new Quaternion(0, 0, 0, 0);
                IAIdentity iAIdentity = newIA.GetComponent<IAIdentity>();
                iAIdentity.teamNb = i;
                iAIdentity.spriteRend.sprite = players[i].playerSprite;

                iATeam[j] = iAIdentity;
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
        if (teams[targetTeam] == playerNbrs - 1)
        {
            Win(targetTeam);
        }
    }

    public void Win(int teamNb)
    {
        //winPanel.transform.GetChild(0).GetComponent<Text>().text = "Player " + (teamNb + 1) + " win!";
        //winPanel.SetActive(true);
        Debug.Log("Team " + teamNb + " win !");
        //Camera.main.transform.DOMoveX(playersOnBoard[teamNb].transform.position.x, 3);
        //Camera.main.transform.DOMoveY(playersOnBoard[teamNb].transform.position.y, 3);
    }
    #endregion

    public void LoadScene(string sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }

    #region Random NavMesh Location
    public static Vector2 RandomNavmeshLocation(float radius, Vector2 origin, ref AIController.CircleOrientation.Orientation navmeshOrientation)
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
        NavMeshHit hit;
        Vector2 finalPosition = Vector2.zero;
        if (NavMesh.SamplePosition(randomPosition, out hit, radius, 1))
        {
            finalPosition = hit.position;
        }
        return finalPosition;
    }

    public static Vector2 RandomNavmeshLocation(float radius, Vector2 origin, ref AIController.CircleOrientation.Orientation navmeshOrientation, List<Collider2D> areaColliders)
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

        AIController.CircleOrientation.Orientation oldOrientation = navmeshOrientation;
        int randomIndex = Random.Range(0, tempList.Count);
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
            //Debug.Log("Random Point Reset");
            if (areaColliders.Count > 0)
            {
                Vector2 newPos;
                Debug.Log("ReturnToTheMiddle is false");
                    //tempList.Remove(randomIndex);
                    //randomIndex = Random.Range(0, tempList.Count);
                    //navmeshOrientation = (AIController.CircleOrientation.Orientation)tempList[randomIndex];
                    
                    //iAOrientation = new AIController.CircleOrientation(oldOrientation);
                    //angle = Random.Range(iAOrientation.angleMin, iAOrientation.angleMax);
                    //int randomArea = Random.Range(0, areaColliders.Count);
                    //newPos = Physics2D.ClosestPoint(, areaColliders[randomArea]);

                    newPos = origin - randomPosition;
                    randomPosition = -newPos;
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

    #region Random Rectangle Location
    public Vector2 RandomRectangleLocation()
    {
        Bounds rectangleBounds = gameObject.GetComponent<BoxCollider2D>().bounds;
        Vector2 rectangleSize = rectangleBounds.size;
        float xSize = 0.5f * rectangleSize.x;
        float ySize = 0.5f * rectangleSize.y;

        Vector2 newLocation = new Vector2(Random.Range(-xSize, xSize), Random.Range(-ySize, ySize));
        newLocation += (Vector2)rectangleBounds.center;

        return newLocation;
    }
    #endregion
}