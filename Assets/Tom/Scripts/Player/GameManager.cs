using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Variables
    public AudioManager aM;

    public Player[] players;

    [SerializeField] private GameObject playerPrefab;

    private List<int> teams;
    #endregion
    private void Start()
    {
        foreach (Player player in players)
        {
            //séléctionnner un point avec le navMesh
            GameObject newPlayer = GameObject.Instantiate(playerPrefab/*, position, rotation, parent*/);

            //team
            newPlayer.GetComponent<Attack>().teamNb = player.playerNb;
            teams[player.playerNb] = 1;

            //skin
            newPlayer.GetComponent<SpriteRenderer>().sprite = player.playerSprite;

            //effects
            GameObject.Instantiate(player.smokeSystem, newPlayer.transform);
        }
    }

    public void ChangeTeam(int curTeam, int targetTeam)
    {
        teams[curTeam] += -1;
        teams[targetTeam] += 1;
    }
}
