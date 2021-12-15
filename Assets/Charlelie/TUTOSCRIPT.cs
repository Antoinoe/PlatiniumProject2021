using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using DG.Tweening;
using UnityEngine.AI;
using Random = UnityEngine.Random;
using UnityEngine.UI;

public class TUTOSCRIPT : MonoBehaviour
{
    #region Variables
    static TUTOSCRIPT _instance;


    //public GameObject winPanel;
    //public GameObject uis;
    [HideInInspector]
    public int[] playerList;
    //public GameObject pause;
    //public bool isPause = false;

    [HideInInspector]
    public bool isWin = false;

    [Header("Managers")]
    public Rewired.InputManager inputManager;

    //public AudioManager audioManager;

    [Header("Players")]
    public Player[] players;
    public int playerNbrs = 1;
    public GameObject[] playersOnBoard;
    [HideInInspector]
    public int[] pSprite;

    //public List<Accelerator> accelerations = new List<Accelerator>();
    //Accelerator currAccel;
    //int currAccelIndex = 0;
    //float currTimer;

    //[SerializeField] private int iAPerPlayer;

    [SerializeField] private GameObject playerPrefab;
    //[SerializeField] private GameObject iAPrefab;


    private int[] teams;
    [HideInInspector] public List<IAIdentity[]> iATeams;

    [Header("Area Event")]
    public AreaManager eventArea;
    public float eventCooldown = 1f;
    public float timeToEvent = 10f;
    private int eventIndex;

    [Header("Smoke")]
    [SerializeField] private GameObject protoSmoke;
    [SerializeField] private float smokeDuration;

    //Camera Shake
    //public event Action OnCameraShake;

    [Header("Bushes")]
    public Sprite invisibleSprite;

    //public int IAPerPlayer
    //{
    //    get { return iAPerPlayer; }
    //    set { iAPerPlayer = value; }
    //}

    #endregion

    private void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(gameObject);    // Suppression d'une instance pr�c�dente (s�curit�...s�curit�...)

