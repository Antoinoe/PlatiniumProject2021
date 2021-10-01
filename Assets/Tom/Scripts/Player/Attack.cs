using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Attack : MonoBehaviour
{
    #region Variables

    //Kill target aquisition
    [HideInInspector] public List<GameObject> targets;
    private GameObject focusedTarget;

    //Kill CD
    [HideInInspector] public bool killOnCD = false;
    [SerializeField] private float killCooldown;
    #endregion

    private void Start()
    {
        //Set trigger script
        AttackZone zoneScript = GetComponentInChildren<AttackZone>();
        zoneScript.playerScript = this;
    }

    public void OnAttack(/*InputAction.CallbackContext context*/)
    {
        if (!killOnCD && focusedTarget)
        {
            if (focusedTarget.tag == "Player")
            {
                //Kill Player
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
    }

    private IEnumerator KillCooldown(float time)
    {
        yield return new WaitForSeconds(time);

        killOnCD = false;
        StopCoroutine(KillCooldown(1));
    }
}
