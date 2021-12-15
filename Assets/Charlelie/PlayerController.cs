using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PlayerController : MonoBehaviour
{
    Controller controller;

    //Teams
    public int teamNb = 0;
    public int playerNb = 0;
    public int contNbr = 0;
    public Sprite[] bases,fillers; //liste des sprites dans l'odre de selection des joueurs dans le menu charac
    float killCooldown = 1.0f;
    float killIAaddCooldown = 1.0f;
    float currKillCooldown = 0.0f;
    public bool isInvincible = false;
    float invicibilityTime = 0.5f;
    float currInvTime;
    [SerializeField]
    private GameObject assignedUI; // A quel UI appartient ce joueur

    public Image actualBaseSprite, actualFillerSprite; // base = tête de mort | filler = jauge (cercle de couleur unie)

    public float KillCooldown
    {
        get { return killCooldown; }
        set { killCooldown = value; }
    }

    public float KillIAaddCooldown
    {
        get { return killIAaddCooldown; }
        set { killIAaddCooldown = value; }
    }

    public float CurrKillCooldown
    {
        get { return currKillCooldown; }
        set { currKillCooldown = value; }
    }

    public GameManager gameManager;

    private SpriteRenderer spriteRend;

    //bushes
    private int invisible = 0;

    void Start()
    {
        controller = GetComponent<Controller>();
        controller.SetNum(playerNb, contNbr);
        gameManager = GameManager.GetInstance();
        if (!FindObjectOfType<TUTOSCRIPT>())
        {
            assignedUI = FindObjectOfType<UIManager>().uiImages[contNbr].gameObject;
            //print(assignedUI.transform.parent);
            actualBaseSprite = assignedUI.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>();
            actualFillerSprite = assignedUI.transform.GetChild(0).GetComponent<Image>();

            actualBaseSprite.sprite = bases[playerNb];
            actualFillerSprite.sprite = fillers[playerNb];
        }
            

        spriteRend = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }


    void Update()
    {
        spriteRend.sortingOrder = Mathf.RoundToInt(transform.position.y * -10f);
        if (isInvincible)
            currInvTime -= Time.deltaTime;
        if (currInvTime <= 0) isInvincible = false;
    }

    public void OnKill(bool isNPC)
    {
        if (isNPC)
            currKillCooldown = killCooldown + killIAaddCooldown;
        else
            currKillCooldown = killCooldown;
    }

    public void SetInv()
    {
        currInvTime = invicibilityTime;
        isInvincible = true;
    }

    public void changeUI(int nb)
    {
        #region Change player UI
       
        #endregion
    }
    public void ChangeTeam(int nb, int cont)
    {
        #region Change player team
        //Debug.Log("Team " + nb + " assimilated player " + playerNb);
        Sprite newSprite;
        if (!FindObjectOfType<TUTOSCRIPT>())
            newSprite = gameManager.players[nb].playerSprite;
        else
            newSprite = FindObjectOfType<TUTOSCRIPT>().players[nb].playerSprite;
        /*actualBaseSprite.sprite = null;
        actualFillerSprite.sprite = null;*/
        if (invisible == 0)
        {
            spriteRend.sprite = newSprite;
        }
        teamNb = nb;
        if (!FindObjectOfType<TUTOSCRIPT>())
            gameManager.WinCheck(teamNb, nb, contNbr);
        else
            FindObjectOfType<TUTOSCRIPT>().WinCheck(teamNb, nb);
        controller.anim.SetFloat("playerNbr", teamNb);
        #endregion

        if (!FindObjectOfType<TUTOSCRIPT>())
        {
            #region Change AIs team
            foreach (IAIdentity iA in gameManager.iATeams[contNbr])
            {
                iA.teamNb = teamNb;
                iA.gameObject.GetComponent<AIController>().ChangeTeam();
                if (iA.isInvisible == 0)
                {
                    iA.spriteRend.sprite = newSprite;
                }
            }
        }
            
        #endregion
    }

    public void SetInvisibility(bool state)
    {
        #region Go invisible
        if (state)
        {
            invisible += 1;
            if(invisible == 1)
            {
                spriteRend.sprite = gameManager.invisibleSprite;
                //debug
                Color spriteColor = spriteRend.color;
                spriteColor = new Color(spriteColor.r, spriteColor.g, spriteColor.b, 0.25f);
                spriteRend.color = spriteColor;
                //end debug
            }
        }
        #endregion

        #region Become Visible again
        else
        {
            invisible += -1;
            if(invisible == 0)
            {
                Sprite newSprite = gameManager.players[teamNb].playerSprite;
                spriteRend.sprite = newSprite;
                //debug
                Color spriteColor = spriteRend.color;
                spriteColor = new Color(spriteColor.r, spriteColor.g, spriteColor.b, 1f);
                spriteRend.color = spriteColor;
                //end debug
            }
        }
        #endregion
    }

    public void OnDieReset()
    {
        //GetComponent<Attack>().bounty = 0;
    }
}