        _instance = this;
        StartCoroutine(CooldownForEvent());
    }

    public static TUTOSCRIPT GetInstance()
    {
        return _instance;
    }

    private void Start()
    {
        //isDebug = Data.isDebug;
        //FindObjectOfType<AudioManager>().Play("Music");
        //if (!isDebug)
        playerNbrs = Data.playerNbr;
        teams = new int[players.Length];
        //iATeams = new List<IAIdentity[]>();
        playersOnBoard = new GameObject[playerNbrs];
        //Instantiate(inputManager);
        //Debug.Log(playerNbrs);
        playerList = new int[playerNbrs];
        for (int i = 0; i < playerNbrs; i++)
        {
            playerList[i] = i;
        }
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
            //if (!isDebug)
           // {
            newPlayerController.playerNb = Data.pSprite[i];
            newPlayerController.teamNb = Data.pSprite[i];
            //}
            //else
            //{
            //    newPlayerController.playerNb = i;
            //    newPlayerController.teamNb = i;
            //}
            newPlayerController.contNbr = i;
            teams[i] = i;
            #endregion

            #region IA
            //Create team
            //IAIdentity[] iATeam = new IAIdentity[iAPerPlayer];

            //Create each AIs
            //for (int j = 0; j < iAPerPlayer; j++)
            //{
            //    /*Vector2 initPos2 = RandomNavmeshLocation(10, transform.position);*/
            //    Vector2 initPos2 = RandomRectangleLocation();
            //    GameObject newIA = GameObject.Instantiate(iAPrefab, new Vector3(initPos2.x, initPos2.y, 0), Quaternion.identity);
            //    //newIA.transform.rotation = new Quaternion(0, 0, 0, 0);
            //    IAIdentity iAIdentity = newIA.GetComponent<IAIdentity>();
            //    iAIdentity.controllerIdentity = newIA.GetComponent<AIController>();
            //    if (!isDebug)
            //    {
            //        iAIdentity.teamNb = Data.pSprite[i];
            //    }
            //    else
            //    {
            //        iAIdentity.teamNb = i;
            //    }
            //
            //    iAIdentity.spriteRend.sprite = players[i].playerSprite;
            //
            //    iATeam[j] = iAIdentity;
            //}

            //Allocate team to player
            //iATeams.Add(iATeam);
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
        //currTimer -= Time.deltaTime;
        //if (currTimer < 0)
        //{
        //    foreach (AIController ai in FindObjectsOfType<AIController>())
        //    {
        //        ai.delayMin = accelerations[currAccelIndex].minMax.x;
        //        ai.delayMax = accelerations[currAccelIndex].minMax.y;
        //    }
        //    if (currAccelIndex + 1 < accelerations.Count)
        //    {
        //        currAccelIndex++;
        //        currTimer = accelerations[currAccelIndex].delayBeforeAccel;
        //    }
        //}
        for (int i = 0; i < ReInput.controllers.joystickCount; i++)
        {
            if (ReInput.players.Players[i].GetButtonDown("Pause"))
                UnityEngine.SceneManagement.SceneManager.LoadScene("DevTest");
        }
    }
    //public void Pause()
    //{
    //    isPause = !isPause;
    //    pause.SetActive(isPause);
    //    if (isPause)
    //    {
    //        //foreach (GameObject player in playersOnBoard)
    //        //{
    //
    //        //}
    //
    //        /*NavMeshAgent[] ias = FindObjectsOfType<NavMeshAgent>();
    //        foreach (NavMeshAgent ia in ias)
    //            ia.enabled = false;*/
    //        Time.timeScale = 0;
    //
    //    }
    //    else
    //    {
    //        /*AIController[] ias = FindObjectsOfType<AIController>();
    //        foreach (AIController ia in ias)
    //            ia.gameObject.GetComponent<NavMeshAgent>().speed = ia.Speed;*/
    //
    //        /*NavMeshAgent[] ias = FindObjectsOfType<NavMeshAgent>();
    //        foreach (NavMeshAgent ia in ias)
    //            ia.enabled = true;*/
    //        Time.timeScale = 1;
    //    }
    //}

    //public void OnValuesChanged(int _pNbr, int _iaPerPlayer)
    //{
    //    playerNbrs = _pNbr;
    //    iAPerPlayer = _iaPerPlayer;
    //    Data.playerNbr = _pNbr;
    //}

    #region Win
    public void WinCheck(int curTeam, int targetTeam)
    {
        PlayerController[] players = FindObjectsOfType<PlayerController>();
        int team = 0;
        Debug.Log("CURRTEAM : " + targetTeam);
        foreach (PlayerController player in players)
        {
            team = player.teamNb;
            Debug.Log(team);
            if (team != targetTeam) return;
        }
        Win(targetTeam);
    }

    public void Win(int teamNb)
    {
        float timer = 2.0f;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        UnityEngine.SceneManagement.SceneManager.LoadScene("TUTO");
    }
    #endregion

    public void LoadScene(string sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }

    public void ReloadScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    #region Random NavMesh Location
    public static Vector2 RandomNavmeshLocation(float radius, Vector2 posOrigin, ref AIController.CircleOrientation.Orientation navmeshOrientation)
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
        randomPosition += posOrigin;
        NavMeshHit hit;
        Vector2 finalPosition = Vector2.zero;
        if (NavMesh.SamplePosition(randomPosition, out hit, radius, 1))
        {
            finalPosition = hit.position;
        }
        return finalPosition;
    }

    /*
    public IEnumerator MoveAllAItoZone()
    {
        Debug.Log("Entrer dans la zone");
        List<Collider2D> area = eventArea.areaColliders[eventIndex].zonesColliders;
        for (int i = 0; i < iATeams.Count; i++)
        {
            IAIdentity[] iATeam = iATeams[i];
            for (int j = 0; j < iATeam.Length; j++)
            {
                iATeam[j].controllerIdentity.GoToEvent(area);
            }
        }
        yield return new WaitForSeconds(timeToEvent);
        Debug.Log("Sortie de zone");
        for (int i = 0; i < iATeams.Count; i++)
        {
            IAIdentity[] iATeam = iATeams[i];
            for (int j = 0; j < iATeam.Length; j++)
            {
                iATeam[j].controllerIdentity.SetDefaultArea();
            }
        }
    }
    */

    private IEnumerator CooldownForEvent()
    {
        while (true)
        {
            if (eventCooldown > timeToEvent)
            {
                yield return new WaitForSeconds(eventCooldown);
            }
            else
            {
                yield return new WaitForSeconds(eventCooldown + timeToEvent);
            }
            Debug.Log("fin du cooldown");
            //StartCoroutine(MoveAllAItoZone());
        }
    }

    public static Vector2 RandomNavmeshLocation(float radius, Vector2 posOrigin, ref AIController.CircleOrientation.Orientation navmeshOrientation, List<Collider2D> areaColliders)
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
        int randomIndex = Random.Range(0, tempList.Count);
        navmeshOrientation = (AIController.CircleOrientation.Orientation)tempList[randomIndex];
        AIController.CircleOrientation iAOrientation = new AIController.CircleOrientation(navmeshOrientation);
        float angle = Random.Range(iAOrientation.angleMin, iAOrientation.angleMax);
        Vector2 randomPosition = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;
        randomPosition += posOrigin;
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
            Vector2 oldRandomPosition = randomPosition;
            Debug.DrawLine(posOrigin, randomPosition, Color.cyan, 5f);
            //Debug.Log("Random Point Reset, Old pos :" + oldRandomPosition);
            //Debug.Log(Vector2.Distance(posOrigin, randomPosition));
            if (areaColliders.Count > 0)
            {
                Vector2 newDir;
                //tempList.Remove(randomIndex);
                //randomIndex = Random.Range(0, tempList.Count);
                //navmeshOrientation = (AIController.CircleOrientation.Orientation)tempList[randomIndex];

                //iAOrientation = new AIController.CircleOrientation(oldOrientation);
                //angle = Random.Range(iAOrientation.angleMin, iAOrientation.angleMax);
                //int randomArea = Random.Range(0, areaColliders.Count);
                //newPos = Physics2D.ClosestPoint(, areaColliders[randomArea]);

                newDir = randomPosition - posOrigin;
                Vector2 newPos = posOrigin - newDir;
                randomPosition = newPos;
                //randomPosition = new Vector2(Mathf.Cos(-angle), Mathf.Sin(-angle)) / radius;
                //Debug.Log("Random Point Reset, New pos :" + randomPosition);
                //Debug.Log(Vector2.Distance(posOrigin, randomPosition));
                Debug.DrawLine(posOrigin, randomPosition, Color.yellow, 5f);
                //Debug.Log("Random Point Reset, newZ pos :" + randomPosition);
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
        //OnCameraShake();
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