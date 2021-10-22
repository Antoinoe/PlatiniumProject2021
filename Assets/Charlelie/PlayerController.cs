using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{
    public Player player;
    //Teams
    public int teamNb = 0;

    public GameManager gameManager;

    void Start()
    {
        //Set trigger script
        AttackZone zoneScript = GetComponentInChildren<AttackZone>();
        zoneScript.playerScript = this;
    }


    void Update()
    {
        
    }

    public void ChangeTeam(int nb)
    {
        teamNb = nb;

        //Player p = Array.Find(gameManager.players, player => player.playerNb == nb);

        /*if (p == null)
            return;

        GetComponent<SpriteRenderer>().sprite = p.playerSprite;*/
    }

    public void OnDieReset()
    {
        GetComponent<Attack>().bounty = 0;
    }
}
