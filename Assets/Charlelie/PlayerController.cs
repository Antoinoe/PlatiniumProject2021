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

    private SpriteRenderer sprite;

    void Start()
    {
        controller = GetComponent<Controller>();
        controller.SetNum(teamNb);
        gameManager = GameManager.GetInstance();
        sprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }


    void Update()
    {
        sprite.sortingOrder = Mathf.RoundToInt(transform.position.y * -10f);
    }

    public void ChangeTeam(int nb)
    {
        Debug.Log("Team " + nb + " assimilated a player from Team " + teamNb);

        GetComponentInChildren<SpriteRenderer>().sprite = gameManager.players[nb].playerSprite;
        gameManager.WinCheck(teamNb, nb);

        teamNb = nb;
    }

    public void OnDieReset()
    {
        GetComponent<Attack>().bounty = 0;
    }
}
