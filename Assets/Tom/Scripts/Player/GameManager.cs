using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Variables
    public AudioManager aM;

    public Player[] players;

    [SerializeField] private GameObject playerPrefab;

    private void Start()
    {
        foreach (Player player in players)
        {
            //séléctionnner un point avec le navMesh
            GameObject newPlayer = GameObject.Instantiate(playerPrefab/*, position, rotation, parent*/);
            newPlayer.GetComponent<Attack>().teamNb = player.playerNb;
            newPlayer.GetComponent<SpriteRenderer>().sprite = player.playerSprite;
            GameObject.Instantiate(player.smokeSystem, newPlayer.transform);
        }
    }
    #endregion
}
