using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackZone : MonoBehaviour
{
    [HideInInspector] public PlayerController playerScript;
    [HideInInspector] public Attack playerAttack;

    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log("ouille");
        GameObject collidingObject = collision.gameObject;
        if (collidingObject && (collidingObject.tag == "Player" && collidingObject.GetComponent<PlayerController>().teamNb != playerScript.teamNb) | collidingObject.tag == "NPC" && collidingObject.GetComponent<IAIdentity>().teamNb != playerScript.teamNb)
        {
            playerAttack.targets.Add(collidingObject);
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        GameObject collidingObject = collision.gameObject;
        if (collidingObject.tag == "Player" | collidingObject.tag == "NPC")
        {
            playerAttack.targets.Remove(collidingObject);
        }
    }
}
