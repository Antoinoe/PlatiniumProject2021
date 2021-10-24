using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{
    #region Variables
    static GameManager _instance;

    public Rewired.InputManager inputManager;

    public AudioManager audioManager;

    public Player[] players;
    public int playerNbrs = 1;

    [SerializeField] private GameObject playerPrefab;

    private List<int> teams;
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
            GameObject newPlayer = GameObject.Instantiate(playerPrefab, RandomNavmeshLocation(10), playerPrefab.transform.rotation);
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

    public Vector3 RandomNavmeshLocation(float radius)
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += transform.position;
        NavMeshHit hit;
        Vector3 finalPosition = Vector3.zero;
        if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1))
        {
            finalPosition = hit.position;
        }
        return finalPosition;
    }
}
