using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{
    Controller controller;

    //Teams
    public int teamNb = 0;
    public int playerNb = 0;

    public GameManager gameManager;

    private SpriteRenderer spriteRend;

    void Start()
    {
        controller = GetComponent<Controller>();
        controller.SetNum(playerNb);
        gameManager = GameManager.GetInstance();
        spriteRend = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }


    void Update()
    {
        spriteRend.sortingOrder = Mathf.RoundToInt(transform.position.y * -10f);
    }

    public void ChangeTeam(int nb)
    {
        #region Change player team
        Debug.Log("Team " + nb + " assimilated player " + playerNb);

        Sprite newSprite = gameManager.players[nb].playerSprite;
        spriteRend.sprite = newSprite;
        gameManager.WinCheck(teamNb, nb);

        teamNb = nb;
        #endregion

        #region Change AIs team
        foreach (IAIdentity iA in gameManager.iATeams[playerNb])
        {
            iA.teamNb = teamNb;
            iA.spriteRend.sprite = newSprite; 
        }
        #endregion
    }

    public void OnDieReset()
    {
        //GetComponent<Attack>().bounty = 0;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere((Vector2)transform.position + GetComponent<Controller>().MovementVector, 1);
    }
}
