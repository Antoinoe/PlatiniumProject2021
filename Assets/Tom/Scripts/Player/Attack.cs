using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.InputSystem;
using UnityEngine.UI;
using System;

public class Attack : MonoBehaviour
{
    #region Variables

    //Kill target aquisition
    [HideInInspector] public List<GameObject> targets;
    private GameObject focusedTarget;

    //Kill CD
    [HideInInspector] public bool killOnCD = false;
    [SerializeField] private float killCooldown;

    //Score
    private int nbOfKills = 0;
    private float actualScore = 0;
    public int bounty = 0;

    //Mettez ça dans le GM Svp
    [SerializeField] private float scorePerKill = 10;
    [SerializeField] private int maxBounty = 4;


    public Text t_score, t_kills, t_bounty;
    #endregion

    private void Start()
    {
        
    }

    public void OnAttack(/*InputAction.CallbackContext context*/)
    {
        if (!killOnCD && focusedTarget)
        {
            if (focusedTarget.tag == "Player")
            {
                //Kill Player
                PlayerController killedPlayerScript = focusedTarget.GetComponent<PlayerController>();

                killedPlayerScript.ChangeTeam(GetComponent<PlayerController>().teamNb);
                killedPlayerScript.OnDieReset(); //reset le bounty du joueur tué 

                //PLAYER A FAIT UN KILL
                actualScore += bounty * bounty + scorePerKill; //calcule le score de Player B en fonction du bounty du player A //f(x) = x²+10

                if (bounty < maxBounty) //augmente bounty si pas au max
                    bounty++; 

                nbOfKills++; //player gagne un kill (c est plus pour le debug sur ma scene)
            }
            else if (focusedTarget.tag == "NPC")
            {
                //Kill NPC
            }

            killOnCD = true;
            StartCoroutine(KillCooldown(killCooldown));
        }
        else if (killOnCD)
        {
            Debug.Log("onCooldown");
        }
    }

    void Update()
    {
        if (Time.deltaTime == 0)
        {
            return;
        }

        #region Targets detection
        float distance = float.PositiveInfinity;
        GameObject temporaryTarget = null;

        if (targets.Count > 0)
        {
            for (int i = 0; i < targets.Count; i++)
            {
                if (targets[i] && targets[i] != gameObject)
                {
                    float targetDistance = Vector3.Distance(transform.position, targets[i].transform.position);
                    if (targetDistance < distance)
                    {
                        temporaryTarget = targets[i];
                    }
                }
                else
                {
                    targets.Remove(targets[i]);
                }
            }

           //Feedback visuel (highlight) 
            if (temporaryTarget != focusedTarget && !killOnCD)
            {
                /*if (focusedTarget != null)
                {
                    if (focusedTarget.tag == "NPC")
                    {
                        focusedTarget.GetComponent<AIBehaviour>().sightsNb += -1;
                    }
                    else if (focusedTarget.tag == "Player")
                    {
                        focusedTarget.GetComponent<PlayerController>().sightsNb += -1;
                    }

                    if (focusedTarget.tag == "NPC" && focusedTarget.GetComponent<AIBehaviour>().sightsNb == 0)
                    {
                        focusedTarget.GetComponent<AIBehaviour>().marker.SetActive(false);
                    }
                    else if (focusedTarget.tag == "Player" && focusedTarget.GetComponent<PlayerController>().sightsNb == 0)
                    {
                        focusedTarget.GetComponent<PlayerController>().marker.SetActive(false);
                    }
                }*/


            if (temporaryTarget && temporaryTarget.tag == "NPC")
                {
                    /*AIBehaviour controller = temporaryTarget.GetComponent<AIBehaviour>();
                    controller.sightsNb += 1;
                    controller.marker.SetActive(true);*/
                    focusedTarget = temporaryTarget;
                }
                else if (temporaryTarget && temporaryTarget.tag == "Player")
                {
                    /*PlayerController controller = temporaryTarget.GetComponent<PlayerController>();
                    controller.sightsNb += 1;
                    controller.marker.SetActive(true);*/
                    focusedTarget = temporaryTarget;
                }
            }
        }
        else
        {
            if (focusedTarget != null)
            {
                /*if (focusedTarget.tag == "NPC")
                {
                    focusedTarget.GetComponent<AIBehaviour>().sightsNb += -1;
                }
                else if (focusedTarget.tag == "Player")
                {
                    focusedTarget.GetComponent<PlayerController>().sightsNb += -1;
                }

                if (focusedTarget.tag == "NPC" && focusedTarget.GetComponent<AIBehaviour>().sightsNb == 0)
                {
                    focusedTarget.GetComponent<AIBehaviour>().marker.SetActive(false);
                }
                else if (focusedTarget.tag == "Player" && focusedTarget.GetComponent<PlayerController>().sightsNb == 0)
                {
                    focusedTarget.GetComponent<PlayerController>().marker.SetActive(false);
                }*/
                
                focusedTarget = null;
            }
        }
        #endregion

        #region Score
        if (Input.GetKeyDown(KeyCode.E))
        {
            //RESET
            actualScore = 0;
            bounty = 0;
            nbOfKills = 0;
        }

        //affiche les infos sur l ecran
        //t_score.text = "score :" + actualScore.ToString();
        //t_bounty.text = "bounty :" + bounty.ToString();
        //t_kills.text = "kills : " + nbOfKills.ToString();
        #endregion
    }

    private IEnumerator KillCooldown(float time)
    {
        yield return new WaitForSeconds(time);

        killOnCD = false;
        StopCoroutine(KillCooldown(1));
    }

    
}
