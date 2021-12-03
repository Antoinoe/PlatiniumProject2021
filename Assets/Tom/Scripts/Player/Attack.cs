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
    /*[HideInInspector] public List<GameObject> targets;*/
    //private GameObject focusedTarget;

    //Player Controller
    private PlayerController playerController;
    private Controller controller;

    //Kill CD
    [HideInInspector] public bool killOnCD = false;
    [SerializeField] private float killCooldown;
    /*[HideInInspector]*/ public float passedTime = 0f;

    //Target detection
    [SerializeField] private float attackRange;
    [SerializeField] private Vector2 targetDetectionBoxSize;

    public Vector2 BoxLength
    {
        get { return targetDetectionBoxSize; }
        set { targetDetectionBoxSize = value; }
    }

    /*//Score
    private int nbOfKills = 0;
    private float actualScore = 0;
    public int bounty = 0;

    //Mettez ça dans le GM Svp
    [SerializeField] private float scorePerKill = 10;
    [SerializeField] private int maxBounty = 4;

    public Text t_score, t_kills, t_bounty;*/
    #endregion

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        controller = GetComponent<Controller>();
    }


    private void OnDrawGizmos()
    {
        if (Application.isPlaying && controller.ShowGizmos)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube((Vector2)transform.position + controller.MovementVector * attackRange, new Vector3(targetDetectionBoxSize.x, targetDetectionBoxSize.y, 0));
        }
    }

    public void OnAttack()
    {
        #region Targets aquisition
        List<GameObject> targets = new List<GameObject>();

        Collider2D[] collidersInRange = Physics2D.OverlapBoxAll((Vector2)transform.position + controller.MovementVector * attackRange, targetDetectionBoxSize, 0);
        foreach (Collider2D c in collidersInRange)
        {
            GameObject collidingObject = c.gameObject;

            if (collidingObject && collidingObject != gameObject)
            {
                if (collidingObject.CompareTag("Player"))
                {
                    if (collidingObject.GetComponent<PlayerController>().teamNb != playerController.teamNb)
                    {
                        targets.Add(collidingObject);
                    }
                }
                else if (collidingObject.CompareTag("NPC"))
                {
                    targets.Add(collidingObject);
                }

            }
        }
        #endregion

        #region Attack target aquisition
        float distance = float.PositiveInfinity;
        GameObject target = null;

        if (targets.Count > 0)
        {
            for (int i = 0; i < targets.Count; i++)
            {
                if (targets[i] && targets[i] != gameObject)
                {
                    float targetDistance = Vector3.Distance(transform.position, targets[i].transform.position);
                    if (targetDistance < distance)
                    {
                        target = targets[i];
                    }
                }
            }
        }
        #endregion

        #region Attack !
        if (!killOnCD && target)
        {
            if (target.CompareTag("Player"))
            {
                //Kill Player
                PlayerController killedPlayerScript = target.GetComponent<PlayerController>();

                killedPlayerScript.ChangeTeam(playerController.teamNb);

                playerController.gameManager.Shake();
                playerController.gameManager.SpawnSmoke(transform.position, playerController.teamNb);

                /*killedPlayerScript.OnDieReset(); //reset le bounty du joueur tué 

                //PLAYER A FAIT UN KILL
                actualScore += bounty * bounty + scorePerKill; //calcule le score de Player B en fonction du bounty du player A //f(x) = x²+10

                if (bounty < maxBounty) //augmente bounty si pas au max
                    bounty++; 

                nbOfKills++; //player gagne un kill (c est plus pour le debug sur ma scene)*/
            }
            else if (target.CompareTag("NPC"))
            {
                //Kill NPC
                //Debug.Log("NPC killed by Team " + playerController.teamNb);
                if (target.GetComponent<IAIdentity>().teamNb != playerController.teamNb) { target.GetComponent<AIController>().OnKilled(); playerController.gameManager.Shake(); }
                else { target.GetComponent<AIController>().OnBone(); }

                
            }


            playerController.OnKill(target);
            killOnCD = true;
        }
        else if (killOnCD)
        {
            Debug.LogError("onCooldown : " + playerController.teamNb);
            RectTransform slider = GameObject.Find("Slider0" + (playerController.teamNb + 1)).GetComponent<RectTransform>();
            StartCoroutine(ShakeIt(slider));
            //Debug.LogError(playerController.CurrKillCooldown);
        }
        else
        {
            Debug.Log("noTarget");
        }
        #endregion
    }
    private IEnumerator ShakeIt(RectTransform transform)
    {
        for(int i = 0; i < 20; i++)
        {
            transform.anchoredPosition = UnityEngine.Random.insideUnitCircle * 5f;
            yield return new WaitForFixedUpdate();
        }
        yield break;
    }
    void Update()
    {
        if (Time.deltaTime == 0)
        {
            return;
        }

        #region Targets detection
        /*float distance = float.PositiveInfinity;
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
                if (focusedTarget != null)
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
                }


                if (temporaryTarget && temporaryTarget.tag == "NPC")
                {
                    AIBehaviour controller = temporaryTarget.GetComponent<AIBehaviour>();
                    controller.sightsNb += 1;
                    controller.marker.SetActive(true);
                    focusedTarget = temporaryTarget;
                }
                else if (temporaryTarget && temporaryTarget.tag == "Player")
                {
                    PlayerController controller = temporaryTarget.GetComponent<PlayerController>();
                    controller.sightsNb += 1;
                    controller.marker.SetActive(true);

                    if (temporaryTarget.GetComponent<PlayerController>().teamNb != playerController.teamNb)
                    {
                        focusedTarget = temporaryTarget;
                    }
                }
            }
        }
        else
        {
            if (focusedTarget != null)
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
        //t_kills.text = "kills : " + nbOfKills.ToString();*/
        #endregion
    }

    private IEnumerator KillCooldown(float moveTime)
    {
        Debug.Log(controller.MovementVector);
        if(controller.MovementVector != Vector2.zero)
        {
            passedTime += Time.deltaTime;
        }

        if(passedTime < moveTime)
        {
            Debug.Log("aurevoir");
            yield return new WaitForSeconds(Time.deltaTime);
        }
        else
        {
            Debug.Log("bonjour");
            passedTime = 0f;
            killOnCD = false;
            StopCoroutine(KillCooldown(moveTime));
        }
    }
}
