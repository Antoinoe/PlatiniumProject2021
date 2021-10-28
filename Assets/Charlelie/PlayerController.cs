using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{
    Controller controller;

    //Teams
    public int teamNb = 0;

    public GameManager gameManager;

    void Start()
    {
        controller = GetComponent<Controller>();
        controller.SetNum(teamNb);
        gameManager = GameManager.GetInstance();
        //Set trigger script
        AttackZone zoneScript = GetComponentInChildren<AttackZone>();
        zoneScript.playerScript = this;
    }


    void Update()
    {
        
    }

    public void ChangeTeam(int nb)
    {
        Debug.Log("Team " + nb + " assimilated a player from Team " + teamNb);

        GetComponent<SpriteRenderer>().sprite = gameManager.players[nb].playerSprite;
        gameManager.WinCheck(teamNb, nb);

        teamNb = nb;
    }

    public void OnDieReset()
    {
        GetComponent<Attack>().bounty = 0;
    }
}
