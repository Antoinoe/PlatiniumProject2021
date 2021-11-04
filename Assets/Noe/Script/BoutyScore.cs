using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BoutyScore : MonoBehaviour
{
    public int nbOfKills = 0;
    public float actualScore = 0;
    public int bounty = 0;
    public float scorePerKill = 10;
    public int maxBounty = 4;

    public Text t_score, t_kills, t_bounty;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            //PLAYER A FAIT UN KILL
            if (bounty < maxBounty)
                bounty++;
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            //PLAYER A SE FAIT KILL ET PERD SON BOUNTY
            actualScore += /*scorePerKill * (1 + (bounty * bountyRatio));*/ bounty * bounty + scorePerKill; //calcule le score de Player B en fonction du bounty du player A //f(x) = x²+10
            bounty = 0; //player A perd son bounty
            nbOfKills++; //player B gagne un kill (c est plus pour le debug sur ma scene)

        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            //RESET
            actualScore = 0;
            bounty = 0;
            nbOfKills = 0;

        }

        //affiche les infos sur l ecran
        t_score.text = "score :" + actualScore.ToString();
        t_bounty.text = "bounty :" + bounty.ToString();
        t_kills.text = "kills : " + nbOfKills.ToString();
    }
}
