using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class GameManager : MonoBehaviour
{
    #region Variables
    static GameManager _instance;

    public Rewired.InputManager inputManager;

    public AudioManager audioManager;

    //public Player[] players;
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
            //séléctionnner un point avec le navMesh
            GameObject newPlayer = GameObject.Instantiate(playerPrefab);

            //team
            newPlayer.GetComponent<PlayerController>().teamNb = player.playerNb;
            teams[player.playerNb] = 1;

            //skin
            newPlayer.GetComponent<SpriteRenderer>().sprite = player.playerSprite;

            //effects
            GameObject.Instantiate(player.smokeSystem, newPlayer.transform);
        }*/

        for (int i = 0; i < playerNbrs; i++)
        {
            GameObject newPlayer = GameObject.Instantiate(playerPrefab);
        }
    }

    public void ChangeTeam(int curTeam, int targetTeam)
    {
        teams[curTeam] += -1;
        teams[targetTeam] += 1;
    }
}
