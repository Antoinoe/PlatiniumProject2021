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

    UIManager ui;

    //Kill CD
    [HideInInspector] public bool killOnCD = false;
    [SerializeField] private float killCooldown; //TO UI
    /*[HideInInspector]*/
    public float passedTime = 0f; //TO UI

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

    //Mettez �a dans le GM Svp
    [SerializeField] private float scorePerKill = 10;
    [SerializeField] private int maxBounty = 4;

    public Text t_score, t_kills, t_bounty;*/

    [SerializeField] private GameObject attackFX;
    [SerializeField] private Sprite attackYSprite;
    #endregion

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        controller = GetComponent<Controller>();
        ui = FindObjectOfType<UIManager>();
    }


    private void OnDrawGizmos()
    {
        if (Application.isPlaying && controller.ShowGizmos)
        {
            Gizmos.color = Color.red;
            
            if(controller.MovementVector != Vector2.zero)
            {
                Transform gizmosTransform = new GameObject().transform;
                gizmosTransform.rotation = Quaternion.Euler(controller.MovementVector);

                Gizmos.matrix = gizmosTransform.localToWorldMatrix;
                Gizmos.DrawWireCube((Vector2)transform.position + controller.MovementVector * (attackRange / 2), new Vector3(targetDetectionBoxSize.x * attackRange, targetDetectionBoxSize.y, 0));
            }
            else
            {
                GameObject newObject = new GameObject();
                Transform gizmosTransform = newObject.transform;
                GameObject.Destroy(newObject, 0);

                Gizmos.matrix = gizmosTransform.localToWorldMatrix;
                Gizmos.DrawWireCube(transform.position, new Vector3(targetDetectionBoxSize.x, targetDetectionBoxSize.y, 0));
            }

        }
    }

    public void OnAttack()
    {
        if (!killOnCD)
        {
            #region Targets aquisition
            List<GameObject> targets = new List<GameObject>();
            Collider2D[] collidersInRange;

            if (controller.MovementVector != Vector2.zero)
            {
               collidersInRange = Physics2D.OverlapBoxAll((Vector2)transform.position + controller.MovementVector * (attackRange / 2), new Vector2(targetDetectionBoxSize.x * attackRange, targetDetectionBoxSize.y), Vector2.SignedAngle(Vector2.right, controller.MovementVector));
            }
            else
            {
               collidersInRange = Physics2D.OverlapBoxAll((Vector2)transform.position, new Vector2(targetDetectionBoxSize.x, targetDetectionBoxSize.y), 0);
            }

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
                        if (targets[i].CompareTag("Player"))
                        {
                            target = targets[i];
                            break;
                        }
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
            if (target)
            {

                if (target.CompareTag("Player"))
                {
                    //Kill Player
                    PlayerController killedPlayerScript = target.GetComponent<PlayerController>();
                    if (killedPlayerScript.isInvincible) return;

                    killedPlayerScript.ChangeTeam(playerController.teamNb, playerController.contNbr);

                    if (!FindObjectOfType<TUTOSCRIPT>())
                    {
                        playerController.gameManager.Shake();
                        playerController.gameManager.SpawnSmoke(transform.position, playerController.teamNb);
                    } else
                    {

                        FindObjectOfType<TUTOSCRIPT>().Shake();
                        FindObjectOfType<TUTOSCRIPT>().SpawnSmoke(transform.position, playerController.teamNb);
                    }
                        

                    playerController.OnKill(false);
                    playerController.SetInv();
                    /*killedPlayerScript.OnDieReset(); //reset le bounty du joueur tu� 

                    //PLAYER A FAIT UN KILL
                    actualScore += bounty * bounty + scorePerKill; //calcule le score de Player B en fonction du bounty du player A //f(x) = x�+10

                    if (bounty < maxBounty) //augmente bounty si pas au max
                        bounty++; 

                    nbOfKills++; //player gagne un kill (c est plus pour le debug sur ma scene)*/
                    if (!FindObjectOfType<TUTOSCRIPT>())
                    {
                        FindObjectOfType<AudioManager>().Play("Attack");
                        FindObjectOfType<AudioManager>().Play("Smoke");
                    }
                        
                    spawnFX(controller.MovementVector * attackRange);
                    //StartCoroutine(KillCooldown(playerController.CurrKillCooldown));
                    controller.anim.SetTrigger("doAttack");
                    killCooldown = playerController.CurrKillCooldown;
                    killOnCD = true;
                    if (!FindObjectOfType<TUTOSCRIPT>())
                        FindObjectOfType<UIManager>().EmptyBar(GetComponent<PlayerController>().contNbr);
                }
                else if (target.CompareTag("NPC"))
                {
                    //Kill NPC
                    //Debug.Log("NPC killed by Team " + playerController.teamNb);
                    if (target.GetComponent<IAIdentity>().teamNb != playerController.teamNb) { target.GetComponent<AIController>().OnKilled(); playerController.gameManager.Shake(); }
                    //else { target.GetComponent<AIController>().OnBone(); }

                    playerController.OnKill(true);
                    FindObjectOfType<AudioManager>().Play("PunchIA");
                    spawnFX(controller.MovementVector * attackRange);
                    //StartCoroutine(KillCooldown(playerController.CurrKillCooldown));
                    controller.anim.SetTrigger("doAttack");
                    killCooldown = playerController.CurrKillCooldown;
                    killOnCD = true;
                    FindObjectOfType<UIManager>().EmptyBar(GetComponent<PlayerController>().contNbr);
                }                       
            }
            else
            {
                //Debug.Log("noTarget");
            }
            #endregion
        }
        else
        {
            //Debug.Log("onCooldown");
        }
    }

    void Update()
    {
        if (!FindObjectOfType<TUTOSCRIPT>())
        {
            if (killOnCD)
            {
                if (controller.MovementVector != Vector2.zero)
                {
                    killCooldown -= Time.deltaTime;
                    float val = (playerController.CurrKillCooldown - killCooldown) / playerController.CurrKillCooldown;
                    ui.UpdateCooldownOnUI(playerController.contNbr, val);
                }

                if (killCooldown <= 0) killOnCD = false;
            }
        } else
        {
            killOnCD = false;
        }
            
    }

    /*private IEnumerator KillCooldown(float moveTime)
    {
        Debug.Log(controller.MovementVector);
        if (controller.MovementVector != Vector2.zero)
        {
            passedTime += Time.deltaTime;
        }

        if (passedTime < moveTime)
        {
            yield return new WaitForSeconds(Time.deltaTime);
        }
        else
        {
            passedTime = 0f;
            killOnCD = false;
            StopCoroutine(KillCooldown(moveTime));
        }
    }*/

    public void spawnFX(Vector2 dir)
    {
        float angle = Vector2.SignedAngle(Vector2.right, dir);
        GameObject fist = GameObject.Instantiate(attackFX, transform.position, Quaternion.Euler(0, 0, angle));

        if (dir.x < 0)
        {
            Vector3 scale = fist.transform.localScale;
            fist.transform.localScale = new Vector3(scale.x, -scale.y, scale.z);
        }
        if(dir.y < -0.5 || dir.y > 0.5)
        {
            fist.GetComponentInChildren<SpriteRenderer>().sprite = attackYSprite;
        }

        GameObject.Destroy(fist, 0.35f);
    }
}
