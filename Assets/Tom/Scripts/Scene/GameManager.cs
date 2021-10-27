using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using DG.Tweening;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{
    #region Variables
    static GameManager _instance;

    public Rewired.InputManager inputManager;

    public AudioManager audioManager;

    public Player[] players;
    public int playerNbrs = 1;

    [SerializeField] private int iAPerPlayer;

    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject iAPrefab;

    private List<int> teams;

    //Camera shake
    [SerializeField] private float shakeDur;
    [SerializeField] private float shakeStrenght;
    [SerializeField] private int shakeVibrato;
    [SerializeField] private int shakeRandomness;
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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("shake your booty !!!");
            Shake();
        }
    }

    private void Start()
    {
        Instantiate(inputManager);
        /*foreach (Player player in players)
        {
            //instancie joueur
            GameObject newPlayer = GameObject.Instantiate(playerPrefab, RandomNavmeshLocation(100), playerPrefab.transform.rotation);

            //team
            newPlayer.GetComponent<PlayerController>().teamNb = player.playerNb;
            teams[player.playerNb] = 1;

            //skin
            newPlayer.GetComponentInChildren<SpriteRenderer>().sprite = player.playerSprite;

            //effects
            if (player.smokeSystem)
            {
                GameObject.Instantiate(player.smokeSystem, newPlayer.transform);
            }
        }*/

        for (int i = 0; i < playerNbrs; i++)
        {
            Vector3 initPos = RandomNavmeshLocation(10, transform.position);
            GameObject newPlayer = GameObject.Instantiate(playerPrefab, new Vector3(initPos.x, initPos.y, 0), playerPrefab.transform.rotation);
            newPlayer.GetComponent<PlayerController>().teamNb = i;

            for (int i2 = 0; i2 < iAPerPlayer; i2++)
            {
                Vector3 initPos2 = RandomNavmeshLocation(10, transform.position);
                GameObject newIA = GameObject.Instantiate(iAPrefab, new Vector3(initPos2.x, initPos2.y, 0), iAPrefab.transform.rotation);
                newIA.GetComponent<IAIdentity>().teamNb = i;
            }
        }
    }

    public void ChangeTeam(int curTeam, int targetTeam)
    {
        teams[curTeam] += -1;
        teams[targetTeam] += 1;

        if (targetTeam == players.Length)
        {
            Win(targetTeam);
        }
    }

    public void Win(int playerNb)
    {
        Debug.Log("Player " + playerNb + " win !");
    }

    public static Vector3 RandomNavmeshLocation(float radius, Vector3 origin)
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += origin;
        NavMeshHit hit;
        Vector3 finalPosition = Vector3.zero;
        if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1))
        {
            finalPosition = hit.position;
        }
        return finalPosition;
    }

    public void Shake()
    {
        Camera.main.DOShakePosition(shakeDur, shakeStrenght, shakeVibrato, shakeRandomness);
    }

    public void Shake(float duration, float strength, int vibrato, int randomness)
    {
        Camera.main.DOShakePosition(duration, strength, vibrato, randomness);
    }
}
