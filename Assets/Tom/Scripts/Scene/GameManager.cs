using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using DG.Tweening;
using UnityEngine.AI;
using Random = UnityEngine.Random;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[System.Serializable]
public struct Accelerator
{
    public int delayBeforeAccel;
    public Vector2 minMax;
}


public class GameManager : MonoBehaviour
{
    #region Variables
    static GameManager _instance;

    [Header("___DEBUG___")]
    public bool isDebug;

    [Space]
    [Space]
    [Space]

    public GameObject winPanel;
    public GameObject uis;
    [HideInInspector]
    public int[] playerList;
    int listIndex;
    public GameObject pause;
    public bool isPause = false;

    [HideInInspector]
    public bool isWin = false;

    [Header("Managers")]
    public Rewired.InputManager inputManager;

    public AudioManager audioManager;

    [Header("Players")]
    public Player[] players;
    public int playerNbrs = 1;
    public GameObject[] playersOnBoard;
    [HideInInspector]
    public int[] pSprite;

    public List<Accelerator> accelerations = new List<Accelerator>();
    Accelerator currAccel;
    int currAccelIndex = 0;
    float currTimer;

    [SerializeField] private int iAPerPlayer;

    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject iAPrefab;


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
    public event Action OnCameraShake;

    [Header("Bushes")]
    public Sprite invisibleSprite;

    [Header("UI")]
    [SerializeField] private Image victorPlayerIcon;
    [SerializeField] private Sprite[] playersIcon;

    [SerializeField] private GameObject conversionVictoryText;
    [SerializeField] private GameObject houndVictoryText;

    public int IAPerPlayer
    {
        get { return iAPerPlayer; }
        set { iAPerPlayer = value; }
    }

    #endregion

    private void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(gameObject);    // Suppression d'une instance pr�c�dente (s�curit�...s�curit�...)

        _instance = this;
        StartCoroutine(CooldownForEvent());
    }

    public static GameManager GetInstance()
    {
        return _instance;
    }

    private void Start()
    {
        isDebug = Data.isDebug;
        FindObjectOfType<AudioManager>().Play("Music");
        if (!isDebug)
            playerNbrs = Data.playerNbr;
        teams = new int[players.Length];
        iATeams = new List<IAIdentity[]>();
        playersOnBoard = new GameObject[playerNbrs];
        Instantiate(inputManager);
        //Debug.Log(playerNbrs);
        playerList = new int[playerNbrs];
        for (int i = 0; i < playerNbrs; i++)
        {
            playerList[i] = i;
        }
        listIndex = playerList.Length;
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
            if (!isDebug)
            {
                newPlayerController.playerNb = Data.pSprite[i];
                newPlayerController.teamNb = Data.pSprite[i];
            }
            else
            {
                newPlayerController.playerNb = i;
                newPlayerController.teamNb = i;
            }
            newPlayerController.contNbr = i;
            teams[i] = i;
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
                iAIdentity.controllerIdentity = newIA.GetComponent<AIController>();
                if (!isDebug)
                {
                    iAIdentity.teamNb = Data.pSprite[i];
                }
                else
                {
                    iAIdentity.teamNb = i;
                }

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
        currTimer -= Time.deltaTime;
        if (currTimer < 0)
        {
            foreach (AIController ai in FindObjectsOfType<AIController>())
            {
                ai.delayMin = accelerations[currAccelIndex].minMax.x;
                ai.delayMax = accelerations[currAccelIndex].minMax.y;
            }
            if (currAccelIndex + 1 < accelerations.Count)
            {
                currAccelIndex++;
                currTimer = accelerations[currAccelIndex].delayBeforeAccel;
            }
        }
    }
    public void Pause()
    {
        isPause = !isPause;
        pause.SetActive(isPause);
        if (isPause)
        {
            //foreach (GameObject player in playersOnBoard)
            //{

            //}

            /*NavMeshAgent[] ias = FindObjectsOfType<NavMeshAgent>();
            foreach (NavMeshAgent ia in ias)
                ia.enabled = false;*/
            Time.timeScale = 0;
            print("pausing/unpausing");
            EventSystem.current.SetSelectedGameObject(pause.transform.GetChild(0).transform.GetChild(2).transform.GetChild(0).gameObject);

        }
        else
        {
            /*AIController[] ias = FindObjectsOfType<AIController>();
            foreach (AIController ia in ias)
                ia.gameObject.GetComponent<NavMeshAgent>().speed = ia.Speed;*/

            /*NavMeshAgent[] ias = FindObjectsOfType<NavMeshAgent>();
            foreach (NavMeshAgent ia in ias)
                ia.enabled = true;*/
            Time.timeScale = 1;
            EventSystem.current.SetSelectedGameObject(null);
            print("pausing/unpausing");
        }
    }

    public void OnValuesChanged(int _pNbr, int _iaPerPlayer)
    {
        playerNbrs = _pNbr;
        iAPerPlayer = _iaPerPlayer;
        Data.playerNbr = _pNbr;
    }

    #region Win
    public void WinCheck(int curTeam, int targetTeam, int contNbr)
    {
        /*Debug.Log(targetTeam + " " + playerList.Length);
        playerList[contNbr] = 999;
        listIndex--;
        if (listIndex == 1)
        {
            string str = "";
            for (int i = 0; i < playerList.Length; i++)
            {
                str += i + " : " + playerList[i] + " , ";
            }
            Debug.Log(str);
            for (int i = 0; i < playerList.Length; i++)
            {
                //if (playerList[i] != 999) Win(targetTeam);
            }
        }*/
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
        //winPanel.transform.GetChild(0).GetComponent<Text>().text = "Player " + (teamNb + 1) + " win!";
        float timer = 1.0f;
        while (timer > 0)
            timer -= Time.deltaTime;
        winPanel.SetActive(true);
        FindObjectOfType<AudioManager>().Stop("Music");
        FindObjectOfType<AudioManager>().Play("Victory");
        victorPlayerIcon.sprite = players[teamNb].UISprite;
        conversionVictoryText.SetActive(true);
        uis.SetActive(false);
        isWin = true;
        FindObjectOfType<WinNav>().Init(teamNb);
        Debug.Log("________Team " + teamNb + " win !");
        //Camera.main.transform.DOMoveX(playersOnBoard[teamNb].transform.position.x, 3);
        //Camera.main.transform.DOMoveY(playersOnBoard[teamNb].transform.position.y, 3);
    }

    public void WinWithSecondObjective(int teamNb)
    {
        FindObjectOfType<AudioManager>().Stop("Music");
        FindObjectOfType<AudioManager>().Play("Victory");
        winPanel.SetActive(true);
        victorPlayerIcon.sprite = players[teamNb].UISprite;
        houndVictoryText.SetActive(true);
        uis.SetActive(false);
        isWin = true;
        FindObjectOfType<WinNav>().Init(teamNb);
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



    #region IA AreaZone

    public IEnumerator MoveAllAItoZone()
    {
        //Debug.Log("Entrer dans la zone");
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
        //Debug.Log("Sortie de zone");
        for (int i = 0; i < iATeams.Count; i++)
        {
            IAIdentity[] iATeam = iATeams[i];
            for (int j = 0; j < iATeam.Length; j++)
            {
                iATeam[j].controllerIdentity.SetDefaultArea();
            }
        }
    }

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
            //Debug.Log("fin du cooldown");
            //StartCoroutine(MoveAllAItoZone());
        }
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